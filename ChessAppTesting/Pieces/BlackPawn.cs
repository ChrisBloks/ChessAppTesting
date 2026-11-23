using ChessAppTesting.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ChessAppTesting.Pieces
{
    public class BlackPawn : Piece
    {
        // == Constructors ==========================
        [SetsRequiredMembers]
        public BlackPawn(ChessBoard board, (int x, int y) position) : base(board, TeamColor.Black, PieceType.Pawn, position)
        {
            CandidateMoves.Add((0, -1));
            CandidateMoves.Add((0, -2));
            CandidateMoves.Add((1, -1));
            CandidateMoves.Add((-1, -1));
        }

        // == Methods ===============================
        internal override bool IsLegalMove((int dx, int dy) move, (int x, int y) position, ChessBoard board)
        {
            (int x, int y) new_position;
            new_position = (Position.x + move.dx, Position.y + move.dy);
            // If the new position is not on the board, return false
            if (!board.PositionInBoard(new_position))
            {
                return false;
            }

            // Check if the move is legal based on the type of move
            bool legal = move switch
            {
                // Move forward one square
                (0, -1) => !board.Board[new_position.x, new_position.y].ContainsPiece(),
                // Move forward two squares from starting position
                (0, -2) => Position.y == 6 &&
                        !board.Board[Position.x, Position.y - 1].ContainsPiece() &&
                        !board.Board[new_position.x, new_position.y].ContainsPiece(),
                // Diagonal capture to the right
                (1, -1) => board.Board[new_position.x, new_position.y].ContainsPiece(),
                // Diagonal capture to the left
                (-1, -1) => board.Board[new_position.x, new_position.y].ContainsPiece(),
                _ => false
            };

            return legal;
        }
    }
}

