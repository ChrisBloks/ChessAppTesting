using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAppTesting.Enums
{
    public enum BoardError
    {
        None,
        OutOfBounds,
        UnknownPieceID,
        CannotPlacePieceOnOccupiedSquare,
        CannotAddSamePieceTwice,
    }
}
