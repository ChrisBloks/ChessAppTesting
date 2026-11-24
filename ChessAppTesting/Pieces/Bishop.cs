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
            out MoveError error)
        {
            if (move.From != Position)
            {
                error = MoveError.PieceNotAtFromLocation;
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
            out MoveError error)
        {
            if (!moveDelta.IsDiagonal())
            {
                error = MoveError.InvallidMoveForThisPiece;
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
                    error = MoveError.BlockedByPiece;
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
                error = MoveError.CannotTakeOwnPiece;
                return false;
            }

            error = MoveError.None;
            return true;
        }

    }
}
