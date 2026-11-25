using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAppTesting.Enums
{
    public enum GameError
    {
        None,
        // Move Errors
        MoveOutOfBounds,
        MoveBlockedByPiece,
        MovePathBlocked,
        MoveConditionsNotMet,
        MoveLeavesKingInCheck,
        InvalidMoveForThisPiece,
        CannotTakeOnEmpty,
        CannotTakeOwnPiece,
        PieceNotAtMoveFromLocation,

        // Board Errors
        BoardPositionOutOfBounds,
        BoardCannotFindPieceID,
        BoardCannotPlacePieceOnOccupiedSquare,
        BoardCannotAddSamePieceTwice,

        // User Errors
        PlayerCannotMoveOthersPieces,
        PlayerGaveUnkownCommand,

        LoadingError_FailedLoadFromCSV,
    }

    public static class GameErrorExtensions
    {
        public static string ToString(GameError error)
            => error switch
            {
                GameError.None => $"{error}: Move is legal.",
                GameError.MoveOutOfBounds => $"{error}: Tried to move off the board.",
                GameError.MoveBlockedByPiece => $"{error}: Tried to move into occupied space.",
                GameError.MovePathBlocked => $"{error}: Tried to move through an occupied space.",
                GameError.MoveConditionsNotMet => $"{error}: This move is not valid under the current board conditions",
                GameError.MoveLeavesKingInCheck => $"{error}: This move would leave the king in check!",
                GameError.InvalidMoveForThisPiece => $"{error}: This move is invalid for this piece.",
                GameError.CannotTakeOnEmpty => $"{error}: Attempted to take on an empty square.",
                GameError.CannotTakeOwnPiece => $"{error}: Attempted to take a friendl piece!",

                GameError.BoardPositionOutOfBounds => $"{error}: Attempted to get position not on the board!",
                GameError.BoardCannotFindPieceID => $"{error}: An unknown piece ID was passed to the board!",
                GameError.BoardCannotPlacePieceOnOccupiedSquare => $"{error}: Attempted to place piece on an occupied square!",
                GameError.BoardCannotAddSamePieceTwice => $"{error}: Attepted to add the same piece (by id) to the board multiple times!",

                GameError.PlayerCannotMoveOthersPieces => $"{error}: Player attempted to move someone else's pieces!",
                GameError.PlayerGaveUnkownCommand => $"{error}: Command given by player not recognized!",
                _ => $"No getMessage defined for {error}"
            };
    }
    

}
