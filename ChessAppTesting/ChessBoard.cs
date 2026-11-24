using ChessAppTesting.Pieces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Swift;
using System.Text;
using ChessAppTesting.Enums;
using ChessAppTesting.Structs;
using ChessAppTesting.Records;

namespace ChessAppTesting
{
    

    // Class to represent the chessboard
    public class ChessBoard
    {
        // == Properties ============================
        // Pieces
        public Dictionary<PieceID, Piece> Pieces { get; set; } = new Dictionary<PieceID, Piece>();
        public List<PieceID> WhitePieces { get; set; } = new List<PieceID>();
        public List<PieceID> BlackPieces { get; set; } = new List<PieceID>();


        // Squares
        public int max_x;
        public int max_y;
        private Square[,] Board { get; set; }

        // Game logic information
        public (PieceID, (int dx, int dy), (int new_x, int new_y)) LastMove { get; set; } = (PieceID.Empty, (0, 0), (0, 0));
        public bool WhiteKingMayCastle { get; set; } = true;
        public bool BlackKingMayCastle { get; set; } = true;

        private MoveError lastError { get; set; } = MoveError.None;
        private Move lastMove { get; set; } = new Move(new Position(0, 0), new Position(0, 0));

        // == Constructors ==========================
        public ChessBoard(int max_x = 8, int max_y = 8)
        {
            this.max_x = max_x;
            this.max_y = max_y;
            Board = new Square[this.max_x, this.max_y];

            for (int x = 0; x < this.max_x; x++)
            {
                for (int y = 0; y < this.max_y; y++)
                {
                    Board[x, y] = new Square
                    {
                        Coordinates = (x, y)
                    };
                }
            }
        }

        // == Methods ===============================

        public bool TryGetWhiteMoves(
            out List<(PieceID, MoveDelta)> legalMoves,
            out BoardError error)
        {
            return TryGetLegalMoves(
                WhitePieces,
                out legalMoves,
                out error);
        }

        public bool TryGetBlackMoves(
            out List<(PieceID, MoveDelta)> legalMoves,
            out BoardError error)
        {
            return TryGetLegalMoves(
                BlackPieces,
                out legalMoves,
                out error);
        }

        public bool TryGetLegalMoves(
            List<PieceID> pieces,
            out List<(PieceID, MoveDelta)> legalMoves,
            out BoardError error)
        {
            legalMoves = new();
            error = BoardError.None;

            Piece? piece;
            foreach (PieceID pieceID in pieces)
            {
                if (!Pieces.TryGetValue(pieceID, out piece))
                {
                    error = BoardError.UnknownPieceID;
                    continue;
                }
                foreach (MoveDelta move in piece.GetLegalMoves(this))
                {
                    legalMoves.Add((pieceID, move));
                }
            }
            return error == BoardError.None;
        }

        public bool TryHandleMovePiece(
            Piece           moving_piece, 
            Move            move,
            out MoveResult  moveResult)
        {
            lastMove = move;
            lastError = MoveError.None;

            if (!PositionInBoard(move.To))
            {
                lastError = MoveError.OutOfBounds;
                moveResult = new(lastError, lastMove);
                return false;
            }

            // If this position is occupied, remove the occupying piece from existence
            if (IsPositionOccupiedUnchecked(move.To))
            {
                PieceID takenPieceID = GetPieceIDAtPosition(move.To);
                switch (Pieces[takenPieceID].Color) 
                {
                    case TeamColor.White:
                        WhitePieces.Remove(takenPieceID);
                        break;
                    case TeamColor.Black:
                        BlackPieces.Remove(takenPieceID);
                        break;
                    default:
                        break;
                }
                Pieces.Remove(takenPieceID);
            }

            SetPieceAtPosition(moving_piece, move.To);
            moving_piece.Position = move.To;
            moveResult = new(lastError, lastMove);
            return true;
        }

        public bool TryAddPieceAtPosition(
            Piece piece,
            Position position,
            out BoardError error)
        {
            if (!PositionInBoard(position))
            {
                error = BoardError.OutOfBounds;
                return false;
            }

            if (IsPositionOccupiedUnchecked(position))
            {
                error = BoardError.CannotPlacePieceOnOccupiedSquare;
                return false;
            }

            if (Pieces.ContainsKey(piece.ID))
            {
                error = BoardError.CannotAddSamePieceTwice;
                return false;
            }

            error = BoardError.None;
            Pieces.Add(piece.ID, piece);
            SetPieceAtPosition(piece.ID, position);
            return true;
        }


        public bool TryGetPieceAtPosition(
            Position position,
            [NotNullWhen(true)] out Piece? piece,
            out BoardError error)
        {
            if (!TryGetPieceIDAtPosition(position, out PieceID pieceID, out error))
            {
                piece = null;
                return false;
            }
            return TryGetPieceByID(pieceID, out piece, out error);
        }

        public bool TryGetPieceIDAtPosition(
            Position position,
            out PieceID pieceID,
            out BoardError error)
        {
            if (!PositionInBoard(position))
            {
                pieceID = PieceID.Empty;
                error = BoardError.OutOfBounds;
                return false;
            }

            pieceID = GetPieceIDAtPosition(position);
            error = BoardError.None;
            return pieceID != PieceID.Empty;
        }

        public bool TryGetPieceByID(
            PieceID pieceID,
            [NotNullWhen(true)] out Piece? piece,
            out BoardError error)
        {
            if (!Pieces.TryGetValue(pieceID, out piece))
            {
                error = BoardError.UnknownPieceID;
                return false;
            }
            error = BoardError.None;
            return true;
        }

        public bool PositionInBoard(Position position)
            => position.X >= 0 &&
                position.X < max_x &&
                position.Y >= 0 &&
                position.Y < max_y;

        public bool IsPositionOccupied(Position position)
        {
            if (!PositionInBoard(position))
            {
                lastError = MoveError.OutOfBounds;
                return false;
            }
            return IsPositionOccupiedUnchecked(position);
        }

        private bool IsPositionOccupiedUnchecked(Position position)
        {
            return Board[position.X, position.Y].ContainsPiece();
        }

        private PieceID GetPieceIDAtPosition(Position position)
        {
            Board[position.X, position.Y].TryGetPiece(out PieceID pieceID);
            return pieceID;
        }
             
        private void ClearPieceAtPosition(Position position)
            => Board[position.X, position.Y].ClearPiece();

        private void SetPieceAtPosition(PieceID pieceID, Position position)
            => Board[position.X, position.Y].SetPiece(pieceID);
        private void SetPieceAtPosition(Piece piece, Position position)
            => Board[position.X, position.Y].SetPiece(piece.ID);

    }


}

    
