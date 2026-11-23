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
        public required (int, int) Coordinates { get; set; }
        private PieceID? _piece_id;

        // Methods
        public bool TryGetPiece([NotNullWhen(true)] out PieceID? pieceID, out string error)
        {
            if (_piece_id == null)
            {
                pieceID = null;
                error = "No piece on this square.";
                return false;
            }
            pieceID = _piece_id;
            error = string.Empty;
            return true;
        }
        public bool ContainsPiece()
        {
            return _piece_id != null;
        }

        public void SetPiece(PieceID id)
        {
            _piece_id = id;
        }

        public void ClearPiece()
        {
            _piece_id = null;
        }

    }
}
