using System;
using System.Runtime.InteropServices;

namespace Mod.LowLevel
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SpanStubModern<T>
    {
        public SpanEx.SpanStubModern Inner;

        public static implicit operator SpanEx.SpanStubModern(SpanStubModern<T> stub)
        {
            return stub;
        }
        public static implicit operator SpanStubModern<T>(SpanEx.SpanStubModern stub)
        {
            return new SpanStubModern<T>() { Inner = stub };
        }
        public static implicit operator SpanStubModern<T>(Span<T> span)
        {
            return SpanEx.ConvertToStubModern(in span);
        }
        public static implicit operator SpanStubModern<T>(ReadOnlySpan<T> span)
        {
            return SpanEx.ConvertToStubModern(in span);
        }
        public static implicit operator Span<T>(SpanStubModern<T> stub)
        {
            return SpanEx.ConvertToSpan(in stub);
        }
        public static implicit operator ReadOnlySpan<T>(SpanStubModern<T> stub)
        {
            return SpanEx.ConvertToReadOnlySpan(in stub);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct SpanStubOld<T>
    {
        public SpanEx.SpanStubOld Inner;

        public static implicit operator SpanEx.SpanStubOld(SpanStubOld<T> stub)
        {
            return stub;
        }
        public static implicit operator SpanStubOld<T>(SpanEx.SpanStubOld stub)
        {
            return new SpanStubOld<T>() { Inner = stub };
        }
        public static implicit operator SpanStubOld<T>(Span<T> span)
        {
            return SpanEx.ConvertToStubOld(in span);
        }
        public static implicit operator SpanStubOld<T>(ReadOnlySpan<T> span)
        {
            return SpanEx.ConvertToStubOld(in span);
        }
        public static implicit operator Span<T>(SpanStubOld<T> stub)
        {
            return SpanEx.ConvertToSpan(in stub);
        }
        public static implicit operator ReadOnlySpan<T>(SpanStubOld<T> stub)
        {
            return SpanEx.ConvertToReadOnlySpan(in stub);
        }
    }

    public static class SpanEx
    {
        private static readonly Type _StubType;
        public static Type StubType => _StubType;
        private static readonly int _SpanSize;
        public static int SpanSize => _SpanSize;
        static SpanEx()
        {
            unsafe
            {
                _SpanSize = sizeof(Span<byte>);
                if (_SpanSize == sizeof(SpanStubModern))
                {
                    _StubType = typeof(SpanStubModern);

                }
                else if (_SpanSize == sizeof(SpanStubOld))
                {
                    _StubType = typeof(SpanStubOld);
                }
            }
        }

        private static ref TNormal ConvertToNormalImp<TNormal, TElement>(in Span<TElement> rspan)
        {
            throw new NotImplementedException();
        }
        private static ref Span<TElement> ConvertToSpanImp<TNormal, TElement>(in TNormal rnormal)
        {
            throw new NotImplementedException();
        }
        private static ref TNormal ConvertToNormalImp<TNormal, TElement>(in ReadOnlySpan<TElement> rspan)
        {
            throw new NotImplementedException();
        }
        private static ref ReadOnlySpan<TElement> ConvertToReadOnlySpanImp<TNormal, TElement>(in TNormal rnormal)
        {
            throw new NotImplementedException();
        }

        public static ref TNormal ConvertToNormal<TNormal, TElement>(in Span<TElement> rspan) where TNormal : struct
        {
            unsafe
            {
                if (sizeof(TNormal) != sizeof(Span<TElement>))
                {
                    throw new ArgumentException("Struct size mismatch!", nameof(TNormal));
                }
            }
            return ref ConvertToNormalImp<TNormal, TElement>(in rspan);
        }
        public static ref Span<TElement> ConvertToSpan<TNormal, TElement>(in TNormal rnormal) where TNormal : struct
        {
            unsafe
            {
                if (sizeof(TNormal) != sizeof(Span<TElement>))
                {
                    throw new ArgumentException("Struct size mismatch!", nameof(TNormal));
                }
            }
            return ref ConvertToSpanImp<TNormal, TElement>(in rnormal);
        }
        public static ref TNormal ConvertToNormal<TNormal, TElement>(in ReadOnlySpan<TElement> rspan) where TNormal : struct
        {
            unsafe
            {
                if (sizeof(TNormal) != sizeof(Span<TElement>))
                {
                    throw new ArgumentException("Struct size mismatch!", nameof(TNormal));
                }
            }
            return ref ConvertToNormalImp<TNormal, TElement>(in rspan);
        }
        public static ref ReadOnlySpan<TElement> ConvertToReadOnlySpan<TNormal, TElement>(in TNormal rnormal) where TNormal : struct
        {
            unsafe
            {
                if (sizeof(TNormal) != sizeof(Span<TElement>))
                {
                    throw new ArgumentException("Struct size mismatch!", nameof(TNormal));
                }
            }
            return ref ConvertToReadOnlySpanImp<TNormal, TElement>(in rnormal);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpanStubModern
        {
            public object Ref;
            public int Length;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SpanStubOld
        {
            public object Ref;
            public IntPtr Offset;
            public int Length;
        }

        public static ref SpanStubModern<T> ConvertToStubModern<T>(this in Span<T> span)
        {
            return ref SpanEx.ConvertToNormal<SpanStubModern<T>, T>(in span);
        }
        public static ref SpanStubModern<T> ConvertToStubModern<T>(this in ReadOnlySpan<T> span)
        {
            return ref SpanEx.ConvertToNormal<SpanStubModern<T>, T>(in span);
        }
        public static ref Span<T> ConvertToSpan<T>(this in SpanStubModern<T> stub)
        {
            return ref SpanEx.ConvertToSpan<SpanStubModern<T>, T>(in stub);
        }
        public static ref ReadOnlySpan<T> ConvertToReadOnlySpan<T>(this in SpanStubModern<T> stub)
        {
            return ref SpanEx.ConvertToReadOnlySpan<SpanStubModern<T>, T>(in stub);
        }
        public static ref SpanStubOld<T> ConvertToStubOld<T>(this in Span<T> span)
        {
            return ref SpanEx.ConvertToNormal<SpanStubOld<T>, T>(in span);
        }
        public static ref SpanStubOld<T> ConvertToStubOld<T>(this in ReadOnlySpan<T> span)
        {
            return ref SpanEx.ConvertToNormal<SpanStubOld<T>, T>(in span);
        }
        public static ref Span<T> ConvertToSpan<T>(this in SpanStubOld<T> stub)
        {
            return ref SpanEx.ConvertToSpan<SpanStubOld<T>, T>(in stub);
        }
        public static ref ReadOnlySpan<T> ConvertToReadOnlySpan<T>(this in SpanStubOld<T> stub)
        {
            return ref SpanEx.ConvertToReadOnlySpan<SpanStubOld<T>, T>(in stub);
        }
    }
}
