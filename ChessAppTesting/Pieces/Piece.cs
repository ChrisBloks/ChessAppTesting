using ChessAppTesting.Enums;
using ChessAppTesting.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;

namespace ChessAppTesting.Pieces
{


    // General class for all chess pieces
    internal abstract class Piece
    {
        // == Static Properties ============================
        public static int PieceCount { get; private set; } = 0;

        // == Enums ============================
        

        // == Properties ============================
        public required ChessBoard ParentBoard { get; set; }
        public required PieceID ID { get; set; }
        public required TeamColor Color { get; set; }
        public required PieceType Type { get; set; } = PieceType.None;
        public required (int x, int y) Position { get; set; }

        public required List<(int, int)> CandidateMoves { get; set; } = new();

        // == Constructors ==========================
        [SetsRequiredMembers]
        public Piece(ChessBoard parentBoard, TeamColor color, PieceType pieceType, (int x, int y) position)
        { 
            ParentBoard = parentBoard;
            ID = new PieceID(color, pieceType, PieceCount);
            Color = color;
            Type = pieceType;
            Position = position;
            PieceCount++;
        }

        // == Methods ===============================

        internal bool IsLegalMove((int x, int y) from, (int x, int y) to) => IsLegalMove((to.x - from.x, to.y - from.y));
        internal bool IsLegalMove((int dx, int dy) move) => IsLegalMove(move, Position, ParentBoard);
        internal abstract bool IsLegalMove((int dx, int dy) move, (int x, int y) position, ChessBoard board);
        
        public List<(int, int)> getLegalMoves() => getLegalMoves(Position, ParentBoard);
        public List<(int, int)> getLegalMoves(ChessBoard board) => getLegalMoves(Position, board);
        public List<(int, int)> getLegalMoves((int x, int y) position, ChessBoard board)
        {
            List<(int, int)> legalMoves = new List<(int, int)>();
            foreach (var move in CandidateMoves)
            {
                if (IsLegalMove(move, position, board))
                {
                    legalMoves.Add(move);
                }
            }
            return legalMoves;
        }

        public bool TryRegularMove((int x, int y) from, (int x, int y) to, out string error) => TryRegularMove((to.x - from.x, to.y - from.y), out error);
        public bool TryRegularMove((int dx, int dy) move, out string error)
        {
            if (!IsLegalMove(move))
            {
                error = "Illegal move";
                return false;
            }

            (int x, int y) new_position = (Position.x + move.dx, Position.y + move.dy);

            if (!ParentBoard.PositionInBoard(new_position))
            {
                error = "Move out of bounds";
                return false;
            }

            if (ParentBoard.TryGetPieceAtPosition(new_position, out _, out error))
            {
                error = "Piece blocking this location!";
                return false; 
            }

            return ParentBoard.TryHandleMovePiece(this, Position, new_position, out error);
        }

        public bool TryRegularTake((int x, int y) from, (int x, int y) to, out string error) => TryRegularTake((to.x - from.x, to.y - from.y), out error);
        public bool TryRegularTake((int dx, int dy) move, out string error)
        {
            if (!IsLegalMove(move))
            {
                error = "Illegal move";
                return false;
            }

            (int x, int y) new_position = (Position.x + move.dx, Position.y + move.dy);
            
            if (!ParentBoard.PositionInBoard(new_position))
            {
                error = "Move out of bounds";
                return false;
            }

            if (!ParentBoard.TryGetPieceAtPosition(new_position, out var targetPiece, out error))
            {
                return false;
            }

            if (targetPiece.Color == Color)
            {
                error = "Cannot take own piece";
                return false;
            }

            // Take the piece
            ParentBoard.TryHandleTakePiece(this, Position, new_position, out error);
            return true;
        }

    }
}
