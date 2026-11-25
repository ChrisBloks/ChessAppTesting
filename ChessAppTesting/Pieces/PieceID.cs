using ChessAppTesting.Enums;
using ChessAppTesting.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ChessAppTesting.Pieces
{
    // Class to represent a unique identifier for each chess piece
    public class PieceID
    {
        // == Properties ============================
        public required string ID { get; set; }

        // Static property to represent an empty piece ID
        public static readonly PieceID Empty = new() { ID = "Empty" };


        // == Constructors ==========================
        [SetsRequiredMembers]
        public PieceID(TeamColor color, PieceType pieceType, int pieceCount)
        {
            string id = Utils.toPieceID(color) + Utils.toPieceID(pieceType) + Utils.toPieceID(pieceCount);
            //ID = Convert.ToInt32(id, 2);
            ID = id;
        }

        private PieceID() { } // Private constructor for the Empty property

        // == Methods ===============================
        public static PieceID CreateID(TeamColor color, PieceType pieceType, int pieceCount)
        {
            return new PieceID(color, pieceType, pieceCount);
        }

        public static string PreviewID(TeamColor color, PieceType pieceType, int pieceCount)
        {
            return Utils.toPieceID(color) + Utils.toPieceID(pieceType) + Utils.toPieceID(pieceCount);

        }

        public string toString()
        {
            return $"{ID}";
        }
    }
}
