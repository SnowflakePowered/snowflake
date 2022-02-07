using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks
{
    /// <summary>
    /// Lightweight helper struct to help manage COM pointer lifetimes.
    /// </summary>
    /// <typeparam name="TPtr">The type of the pointer to wrap</typeparam>
    internal ref struct ComPtr<TPtr> where TPtr : unmanaged
    {
        /// <summary>
        /// Get a reference to the wrapped pointer.
        /// </summary>
        public unsafe readonly ref TPtr Ref => ref *_pointer;

        /// <summary>
        /// Delegate to release the wrapped pointer.
        /// 
        /// Prefer static delegates for best performance.
        /// </summary>
        /// <typeparam name="TComRelease">The type of pointer to release.</typeparam>
        /// <param name="source">The pointer to release.</param>
        public unsafe delegate void ComPtrRelease<TComRelease>(TComRelease* source)
            where TComRelease : unmanaged;

        private ComPtrRelease<TPtr>? Release { get; }
        private bool _forgotten = false;
        private unsafe TPtr* _pointer;

        /// <summary>
        /// Create a non-owning ComPtr wrapper.
        /// 
        /// This wrapper will not release the pointer when disposed.
        /// </summary>
        /// <param name="nonOwningPointer">The non-owning pointer.</param>
        public unsafe ComPtr(TPtr* nonOwningPointer)
        {
            this._pointer = nonOwningPointer;
            this.Release = null;
        }

        /// <summary>
        /// Create an owning ComPtr wrapper.
        /// 
        /// Ownership of the passed pointer is moved into the ComPtr struct, and will be released on dispose.
        /// </summary>
        /// <param name="pointer">The pointer to move into the wrapper.</param>
        /// <param name="release">The delegate to call on dispose to release the pointer.</param>
        public unsafe ComPtr(TPtr* pointer, ComPtrRelease<TPtr>? release)
        {
            this._pointer = pointer;
            this.Release = release;
        }

        /// <summary>
        /// Casting delegate to query a new interface of this COM pointer.
        /// 
        /// 'Casting' here is generalized to mean any call to create a new COM handle.
        /// </summary>
        /// <typeparam name="TSourcePtr">The type of the source pointer to cast.</typeparam>
        /// <param name="source">The source pointer to cast.</param>
        /// <param name="riid">The resource GUID of the target interface.</param>
        /// <param name="outPtr">The pointer that will contain the new handle.</param>
        public unsafe delegate void ComPtrCast<TSourcePtr>(TSourcePtr* source, Guid* riid, void** outPtr)
            where TSourcePtr: unmanaged;

        /// <summary>
        /// Casting delegate to query a new interface of this COM pointer.
        /// 
        /// 'Casting' here is generalized to mean any call to create a new COM handle.
        /// </summary>
        /// <typeparam name="TSourcePtr">The type of the source pointer to cast.</typeparam>
        /// <param name="source">The source pointer to cast.</param>
        /// <param name="riid">The resource GUID of the target interface.</param>
        /// <param name="outPtr">The pointer that will contain the new handle.</param>
        /// <returns>The result of the casting operation.</returns>
        public unsafe delegate int ComPtrCastInt<TSourcePtr>(TSourcePtr* source, Guid* riid, void** outPtr)
            where TSourcePtr : unmanaged;


        /// <summary>
        /// Casting delegate to query a new interface of this COM pointer.
        /// 
        /// 'Casting' here is generalized to mean any call to create a new COM handle.
        /// </summary>
        /// <typeparam name="TSourcePtr">The type of the source pointer to cast.</typeparam>
        /// <param name="source">The source pointer to cast.</param>
        /// <param name="outPtr">The pointer that will contain the new handle.</param>
        public unsafe delegate void ComPtrCastT<TSourcePtr, TOutPtr>(TSourcePtr* source, TOutPtr** outPtr)
            where TSourcePtr : unmanaged 
            where TOutPtr : unmanaged;

        /// <summary>
        /// Casts this COM pointer to another interface, creating a new handle.
        /// </summary>
        /// <typeparam name="TOut">The type of the new interface.</typeparam>
        /// <param name="cast">The delegate to cast this pointer with.</param>
        /// <param name="riid">The resource GUID of the target interface.</param>
        /// <param name="release">The delegate that releases the new handle.</param>
        /// <returns>The new casted handle as a wrapped ComPtr</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public unsafe ComPtr<TOut> Cast<TOut>(ComPtrCast<TPtr> cast, Guid riid, ComPtr<TOut>.ComPtrRelease<TOut>? release)
            where TOut: unmanaged
        {
            TOut* outPointer = null;
            cast(this._pointer, &riid, (void**)&outPointer);
            return new(outPointer, release);
        }

        /// <summary>
        /// Casts this COM pointer to another interface, creating a new handle.
        /// </summary>
        /// <typeparam name="TOut">The type of the new interface.</typeparam>
        /// <param name="cast">The delegate to cast this pointer with.</param>
        /// <param name="riid">The resource GUID of the target interface.</param>
        /// <param name="release">The delegate that releases the new handle.</param>
        /// <param name="result">The result of the casting operation.</param>
        /// <returns>The new casted handle as a wrapped ComPtr</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public unsafe ComPtr<TOut> Cast<TOut>(ComPtrCastInt<TPtr> cast, Guid riid, ComPtr<TOut>.ComPtrRelease<TOut>? release, out int result)
            where TOut : unmanaged
        {
            TOut* outPointer = null;
            result = cast(this._pointer, &riid, (void**)&outPointer);
            return new(outPointer, release);
        }

        /// <summary>
        /// Casts this COM pointer to another interface, creating a new handle.
        /// </summary>
        /// <typeparam name="TOut">The type of the new interface.</typeparam>
        /// <param name="cast">The delegate to cast this pointer with.</param>
        /// <param name="release">The delegate that releases the new handle.</param>
        /// <returns>The new casted handle as a wrapped ComPtr</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public unsafe ComPtr<TOut> Cast<TOut>(ComPtrCastT<TPtr, TOut> cast, ComPtr<TOut>.ComPtrRelease<TOut>? release)
         where TOut : unmanaged
        {
            TOut* outPointer = null;
            cast(this._pointer, &outPointer);
            return new(outPointer, release);
        }

        /// <summary>
        /// Moves ownership of the pointer out of the ComPtr, and invalidates the ComPtr wrapper.
        /// Disposing the ComPtr after calling Forget will do nothing. The caller is therefore responsible for releasing the handle afterwards.
        /// </summary>
        /// <returns>The wrapped pointer.</returns>
        public unsafe TPtr* Forget()
        {
            this._forgotten = true;
            TPtr* @this = this._pointer;
            this._pointer = null;
            return @this;
        }

        /// <summary>
        /// Explicit cast to the containing pointer. This method is unsafe.
        /// </summary>
        /// <param name="ptr">The ComPtr handle.</param>
        /// <returns>The pointer contained by the ComPtr wrapper.</returns>
        public static unsafe TPtr* operator ~(ComPtr<TPtr> ptr) => ptr._pointer;

        /// <summary>
        /// Implicit cast to the containing pointer. This method is unsafe.
        /// </summary>
        /// <param name="ptr">The ComPtr handle.</param>
        public static unsafe implicit operator TPtr* (ComPtr<TPtr> ptr) => ptr._pointer;

        /// <summary>
        /// Implicit cast to a pointer of the containing pointer. This method is unsafe.
        /// </summary>
        /// <param name="ptr">The ComPtr handle.</param>
        public static unsafe implicit operator TPtr**(ComPtr<TPtr> ptr) => AddressOf(ptr._pointer);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
