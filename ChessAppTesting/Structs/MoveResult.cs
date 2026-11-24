using ChessAppTesting.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using ChessAppTesting.Records;

namespace ChessAppTesting.Structs
{
    public readonly record struct MoveResult(MoveError Error, Move Move)
    {
        public bool IsLegal => Error == MoveError.None;

        // Built only when someone asks for it
        public string Message => Error switch
        {
            MoveError.None => "Move is legal.",
            MoveError.OutOfBounds => $"{Move.To} is off the board.",
            MoveError.BlockedByPiece => $"{Move.To} is occupied by a piece.",
            MoveError.CannotTakeOwnPiece => $"Cannot take on {Move.To} because it is occupied by a friendly piece.",
            MoveError.PathBlocked => $"The path to {Move.To} is blocked.",
            MoveError.CannotTakeOnEmpty => $"Tried to take on an empty square {Move.To}",
            MoveError.InvallidMoveForThisPiece => $"{GetType().Name} can't move that way.", // Check how this works
            MoveError.LeavesKingInCheck => $"Move would leave own king in check",
            _ => Error.ToString(),
        };

        public static implicit operator bool(MoveResult r) => r.IsLegal; 
    }

}
