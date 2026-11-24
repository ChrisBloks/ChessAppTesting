using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAppTesting.Records
{
    public record MoveDelta(int Dx, int Dy)
    {
        public static MoveDelta operator +(MoveDelta A, MoveDelta B)
            => new(A.Dx + B.Dx, A.Dy + B.Dy);

        public static MoveDelta operator *(MoveDelta A, int B)
            => new(A.Dx * B, A.Dy * B);

        public static implicit operator MoveDelta((int x, int y) A)
            => new(A.x, A.y);

        public bool IsDiagonal()
            => Math.Abs(Dx) == Math.Abs(Dy);

        public MoveDelta Direction()
            => new(Math.Sign(Dx), Math.Sign(Dy));

        public int Xdistance()
            => Math.Abs(Dx);

        public int Ydistance()
            => Math.Abs(Dy);
    }
}
