using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ChessAppTesting.Enums;

namespace ChessAppTesting.Pieces
{
    [method: SetsRequiredMembers]
    public class Bishop(
        ChessBoard board,
        TeamColor teamColor,
        (int x, int y) position)
        : Piece(board, teamColor, PieceType.Bishop, position)
    {
        internal override bool IsLegalMove(
            (int dx, int dy) move,
            (int x, int y) position,
            ChessBoard board,
            out string? error)
        {
            error = string.Empty;
            return IsDiagonalPathClear(move, Position, ParentBoard);
        }

        public override List<(int, int)> GetLegalMoves(
            (int x, int y) position,
            ChessBoard board)
        {
            throw new NotImplementedException();
        }

        private bool IsDiagonalPathClear(
            (int dx, int dy) move,
            (int x, int y) startPosition,
            ChessBoard board)
        {
            if (!IsMoveDiagonal(move))
            {
                return false;
            }

            (int dx, int dy) dir = (Math.Sign(move.dx), Math.Sign(move.dy));
            int distance = Math.Abs(move.dx);

            (int x, int y) temp_pos;
            for (int i = 1; i < distance; i++)
            {
                temp_pos = (startPosition.x + dir.dx * i, startPosition.y + dir.dy * i);
                if (board.IsPositionOccupied(temp_pos, out _))
                {
                    return false;
                }
            }

            (int x, int y) new_position = (startPosition.x + move.dx, startPosition.y + move.dy);
            if (board.TryGetPieceAtPosition(
                    new_position,
                    out Piece? mystery_piece,
                    out _) &&
                mystery_piece.Color == this.Color) 
            {
                return false;
            }

            return true;
        }

        private bool IsMoveDiagonal((int dx, int dy) move)
        {
            return Math.Abs(move.dx) == Math.Abs(move.dy);
        }

    }
}
