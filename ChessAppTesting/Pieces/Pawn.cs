using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using ChessAppTesting.Enums;
using System.ComponentModel;

namespace ChessAppTesting.Pieces
{
    [method: SetsRequiredMembers]
    public class Pawn(
        ChessBoard board,
        TeamColor teamColor,
        (int x, int y) position)
        : Piece(board, teamColor, PieceType.Pawn, position)
    {

        // == Methods ===============================
        internal override bool IsLegalMove((int dx, int dy) move, (int x, int y) position, ChessBoard board, out string? error)
        {
            error = string.Empty;
            (int x, int y) new_position;

            new_position = (Position.x + move.dx, Position.y + move.dy);

            int dir = this.Color == TeamColor.White ? 1 : -1;
            move.dy *= dir;

            // Check if the move is legal based on the type of move
            bool legal = move switch
            {
                // Move forward one square
                (0, 1) =>
                    !board.TryGetPieceIDAtPosition(
                        position: new_position,
                        out _,
                        out error),
                // Move forward two squares from starting position
                (0, 2) =>
                    IsOnStartRanks(out error) &&
                    !board.TryGetPieceIDAtPosition(
                        position: (Position.x, Position.y + (1*dir)),
                        out _,
                        out error) &&
                    !board.TryGetPieceIDAtPosition(
                        position: new_position,
                        out _,
                        out error),
                // Diagonal capture to the right
                (1, 1) =>
                    board.TryGetPieceAtPosition(
                        position: new_position,
                        out Piece? other_piece,
                        out error) &&
                    other_piece.Color != this.Color,
                // Diagonal capture to the left
                (-1, 1) =>
                    board.TryGetPieceAtPosition(
                        position: new_position,
                        out Piece? other_piece,
                        out error) &&
                    other_piece.Color != this.Color,
                _ => false
            };

            error = $"{this}: " + error;
            return legal;
        }

        private bool IsOnStartRanks([NotNullWhen(true)] out string? error)
        {
            bool onStartRank = Color switch
            {
                TeamColor.White =>
                    Position.y == 1 ||
                    Position.y == 0,
                TeamColor.Black =>
                    Position.y == (ParentBoard.max_y - 1) ||
                    Position.y == (ParentBoard.max_y - 2),
                _ => false
            };

            error = onStartRank ? null : $"Pawn {ID} is not ({Position.x}, {Position.y}) on one of its 2 starting ranks";
            return onStartRank;
        }

        public override List<(int, int)> GetLegalMoves(
            (int x, int y) position,
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
