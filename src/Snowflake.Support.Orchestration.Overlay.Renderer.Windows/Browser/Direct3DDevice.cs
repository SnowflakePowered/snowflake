using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Browser
{
    internal class Direct3DDevice : IDisposable
    {
        private D3D11 Direct3D;
        private unsafe ID3D11Device* RenderDevice;
        private unsafe ID3D11DeviceContext* RenderContext;
        private bool disposedValue;
        private static readonly D3DFeatureLevel[] FEATURE_LEVELS = new[] { D3DFeatureLevel.D3DFeatureLevel111 };

        public Direct3DDevice()
        {
            this.Direct3D = D3D11.GetApi();
            unsafe
            {
                Span<D3DFeatureLevel> requestFeatureLevels = FEATURE_LEVELS.AsSpan();
                D3DFeatureLevel outFeatureLevel = 0;

                // Released on dispose.
                ID3D11Device* device = null;
                ID3D11DeviceContext* context = null;

                int? result = 0;
                fixed (D3DFeatureLevel* featureLevels = requestFeatureLevels)
                {
                    result = Direct3D.CreateDevice(
                    null,
                    D3DDriverType.D3DDriverTypeHardware,
                    0,
                    (uint)(CreateDeviceFlag.CreateDeviceBgraSupport|CreateDeviceFlag.CreateDeviceDebug),
                    featureLevels,
                    (uint)requestFeatureLevels.Length,
                    D3D11.SdkVersion,
                    ref device,
                    ref outFeatureLevel,
                    ref context
                    );
                }

                if (result == null || result.Value < 0)
                {
                    throw new PlatformNotSupportedException("Direct3D11 not supported.");
                }

                this.RenderDevice = device;
                this.RenderContext = context;
            }
        }

        /// <summary>
        /// Creates a target texture to paint CEF with the given size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public nint CreateNewCefTargetTexture(Size size)
        {
            Texture2DDesc texture2DDesc = new() 
            {
                Width = (uint)size.Width,
                Height = (uint)size.Height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Silk.NET.DXGI.Format.FormatB8G8R8A8Unorm,
                SampleDesc = new(1,0),
                Usage = Usage.UsageDefault,
                BindFlags = (uint)(BindFlag.BindShaderResource),
                // needs NT Handle for DX12/OGL/vK interop
                MiscFlags = (uint)(ResourceMiscFlag.ResourceMiscSharedNthandle | ResourceMiscFlag.ResourceMiscSharedKeyedmutex),
                CPUAccessFlags = 0,
            };

            unsafe
            {
                ID3D11Texture2D* texture = null;
                int result = 0;
                if ((result = this.RenderDevice->CreateTexture2D(ref texture2DDesc, null, ref texture)) != 0)
                {
                    throw new InvalidOperationException($"Failed to create D3D11 Texture: {result}");
                }
                return (nint)texture;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    unsafe
                    {
                        this.RenderDevice->Release();
                        this.RenderContext->Release();
                    }
                }
                disposedValue = true;
            }
        }

  
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
