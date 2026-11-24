using ChessAppTesting.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace ChessAppTesting
{
    // Class to represent a square on the chessboard
    public class Square
    {
        // Properties
        public required (int x, int y) Coordinates { get; set; }
        private PieceID _piece_id = PieceID.Empty;

        // Methods
        public bool TryGetPiece([NotNullWhen(true)] out PieceID pieceID)
        {
            pieceID = _piece_id;
            return pieceID != PieceID.Empty;
        }
        
        public bool ContainsPiece()
        {
            return _piece_id != PieceID.Empty;
        }

        public void SetPiece(PieceID id)
        {
            _piece_id = id;
        }

        public void ClearPiece()
        {
            _piece_id = PieceID.Empty;
        }

    }
}
