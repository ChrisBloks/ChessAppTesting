using ChessAppTesting.Pieces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Swift;
using System.Text;

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
        public bool PositionInBoard((int x, int y) position)
        {
            return position.x >= 0  &&
                position.x < max_x &&
                position.y >= 0     &&
                position.y < max_y;
        }

        public List<(PieceID, (int dx, int dy))> GetWhiteMoves()
        {
            return GetLegalMoves(WhitePieces);
        }

        public List<(PieceID, (int dx, int dy))> GetBlackMoves()
        {
            return GetLegalMoves(BlackPieces);
        }   

        public List<(PieceID, (int dx, int dy))> GetLegalMoves(List<PieceID> pieces)
        {
            List<(PieceID, (int dx, int dy))> legalMoves = new List<(PieceID, (int dx, int dy))>();
            foreach (var pieceID in pieces)
            {
                if (!Pieces.ContainsKey(pieceID))
                {
                    continue;
                }
                var piece = Pieces[pieceID];
                foreach (var move in piece.GetLegalMoves(this))
                {
                    legalMoves.Add((pieceID, move));
                }
            }
            return legalMoves;
        }

        public bool TryHandleMovePiece(
            Piece           moving_piece, 
            (int x, int y)  old_position, 
            (int x, int y)  new_position,
            out string      error)
        {
            if (!PositionInBoard(old_position) || !PositionInBoard(new_position))
            {
                error = $"Failed to move piece on the board in ChessBoard.TryHandleMovePiece {old_position} {new_position}";
                return false;
            }

            Board[old_position.x, old_position.y].ClearPiece();
            Board[new_position.x, new_position.y].SetPiece(moving_piece.ID);
            moving_piece.Position = new_position;

            error = string.Empty;
            return true;
        }

        public bool TryHandleTakePiece(
            Piece taking_piece,
            (int x, int y) old_position, 
            (int x, int y) new_position,
            out string error)
        {
            if (!PositionInBoard(old_position) || !PositionInBoard(new_position))
            {
                error = $"Failed to move piece on the board in ChessBoard.TryHandleTakePiece {old_position} {new_position}";
                return false;
            }

            if (!TryGetPieceAtPosition(new_position, out Piece? taken_piece, out error))
            {
                error = $"Failed to find a piece to take ChessBoard.TryHandleTakePiece {old_position} {new_position}";
                return false;
            }

            Board[old_position.x, old_position.y].ClearPiece();
            Board[new_position.x, new_position.y].SetPiece(taking_piece.ID);
            taking_piece.Position = new_position;

            Pieces.Remove(taken_piece.ID);
            if (taken_piece.Color == Enums.TeamColor.White)
            {
                WhitePieces.Remove(taken_piece.ID);
            }
            else
            {
                BlackPieces.Remove(taken_piece.ID);
            }
            error = string.Empty;
            return true;
        }

        public bool TryGetPieceByID(PieceID pieceID, [NotNullWhen(true)] out Piece? piece, out string error)
        {
            if (!Pieces.TryGetValue(pieceID, out piece))
            {
                error = $"Piece {pieceID} not found in Pieces dict!.";
                return false;
            }
            error = string.Empty;
            return true;
        }

        public bool TryGetPieceIDAtPosition((int x, int y) position, [NotNullWhen(true)] out PieceID? pieceID, out string error)
        {
            if (!PositionInBoard(position))
            {
                pieceID = null;
                error = "Position (" + position.x + ", " + position.y + ") is out of bounds.";
                return false;
            }

            return Board[position.x, position.y].TryGetPiece(out pieceID, out error);
        }

        public bool IsPositionOccupied((int x, int y) position, out string error)
        {
            if (!PositionInBoard(position))
            {
                error = "Position (" + position.x + ", " + position.y + ") is out of bounds.";
                return false;
            }
            error = string.Empty;
            return Board[position.x, position.y].ContainsPiece();
        }


        public bool TryGetPieceAtPosition((int x, int y) position, [NotNullWhen(true)] out Piece? piece, out string error)
        {
            if (!TryGetPieceIDAtPosition(position, out var pieceID, out error))
            {
                piece = null;
                return false;
            }
            return TryGetPieceByID(pieceID, out piece, out error);
        }

        public bool TryAddPieceAtPosition(Piece piece, (int x, int y) position, out string error)
        {
            if(!PositionInBoard(position))
            {
                error = $"Position ({position.x},{position.y}) is out of bounds for board ({max_x},{max_y})";
                return false;
            }

            if(Board[position.x, position.y].ContainsPiece())
            {
                error = $"Location ({position.x}, {position.y}) is already occupied.";
                return false;
            }

            if (Pieces.TryGetValue(piece.ID, out Piece? existing_piece))
            {
                error = $"Piece {piece.ID} already exists at location {existing_piece.Position}";
                return false;
            }

            error = string.Empty;
            Pieces.Add(piece.ID, piece);
            Board[position.x, position.y].SetPiece(piece.ID);
            return true;
        }
    }


}

    
