using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ChessAppTesting.Records
{
    public record Move(Position From, Position To)
    {
        public Move(Position from, MoveDelta delta)
            : this(from, new Position(from.X + delta.Dx, from.Y + delta.Dy))
        { }

        public MoveDelta ToDelta()
            => new(To.X - From.X, To.Y - From.Y);

        public static implicit operator Move((Position from, Position to) A)
            => new(A.from, A.to);
    }
}
