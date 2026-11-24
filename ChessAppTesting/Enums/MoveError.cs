using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAppTesting.Enums
{
    public enum MoveError
    {
        None,
        OutOfBounds,
        BlockedByPiece,
        CannotTakeOwnPiece,
        PathBlocked,
        InvallidMoveForThisPiece,
        CannotTakeOnEmpty,
        PieceNotAtFromLocation,
        LeavesKingInCheck,
    }
}
