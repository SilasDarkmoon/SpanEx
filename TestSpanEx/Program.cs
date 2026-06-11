using System;

namespace Mod.LowLevel.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Span<byte> span = stackalloc byte[10];
            for (int i = 0; i < 10; i++)
            {
                span[i] = (byte)(10 * i);
            }

            if (SpanEx.StubType == typeof(SpanStubModern))
            {
                ref SpanStubModern layout = ref SpanEx.ConvertToNormal<SpanStubModern, byte>(in span);
                Test(layout);

            }
            else if (SpanEx.StubType == typeof(SpanStubOld))
            {
                ref SpanStubOld layout = ref SpanEx.ConvertToNormal<SpanStubOld, byte>(in span);
                Test(layout);
            }
        }

        static void Test(SpanStubOld old)
        {
            Span<byte> span = SpanEx.ConvertToSpan<SpanStubOld, byte>(in old);
            foreach (byte b in span)
            {
                Console.WriteLine(b);
            }
        }
        static void Test(SpanStubModern old)
        {
            Span<byte> span = SpanEx.ConvertToSpan<SpanStubModern, byte>(in old);
            foreach (byte b in span)
            {
                Console.WriteLine(b);
            }
        }
    }
}