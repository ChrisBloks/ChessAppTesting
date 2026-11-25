using ChessAppTesting.Pieces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Swift;
using System.Text;
using ChessAppTesting.Enums;
using ChessAppTesting.Records;

namespace ChessAppTesting
{
    

    // Class to represent the chessboard
    public class ChessBoard
    {
        // == Properties ============================
        // Pieces
        public Dictionary<PieceID, Piece> Pieces { get; set; } = new Dictionary<PieceID, Piece>();
        private Dictionary<PieceID, Position> PieceLocations { get; set; } = new();
        public List<PieceID> WhitePieces { get; set; } = new List<PieceID>();
        public List<PieceID> BlackPieces { get; set; } = new List<PieceID>();
        

        // Squares
        public int max_x;
        public int max_y;
        private Square[,] Board { get; set; }

        // Game logic information
        public (PieceID, Move) LastMove { get; set; } = (PieceID.Empty, ((0, 0), (0, 0)));
        public bool WhiteKingMayCastle { get; set; } = true;
        public bool BlackKingMayCastle { get; set; } = true;

        private GameError lastError { get; set; } = GameError.None;
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
            out GameError error)
        {
            return TryGetLegalMoves(
                WhitePieces,
                out legalMoves,
                out error);
        }

        public bool TryGetBlackMoves(
            out List<(PieceID, MoveDelta)> legalMoves,
            out GameError error)
        {
            return TryGetLegalMoves(
                BlackPieces,
                out legalMoves,
                out error);
        }

        public bool TryGetLegalMoves(
            List<PieceID> pieces,
            out List<(PieceID, MoveDelta)> legalMoves,
            out GameError error)
        {
            legalMoves = new();
            error = GameError.None;

            Piece? piece;
            foreach (PieceID pieceID in pieces)
            {
                if (!Pieces.TryGetValue(pieceID, out piece))
                {
                    error = GameError.BoardCannotFindPieceID;
                    continue;
                }
                foreach (MoveDelta move in piece.GetLegalMoves(this))
                {
                    legalMoves.Add((pieceID, move));
                }
            }
            return error == GameError.None;
        }

        public bool TryHandleMovePiece(
            Piece           moving_piece, 
            Move            move,
            out MoveResult  moveResult)
        {
            lastMove = move;
            lastError = GameError.None;

            if (!PositionInBoard(move.To))
            {
                lastError = GameError.BoardPositionOutOfBounds;
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
                PieceLocations.Remove(takenPieceID);
                ClearPieceAtPosition(move.To);
            }

            UpdatePiecePosition(
                piece: moving_piece,
                oldPos: move.From,
                newPos: move.To);
            moveResult = new(lastError, lastMove);
            return true;
        }

        public bool TryAddPieceAtPosition(
            Piece piece,
            Position position,
            out GameError error)
        {
            if (!PositionInBoard(position))
            {
                error = GameError.BoardPositionOutOfBounds;
                return false;
            }

            if (IsPositionOccupiedUnchecked(position))
            {
                error = GameError.BoardCannotPlacePieceOnOccupiedSquare;
                return false;
            }

            if (Pieces.ContainsKey(piece.ID))
            {
                error = GameError.BoardCannotAddSamePieceTwice;
                return false;
            }

            error = GameError.None;
            Pieces.Add(piece.ID, piece);
            switch (piece.Color)
            {
                case TeamColor.White:
                    WhitePieces.Add(piece.ID);
                    break;
                case TeamColor.Black:
                    BlackPieces.Add(piece.ID);
                    break;
                default:
                    break;
            }

            UpdatePiecePosition(
                piece: piece,
                oldPos: null,
                newPos: position);
            return true;
        }


        public bool TryGetPieceAtPosition(
            Position position,
            [NotNullWhen(true)] out Piece? piece,
            out GameError error)
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
            out GameError error)
        {
            if (!PositionInBoard(position))
            {
                pieceID = PieceID.Empty;
                error = GameError.BoardPositionOutOfBounds;
                return false;
            }

            pieceID = GetPieceIDAtPosition(position);
            error = GameError.None;
            return pieceID != PieceID.Empty;
        }

        public bool TryGetPieceByID(
            PieceID pieceID,
            [NotNullWhen(true)] out Piece? piece,
            out GameError error)
        {
            if (!Pieces.TryGetValue(pieceID, out piece))
            {
                error = GameError.BoardCannotFindPieceID;
                return false;
            }
            error = GameError.None;
            return true;
        }

        public bool PositionInBoard(Position position)
            => position.X >= 0 &&
                position.X < max_x &&
                position.Y >= 0 &&
                position.Y < max_y;

        public bool IsPositionOccupied(int x, int y)
            => IsPositionOccupied(new Position(x, y));

        public bool IsPositionOccupied(Position position)
        {
            if (!PositionInBoard(position))
            {
                lastError = GameError.BoardPositionOutOfBounds;
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


        public bool TryGetPiecePosition(Piece piece, out Position pos)
            => TryGetPiecePosition(piece.ID, out pos);
        public bool TryGetPiecePosition(PieceID id, out Position pos)
            => PieceLocations.TryGetValue(id, out pos);

        private void UpdatePiecePosition(Piece piece, Position? oldPos, Position newPos)
            => UpdatePiecePosition(piece.ID, oldPos, newPos);
        private void UpdatePiecePosition(PieceID ID, Position? oldPos, Position newPos)
        {
            if(oldPos != null)
            {
                ClearPieceAtPosition(oldPos);
            }
            PieceLocations[ID] = newPos;
            SetPieceAtPosition(ID, newPos);
        }

        
        private Position GetPiecePosition(Piece piece)
            => GetPiecePosition(piece.ID);
        public Position GetPiecePosition(PieceID id)
            => PieceLocations[id];

        private void ClearPieceAtPosition(Position position)
            => Board[position.X, position.Y].ClearPiece();

        private void SetPieceAtPosition(PieceID pieceID, Position position)
            => Board[position.X, position.Y].SetPiece(pieceID);
        private void SetPieceAtPosition(Piece piece, Position position)
            => Board[position.X, position.Y].SetPiece(piece.ID);

    }


}

    
