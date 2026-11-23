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
    public abstract class Piece
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

        //private List<(int dx, int dy)> LegalMoves { get; set; } = new();

        // == Constructors ==========================
        [SetsRequiredMembers]
        public Piece(
            ChessBoard parentBoard,
            TeamColor color,
            PieceType pieceType,
            (int x, int y) position)
        {
            ID = new PieceID(color, pieceType, PieceCount);
            ParentBoard = parentBoard;
            Color = color;
            Type = pieceType;
            Position = position;
            if (!parentBoard.TryAddPieceAtPosition(this, position, out string error))
            {
                throw new InvalidOperationException(error);
            }
            PieceCount++;
        }

        // == Methods ===============================

        internal bool IsLegalMove(
            (int x, int y) from,
            (int x, int y) to,
            out string? error) => 
            IsLegalMove(
                (to.x - from.x, to.y - from.y),
                out error);

        internal bool IsLegalMove(
            (int dx, int dy) move,
            out string? error) =>
            IsLegalMove(
                move,
                Position,
                ParentBoard,
                out error);

        internal abstract bool IsLegalMove(
            (int dx, int dy) move,
            (int x, int y) position,
            ChessBoard board,
            out string? error);
        
        public List<(int, int)> GetLegalMoves() =>
            GetLegalMoves(
                Position,
                ParentBoard);

        public List<(int, int)> GetLegalMoves(ChessBoard board) =>
            GetLegalMoves(
                Position,
                board);

        public abstract List<(int, int)> GetLegalMoves(
            (int x, int y) position,
            ChessBoard board);

        public bool TryRegularMove(
            (int x, int y) from,
            (int x, int y) to,
            out string? error) =>
            TryRegularMove(
                (to.x - from.x, to.y - from.y),
                out error);

        public bool TryRegularMove(
            (int dx, int dy) move,
            out string? error)
        {
            if (!IsLegalMove(move, out error))
            {
                return false;
            }
            return ParentBoard.TryHandleMovePiece(
                moving_piece: this,
                old_position: Position,
                new_position: (Position.x + move.dx, Position.y + move.dy),
                error: out error);
        }

        public bool TryRegularTake(
            (int x, int y) from,
            (int x, int y) to,
            out string? error) => 
            TryRegularTake(
                (to.x - from.x, to.y - from.y),
                out error);

        public bool TryRegularTake(
            (int dx, int dy) move,
            out string? error)
        {
            if (!IsLegalMove(move, out error))
            {
                return false;
            }
            return ParentBoard.TryHandleTakePiece(
                taking_piece: this,
                old_position: Position,
                new_position: (Position.x + move.dx, Position.y + move.dy),
                error: out error);
        }

    }
}
