using ChessAppTesting.Enums;
using ChessAppTesting.Records;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ChessAppTesting.Pieces
{
    [method: SetsRequiredMembers]
    public class Bishop(
        ChessBoard board,
        TeamColor teamColor,
        Position position)
        : Piece(board, teamColor, PieceType.Bishop, position)
    {
        internal override bool IsLegalMove(
            Move move,
            ChessBoard board,
            out GameError error)
        {
            if (move.From != Position)
            {
                error = GameError.PieceNotAtMoveFromLocation;
                return false;
            }
            return IsDiagonalPathClear(move.ToDelta(), Position, ParentBoard, out error);
        }

        public override List<MoveDelta> GetLegalMoves(
            Position position,
            ChessBoard board)
        {
            throw new NotImplementedException();
        }

        private bool IsDiagonalPathClear(
            MoveDelta moveDelta,
            Position startPosition,
            ChessBoard board,
            out GameError error)
        {
            if (!moveDelta.IsDiagonal())
            {
                error = GameError.InvalidMoveForThisPiece;
                return false;
            }

            MoveDelta direction = moveDelta.Direction();
            int distance = moveDelta.Xdistance();

            Position tempPosition;
            for (int i = 1; i < distance; i++)
            {
                tempPosition = startPosition + (direction * i);
                if (board.IsPositionOccupied(tempPosition))
                {
                    error = GameError.MoveBlockedByPiece;
                    return false;
                }
            }

            tempPosition = startPosition + moveDelta;
            if (board.TryGetPieceAtPosition(
                    tempPosition,
                    out Piece? mystery_piece,
                    out _) &&
                mystery_piece.Color == this.Color) 
            {
                error = GameError.CannotTakeOwnPiece;
                return false;
            }

            error = GameError.None;
            return true;
        }

    }
}
