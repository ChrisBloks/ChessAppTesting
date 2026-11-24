using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAppTesting.Records
{
    public record MoveDelta(int Dx, int Dy)
    {
        public static MoveDelta operator +(MoveDelta A, MoveDelta B)
            => new(A.Dx + B.Dx, A.Dy + B.Dy);
    }
}
