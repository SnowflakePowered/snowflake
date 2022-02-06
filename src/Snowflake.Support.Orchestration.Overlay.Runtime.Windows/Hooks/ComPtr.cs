using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks
{
    internal ref struct ComPtr<TPtr> where TPtr : unmanaged
    {
        public unsafe readonly ref TPtr Ref => ref *_pointer;

        public unsafe delegate void ComPtrRelease<TComRelease>(TComRelease* source)
            where TComRelease : unmanaged;

        private ComPtrRelease<TPtr>? Release { get; }
        private bool _forgotten = false;
        private unsafe TPtr* _pointer;
        public unsafe void** OutPointer => (void**)AddressOf(this._pointer);

        public unsafe ComPtr(TPtr* nonOwningPointer)
        {
            this._pointer = nonOwningPointer;
            this.Release = null;
        }

        public unsafe ComPtr(ComPtrRelease<TPtr> release)
        {
            this._pointer = null;
            this.Release = release;
        }

        public unsafe ComPtr(TPtr* pointer, ComPtrRelease<TPtr> release)
        {
            this._pointer = pointer;
            this.Release = release;
        }

        public unsafe delegate void ComPtrCast<TSourcePtr>(TSourcePtr* source, Guid* riid, void** outPtr)
            where TSourcePtr: unmanaged;
        public unsafe delegate int ComPtrCastInt<TSourcePtr>(TSourcePtr* source, Guid* riid, void** outPtr)
            where TSourcePtr : unmanaged;

        public unsafe delegate void ComPtrCastT<TSourcePtr, TOutPtr>(TSourcePtr* source, TOutPtr** outPtr)
            where TSourcePtr : unmanaged 
            where TOutPtr : unmanaged;

        public unsafe ComPtr<TOut> Cast<TOut>(ComPtrCast<TPtr> cast, Guid riid, ComPtr<TOut>.ComPtrRelease<TOut>? release)
            where TOut: unmanaged
        {
            TOut* outPointer = null;
            cast(this._pointer, &riid, (void**)&outPointer);
            return new(outPointer, release);
        }

        public unsafe ComPtr<TOut> Cast<TOut>(ComPtrCastInt<TPtr> cast, Guid riid, ComPtr<TOut>.ComPtrRelease<TOut>? release, out int result)
            where TOut : unmanaged
        {
            TOut* outPointer = null;
            result = cast(this._pointer, &riid, (void**)&outPointer);
            return new(outPointer, release);
        }

        public unsafe ComPtr<TOut> Cast<TOut>(ComPtrCastT<TPtr, TOut> cast, ComPtr<TOut>.ComPtrRelease<TOut>? release)
         where TOut : unmanaged
        {
            TOut* outPointer = null;
            cast(this._pointer, &outPointer);
            return new(outPointer, release);
        }

        public unsafe TPtr* Forget()
        {
            this._forgotten = true;
            TPtr* @this = this._pointer;
            this._pointer = null;
            return @this;
        }

        public static unsafe TPtr* operator ~(ComPtr<TPtr> ptr) => ptr._pointer;

        public static unsafe implicit operator TPtr* (ComPtr<TPtr> ptr) => ptr._pointer;
        public static unsafe implicit operator TPtr**(ComPtr<TPtr> ptr) => AddressOf(ptr._pointer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (_forgotten)
                return;
            unsafe
            {
                if (this._pointer == null)
                    return;
                this.Release?.Invoke(this._pointer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe TPtr** AddressOf(TPtr* @in) => &@in;
    }
}
