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

            if (SpanEx.StubType == typeof(SpanEx.SpanStubModern))
            {
                ref SpanEx.SpanStubModern layout = ref SpanEx.ConvertToNormal<SpanEx.SpanStubModern, byte>(in span);
                Test(layout);

            }
            else if (SpanEx.StubType == typeof(SpanEx.SpanStubOld))
            {
                ref SpanEx.SpanStubOld layout = ref SpanEx.ConvertToNormal<SpanEx.SpanStubOld, byte>(in span);
                Test(layout);
            }
        }

        static void Test(SpanEx.SpanStubOld old)
        {
            Span<byte> span = SpanEx.ConvertToSpan<SpanEx.SpanStubOld, byte>(in old);
            foreach (byte b in span)
            {
                Console.WriteLine(b);
            }
        }
        static void Test(SpanEx.SpanStubModern old)
        {
            Span<byte> span = SpanEx.ConvertToSpan<SpanEx.SpanStubModern, byte>(in old);
            foreach (byte b in span)
            {
                Console.WriteLine(b);
            }
        }
    }
}