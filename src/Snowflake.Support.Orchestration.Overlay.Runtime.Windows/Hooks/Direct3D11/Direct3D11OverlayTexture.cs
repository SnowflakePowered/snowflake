using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D11
{
    internal class Direct3D11OverlayTexture
    {
        private unsafe IDXGIKeyedMutex* overlayTextureMutex = null;
        private unsafe ID3D11ShaderResourceView* overlayShaderResourceView = null;
        private unsafe ID3D11Texture2D* overlayTexture = null;

        private Texture2DDesc overlayTextureDesc = new();
        private IntPtr overlayTextureHandle = IntPtr.Zero;
        private nint outputWindowHandle = 0;

        public readonly object TextureMutex = new();

        private bool ReadyToPaint { get; set; }
        public bool ReadyToInitialize => this.overlayTextureHandle != IntPtr.Zero;

        public void AcquireSync()
        {
            unsafe {
                this.overlayTextureMutex->AcquireSync(0, unchecked((uint)-1));
            }
        }

        public void ReleaseSync()
        {
            unsafe
            {
                this.overlayTextureMutex->ReleaseSync(0);
            }
        }

        public bool Refresh(int owningPid, nint textureHandle)
        {
            var process = Kernel32.OpenProcess(new(Kernel32.ProcessAccess.PROCESS_DUP_HANDLE), false, (uint)owningPid);
            if (process.IsNull)
            {
                Console.WriteLine("unable to open source process...");
                return false;
            }

            if (!Kernel32.DuplicateHandle(process, textureHandle, Kernel32.GetCurrentProcess(), out IntPtr dupedHandle,
                0, false, Kernel32.DUPLICATE_HANDLE_OPTIONS.DUPLICATE_SAME_ACCESS))
            {
                Console.WriteLine("unable to dupe handle");
                return false;
            };

            Console.WriteLine($"Got owned handle {dupedHandle.ToString("x")}");

            // Release old texture
            unsafe
            {
                if (this.overlayTexture != null)
                {
                    this.Invalidate();
                }
            }
           
            // close old the handle
            Kernel32.CloseHandle(this.overlayTextureHandle);

            // new texture will be fetched on next paint.
            this.overlayTextureHandle = dupedHandle;
            return true;
        }

        private unsafe void Invalidate()
        {
            lock (this.TextureMutex)
            {
                if (this.overlayShaderResourceView != null)
                    this.overlayShaderResourceView->Release();

                if (this.overlayTextureMutex != null)
                    this.overlayTextureMutex->Release();

                if (this.overlayTexture != null)
                    this.overlayTexture->Release();

                this.overlayTexture = null;
                this.overlayTextureMutex = null;
                this.overlayShaderResourceView = null;

                this.ReadyToPaint = false;
            }
        }

        public unsafe bool PrepareForPaint(ComPtr<ID3D11Device1> device1Handle, nint outputWindowHandle)
        {
            if (this.ReadyToPaint && this.outputWindowHandle == outputWindowHandle)
            {
                return true;
            }

            this.Invalidate();

            ID3D11ShaderResourceView* texSRV = null; // moved to this.overlayShaderResourceView

            using ComPtr<ID3D11Texture2D> tex2D =
                device1Handle.Cast<ID3D11Texture2D>((p, g, o) => 
                    p->OpenSharedResource1(this.overlayTextureHandle.ToPointer(), g, o), ID3D11Texture2D.Guid, static d => d->Release(), out int res);

            using ComPtr<ID3D11Device> deviceHandle = 
                device1Handle.Cast<ID3D11Device>(static (p, g, o) => p->QueryInterface(g, o), ID3D11Device.Guid, static d => d->Release());

            if (res != 0)
            {
                Console.WriteLine($"Unable to open shared texture handle {this.overlayTextureHandle}: {res.ToString("x")}.");
                return false;
            }

            // todo: error check
            Console.WriteLine("Opened shared texture.");
            using var texMtx = tex2D.Cast<IDXGIKeyedMutex>(static (p, g, o) => p->QueryInterface(g, o), IDXGIKeyedMutex.Guid, static d => d->Release());

            if (~texMtx == null)
            {
                Console.WriteLine("Mutex not yet ready, can not open shared texture.");

                // not ready yet.
                return false;

            }

            Texture2DDesc tex2dDesc = new(); // dropped 

            tex2D.Ref.GetDesc(ref tex2dDesc);
            using var texRsrc = tex2D.Cast<ID3D11Resource>(static (p, g, o) => p->QueryInterface(g, o), ID3D11Resource.Guid, static d => d->Release());

            this.overlayTextureMutex = texMtx.Forget();
            this.overlayTexture = tex2D.Forget();
            this.overlayTextureDesc = tex2dDesc;

            ShaderResourceViewDesc srvDesc = new() // dropped
            {
                Format = tex2dDesc.Format,
                ViewDimension = D3DSrvDimension.D3D101SrvDimensionTexture2D,
                Anonymous = new(texture2D: new()
                {
                    MipLevels = tex2dDesc.MipLevels,
                    MostDetailedMip = 0,
                })
            };

            // not worth using ComPtr for texSrc here because capture and lifetime of texSRV..
            deviceHandle.Ref.CreateShaderResourceView(texRsrc, ref srvDesc, ref texSRV);

            // Get text src
            this.overlayShaderResourceView = texSRV;
            this.outputWindowHandle = outputWindowHandle;

            this.ReadyToPaint = true;
            return true;
        }

        public void Paint(Action<nint, uint, uint> renderFn)
        {
            lock (this.TextureMutex)
            {
                unsafe
                {
                    renderFn((nint)this.overlayShaderResourceView, overlayTextureDesc.Width, overlayTextureDesc.Height);
                }
            }
        }
    }
}
