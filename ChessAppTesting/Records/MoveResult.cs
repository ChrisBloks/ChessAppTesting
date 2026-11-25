using ChessAppTesting.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAppTesting.Records
{
    public record MoveResult(
        GameError Error,
        Move Move) : ActionResult(Error)
    {
        public MoveResult(GameError error, PlayerAction.MoveAction moveAction)
            : this(error, moveAction.Move)
        { }

        // Built only when someone asks for it
        new public string Message => Error switch
        {
            GameError.MoveOutOfBounds => $"{Move.To} is off the board.",
            GameError.MoveBlockedByPiece => $"{Move.To} is occupied by a piece.",
            GameError.MovePathBlocked => $"Path from {Move.From} to {Move.To} is blocked by a piece!",
            _ => base.Message,
        };

    }

}
