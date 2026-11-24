using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAppTesting.Enums
{
    public enum MoveError
    {
        None,
        OutOfBounds,
        BlockedByOwnPiece,
        BlockedByOtherPiece,
        PathBlocked,
        InvallidMoveForThisPiece,
        LeavesKingInCheck,
    }
}
