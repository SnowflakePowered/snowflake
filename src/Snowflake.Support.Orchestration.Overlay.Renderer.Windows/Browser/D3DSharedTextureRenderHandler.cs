using CefSharp;
using CefSharp.Enums;
using CefSharp.OffScreen;
using CefSharp.Structs;
using Evergine.Bindings.RenderDoc;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using Snowflake.Orchestration.Ingame;
using Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Browser;
using Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Remoting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Renderer.Windows
{
    internal class D3DSharedTextureRenderHandler : IRenderHandler
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Guid* RiidOf(Guid @in)
        {
            return &@in;
        }

        [DllImport("user32.dll")]
        private static extern nint MonitorFromWindow(nint hwnd, uint dwFlags);
        [DllImport("shcore.dll")]
        private static extern void GetScaleFactorForMonitor(nint hMon, out uint pScale);

        [DllImport("kernel32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern bool CloseHandle(IntPtr hObject);

        // Texture paint stuff.
        private ConcurrentQueue<(nint texturePointer, nint sharedHandle)> ObsoleteResources { get; }

        private unsafe ID3D11Texture2D* TargetTexture;

        private Texture2DDesc TargetTextureDescription;
        public nint SharedTextureHandle { get; set; }

        // D3D device is plugin-wide.
        private Direct3DDevice Device { get; }
        private float DpiScaleFactor { get; }
        private readonly object TextureLock = new();

        // CEF buffers are 32-bit BGRA
        private const byte CEFBufferBPP = 4;
        private const uint INFINITE = 0xFFFFFFFF;

        // Command server to notify ppl
        private IngameCommandController CommandServer { get; }
        public RenderDoc RenderDoc { get; }

        public unsafe D3DSharedTextureRenderHandler(Direct3DDevice device, IngameCommandController commandServer, RenderDoc renderDoc)
        {
            this.CommandServer = commandServer;
            RenderDoc = renderDoc;
            this.ObsoleteResources = new ConcurrentQueue<(nint texturePointer, nint sharedHandle)>();
            // Todo: ask ingame for scale monitor handle.
            nint hMon = MonitorFromWindow(0, 0x1);
            GetScaleFactorForMonitor(hMon, out uint scale);
            this.DpiScaleFactor = scale / 100f;
            this.Device = device;
            this.CommandServer.CommandReceived += CommandReceivedHandler;
        }

        private void CommandReceivedHandler(GameWindowCommand command)
        {
            if (command.Type == GameWindowCommandType.OverlayTextureEvent)
                this.CommandServer.Broadcast(new()
                {
                    Magic = GameWindowCommand.GameWindowMagic,
                    Type = GameWindowCommandType.OverlayTextureEvent,
                    TextureEvent = new()
                    {
                        SourceProcessId = Environment.ProcessId,
                        TextureHandle = this.SharedTextureHandle,
                        Width = this.TargetTextureDescription.Width,
                        Height = this.TargetTextureDescription.Height,
                        Size = this.TargetTextureDescription.Width * this.TargetTextureDescription.Height * CEFBufferBPP * 2
                    }
                });
        }

        public ScreenInfo? GetScreenInfo()
        {
            return new()
            {
                DeviceScaleFactor = this.DpiScaleFactor
            };
        }

        public bool GetScreenPoint(int viewX, int viewY, out int screenX, out int screenY)
        {
            screenX = viewX;
            screenY = viewY;

            return false;
        }

        public void Resize(System.Drawing.Size size, bool force = false)
        {
            Console.WriteLine("Resize buffer requested");
            if (!force && size.Height == this.TargetTextureDescription.Height && size.Width == this.TargetTextureDescription.Width)
            {
                Console.WriteLine("Resize would not change size, throttling.");
                return;
            }

            nint texPtr = this.Device.CreateNewCefTargetTexture(size);
            unsafe
            {
                // Released when disposed in OnPaint.
                ID3D11Texture2D* texture = (ID3D11Texture2D*)texPtr;

                // released on resize.
                IDXGIResource1* texResrc = null;
                lock (this.TextureLock)
                {
                    nint oldTexture = (nint)this.TargetTexture;
                    nint oldHandle = this.SharedTextureHandle;

                    texture->QueryInterface(RiidOf(IDXGIResource1.Guid), (void**)&texResrc);

                    int res;
                    void* handle = null;
                    if ((res = texResrc->CreateSharedHandle(null, unchecked((uint)0x80000000ul), (char*)null, &handle)) != 0)
                    {
                        throw new InvalidOperationException($"Unable to update shared handled: {res}");
                    }

                    texture->GetDesc(ref this.TargetTextureDescription);
                    this.SharedTextureHandle = (nint)handle;
                    this.TargetTexture = texture;
                    this.ObsoleteResources.Enqueue((oldTexture, oldHandle));
                    
                    // release resource
                    texResrc->Release();
                    Console.WriteLine("updated buffer");
                }
            }

            // Broadcast to all listeners to update their texture handle.
            this.CommandServer.Broadcast(new()
            {
                Magic = GameWindowCommand.GameWindowMagic,
                Type = GameWindowCommandType.OverlayTextureEvent,
                TextureEvent = new()
                {
                    TextureHandle = this.SharedTextureHandle,
                    SourceProcessId = Environment.ProcessId,
                    Width = this.TargetTextureDescription.Width,
                    Height = this.TargetTextureDescription.Height,
                    Size = this.TargetTextureDescription.Width * this.TargetTextureDescription.Height * CEFBufferBPP * 2,
                }
            });
        }

        public Rect GetViewRect()
        {
            // thanks browsingway.
            static Rect DpiScaleRect(Rect rect, float scaleFactor)
            {
                return new Rect(rect.X, rect.Y, (int)Math.Ceiling(rect.Width * (1 / scaleFactor)), 
                    (int)Math.Ceiling(rect.Height * (1 / scaleFactor)));
            }

            // todo: scale dpi
            return DpiScaleRect(new(0, 0, (int)this.TargetTextureDescription.Width,
                (int)this.TargetTextureDescription.Height), this.DpiScaleFactor);
        }

        public void OnCursorChange(IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
        {
            this.CommandServer.Broadcast(new()
            {
                Magic = GameWindowCommand.GameWindowMagic,
                Type = GameWindowCommandType.CursorEvent,
                CursorEvent = new() { Cursor = (Cursor)(byte)type, }
            });
        }

        public void OnPaint(PaintElementType type, Rect dirtyRect, IntPtr buffer, int width, int height)
        {
            // Don't care about popups.
            if (type != PaintElementType.View)
                return;
            lock(this.TextureLock)
            {
                unsafe
                {
                    // not initialized.
                    if (this.TargetTexture == null)
                        return;
                }

                // thanks browsingway
                int rowPitch = width * CEFBufferBPP;
                int depthPitch = rowPitch * height;

                var texDesc = this.TargetTextureDescription;
                Box destRegion = new()
                {
                    Top = Math.Min(unchecked((uint)dirtyRect.Y), texDesc.Height),
                    Bottom = Math.Min(unchecked((uint)dirtyRect.Y) + unchecked((uint)dirtyRect.Height), texDesc.Height),
                    Left = Math.Min(unchecked((uint)dirtyRect.X), texDesc.Width),
                    Right = Math.Min(unchecked((uint)dirtyRect.X) + unchecked((uint)dirtyRect.Width), texDesc.Width),
                    Front = 0,
                    Back = 1
                };

                unsafe
                {
                    nint sourcePtr = buffer + (dirtyRect.X * CEFBufferBPP) + (dirtyRect.Y * rowPitch);

                    ID3D11Device* device = null;
                    ID3D11DeviceContext* context = null;
                    ID3D11Resource* textureResc = null;
                    // this is going to be a pain isnt it.
                    IDXGIKeyedMutex* textureMtx = null;

                    this.TargetTexture->QueryInterface(RiidOf(ID3D11Resource.Guid), (void**)&textureResc);
                    this.TargetTexture->QueryInterface(RiidOf(IDXGIKeyedMutex.Guid), (void**)&textureMtx);

                    this.TargetTexture->GetDevice(ref device);
                    device->GetImmediateContext(ref context);
                    //this.RenderDoc.API.StartFrameCapture((nint)context, (IntPtr)null);

                    textureMtx->AcquireSync(0, INFINITE); // infinite
                    context->UpdateSubresource(textureResc, 0, ref destRegion, (void*)sourcePtr, (uint)rowPitch, (uint)depthPitch);
                    context->Flush();
                    textureMtx->ReleaseSync(0);

                    //this.RenderDoc.API.EndFrameCapture((nint)context, (IntPtr)null);


                    // ensure we release all local COM pointers here.
                    // really wish C# had traits 
                    textureResc->Release();
                    textureMtx->Release();
                    context->Release();
                    device->Release();
                }

                // cleanup..
                while (this.ObsoleteResources.TryDequeue(out (nint texturePointer, nint sharedHandle) texture)) 
                {
                    unsafe 
                    {
                        // Honestly should check error but if this fails we just leak it.
                        // Expectation is that Resize is always client-side triggered anyways,
                        // so if their handle is properly duped this doesn't matter.
                        CloseHandle(texture.sharedHandle);
                
                        // Texture will _actually_ be freeded when all clients acually let go of it.
                        if (texture.texturePointer != 0)
                            ((ID3D11Texture2D*)texture.texturePointer)->Release();
                    }
                }
            }
        }

        public void Dispose()
        {
            // todo...

            return;
        }

        #region Not Supported
        public void OnAcceleratedPaint(PaintElementType type, Rect dirtyRect, IntPtr sharedHandle)
        {
            // Not supported.
            return;
        }

        public void OnImeCompositionRangeChanged(CefSharp.Structs.Range selectedRange, Rect[] characterBounds)
        {
            // Not supported.
            return;
        }


        public void OnPopupShow(bool show)
        {
            // Not supported.
            return;
        }

        public void OnPopupSize(Rect rect)
        {
            // Not supported.
            return;
        }

        public void OnVirtualKeyboardRequested(IBrowser browser, TextInputMode inputMode)
        {
            // Not supported.
            return;
        }

        public bool StartDragging(IDragData dragData, DragOperationsMask mask, int x, int y)
        {
            // not supported.
            return false;
        }

        public void UpdateDragCursor(DragOperationsMask operation)
        {
            return;
        }
        #endregion
    }
}
