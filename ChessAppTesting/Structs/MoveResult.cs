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
            MoveError.BlockedByOwnPiece => $"{Move.To} is occupied by your own piece.",
            MoveError.BlockedByOtherPiece => $"{Move.To} is occupied by your enemy piece.",
            MoveError.PathBlocked => $"The path to {Move.To} is blocked.",
            MoveError.InvallidMoveForThisPiece => $"{GetType().Name} can't move that way.",
            MoveError.LeavesKingInCheck => $"Move would leave own king in check",
            _ => Error.ToString(),
        };

        public static implicit operator bool(MoveResult r) => r.IsLegal; // optional
    }

}
