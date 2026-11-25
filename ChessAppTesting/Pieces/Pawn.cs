using ChessAppTesting.Enums;
using ChessAppTesting.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ChessAppTesting.Pieces
{
    [method: SetsRequiredMembers]
    public class Pawn(
        ChessBoard board,
        TeamColor teamColor,
        Position position)
        : Piece(board, teamColor, PieceType.Pawn, position)
    {

        // == Methods ===============================
        internal override bool IsLegalMove(
            Move move,
            ChessBoard board,
            out GameError error)
        {
            if (Position != move.From)
            {
                error = GameError.PieceNotAtMoveFromLocation;
                return false;
            }

            MoveDelta moveDelta = move.ToDelta();
            // Check if the move is legal based on the type of move
            bool legal = moveDelta switch
            {
                // Move forward one square
                (0, 1 or -1) =>
                    CheckAdvanceOne(this, move, moveDelta, board, out error),
                // Move forward two squares from starting position
                (0, 2 or -2) =>
                    CheckAdvanceTwo(this, move, moveDelta, board, out error),
                // Diagonal capture to the right
                (1 or -1, 1 or -1) =>
                    CheckRegularTake(this, move, moveDelta, board, out error),
                // Diagonal capture to the left
                _ => HandleDefault(out error)
            };
            return legal;
        }

        private static bool CheckAdvanceOne(
            Pawn pawn,
            Move move,
            MoveDelta moveDelta,
            ChessBoard board,
            out GameError error)
        {
            if (!MoveInCorrectDirection(pawn, moveDelta))
            {
                error = GameError.InvalidMoveForThisPiece;
                return false;
            }

            if (board.IsPositionOccupied(position: move.To))
            {
                error = GameError.MoveBlockedByPiece;
                return false;
            }
            error = GameError.None;
            return true;
        }

        private static bool CheckAdvanceTwo(
            Pawn pawn,
            Move move,
            MoveDelta moveDelta,
            ChessBoard board,
            out GameError error)
        {
            if (!MoveInCorrectDirection(pawn, moveDelta))
            {
                error = GameError.InvalidMoveForThisPiece;
                return false;
            }

            if (!IsOnStartRanks(pawn, move.From, board))
            {
                error = GameError.MoveConditionsNotMet;
                return false;
            }

            if (board.IsPositionOccupied(move.From + moveDelta.Direction())
                || board.IsPositionOccupied(position: move.To))
            {
                error = GameError.MoveBlockedByPiece;
                return false;
            }

            error = GameError.None;
            return true;
        }

        private static bool CheckRegularTake(
            Pawn pawn,
            Move move,
            MoveDelta moveDelta,
            ChessBoard board,
            out GameError error)
        {
            if (!MoveInCorrectDirection(pawn, moveDelta))
            {
                error = GameError.InvalidMoveForThisPiece;
                return false;
            }

            if (!board.TryGetPieceAtPosition(move.To, out Piece? mysteryPiece, out _))
            {
                error = GameError.MoveConditionsNotMet;
                return false;
            }

            if(pawn.Color == mysteryPiece.Color)
            {
                error = GameError.CannotTakeOwnPiece;
                return false;
            }

            error = GameError.None;
            return true;
        }

        private bool IsOnStartRanks()
            => IsOnStartRanks(this, Position, ParentBoard);

        private static bool IsOnStartRanks(Pawn pawn, Position position, ChessBoard board)
        {
            return pawn.Color switch
            {
                TeamColor.White =>
                    position.Y == 1 ||
                    position.Y == 0,
                TeamColor.Black =>
                    position.Y == (board.max_y - 1) ||
                    position.Y == (board.max_y - 2),
                _ => false
            };
        }

        private static bool MoveInCorrectDirection(Pawn pawn, MoveDelta moveDelta)
        {
            int moveDir = pawn.Color == TeamColor.White ? 1 : -1;
            return moveDir == Math.Sign(moveDelta.Dy);
        }

        private static bool HandleDefault(out GameError error)
        {
            error = GameError.InvalidMoveForThisPiece;
            return false;
        }

        public override List<MoveDelta> GetLegalMoves(
            Position position,
            ChessBoard board)
        {
            List<(int, int)> candidateMoves =
                Color switch
                {
                    TeamColor.White => [(0, 1), (0, 2), (-1, 1), (1, 1)],
                    TeamColor.Black => [(0, -1), (0, -2), (-1, -1), (1, -1)],
                    _ => throw new InvalidEnumArgumentException($"Color {Color} is not recognized in {ID}'s GetLegalMoves method")
                };

            return [.. candidateMoves.Where(move => IsLegalMove(move, out _))];
        }
    }
}
