using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAppTesting.Records
{
    public record Move((int x, int y) From, (int x, int y) To)
    {
        public MoveDelta ToDelta()
            => new(To.x - From.x, To.y - From.y);
    }
}
