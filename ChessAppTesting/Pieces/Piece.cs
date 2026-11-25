using ChessAppTesting.Enums;
using ChessAppTesting.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using ChessAppTesting.Records;

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
        public required Position Position { get; set; }

        //private List<(int dx, int dy)> LegalMoves { get; set; } = new();

        // == Constructors ==========================
        [SetsRequiredMembers]
        public Piece(
            ChessBoard parentBoard,
            TeamColor color,
            PieceType pieceType,
            Position position)
        {
            ID = new PieceID(color, pieceType, PieceCount);
            ParentBoard = parentBoard;
            Color = color;
            Type = pieceType;
            Position = position;
            if (!parentBoard.TryAddPieceAtPosition(
                piece: this,
                position: position,
                out _))
            {
                throw new InvalidOperationException($"Failed to add piece {ID} to location {Position}");
            }
            PieceCount++;
        }

        // == Methods ===============================

        internal bool IsLegalMove(
            Position from,
            Position to,
            out GameError error) 
            => IsLegalMove(
                new Move(from, to),
                ParentBoard,
                out error);

        internal bool IsLegalMove(
            MoveDelta moveDelta,
            out GameError error) =>
            IsLegalMove(
                new Move(Position, moveDelta),
                ParentBoard,
                out error);
        internal bool IsLegalMove(
            Move move,
            out GameError error)
            => IsLegalMove(
                move: move,
                board: ParentBoard,
                error: out error);

        internal abstract bool IsLegalMove(
            Move move,
            ChessBoard board,
            out GameError error);
        
        public List<MoveDelta> GetLegalMoves() 
            => GetLegalMoves(
                Position,
                ParentBoard);

        public List<MoveDelta> GetLegalMoves(ChessBoard board) 
            => GetLegalMoves(
                Position,
                board);

        public abstract List<MoveDelta> GetLegalMoves(
            Position position,
            ChessBoard board);

        public bool TryRegularMove(
            MoveDelta moveDelta,
            out MoveResult result)
            => TryRegularMove(
                new Move(Position, moveDelta),
                out result);

        public bool TryRegularMove(
            Position from,
            Position to,
            out MoveResult result) 
            => TryRegularMove(
                new Move(from, to),
                out result);

        public bool TryRegularMove(
            Move move,
            out MoveResult result)
        {
            if (!IsLegalMove(
                move,
                ParentBoard,
                out GameError error))
            {
                result = new MoveResult(error, move);
                return false;
            }
            return ParentBoard.TryHandleMovePiece(
                moving_piece: this,
                move: move,
                moveResult: out result);
        }
    }
}
