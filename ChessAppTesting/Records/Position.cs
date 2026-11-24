using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAppTesting.Records
{
    public record Position(int X, int Y)
    {
        public static Position operator +(Position pos, MoveDelta d)
            => new(pos.X + d.Dx, pos.Y + d.Dy);
    }
}
