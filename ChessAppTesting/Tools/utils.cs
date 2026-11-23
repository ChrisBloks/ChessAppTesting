using ChessAppTesting.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAppTesting.Tools
{
    internal class Utils
    {
        public static string toPieceID(TeamColor color)
        {
            //return color == TeamColor.White ? "0" : "1";
            return color switch
            {
                TeamColor.White => "W",
                TeamColor.Black => "B",
                _ => throw new ArgumentException("Invalid team color"),
            };
        }

        public static string toPieceID(PieceType pieceType)
        {
            return pieceType switch
            {
                PieceType.None => "NULL",//"000",
                PieceType.King => "K",//"001",
                PieceType.Pawn => "P",//"010",
                PieceType.Knight => "N",//"011",
                PieceType.Bishop => "B",//"100",
                PieceType.Rook => "R",//"101",
                PieceType.Queen => "Q",//"110",
                _ => throw new ArgumentException("Invalid piece type"),
            };
        }

        public static string toPieceID(int pieceCount)
        {
            return Convert.ToString(pieceCount);
            //if (pieceCount < 0 || pieceCount > 63)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(pieceCount), "Piece count must be between 0 and 63 (6 bits).");
            //}
            //return Convert.ToString(pieceCount, 2).PadLeft(6, '0');
        }

        public static void DescribePiece(Pieces.Piece piece)
        {
            if (piece != null)
            {
                Console.WriteLine($"Piece ID: {piece.ID}");
                Console.WriteLine($"Position: ({piece.Position.x}, {piece.Position.y})");
                Console.WriteLine("Candidate Moves:");
                foreach (var move in piece.CandidateMoves)
                {
                    Console.WriteLine($"({move.Item1}, {move.Item2})");
                }
            }
            else
            {
                Console.WriteLine("No piece to describe.");
            }
        }

        public static bool TryColToNumber(char col, out int i)
        {
            string cols = "ABCDEFGH";
            i = cols.IndexOf(char.ToUpper(col));
            return i != -1;
        }

        public static bool TryNumberToCol(int i, out char col)
        {
            string cols = "ABCDEFGH";
            try
            {
                col = cols[i];
            }
            catch (IndexOutOfRangeException e)
            {
                col = '?';
                return false;
            }
            return true;
        }
    }
}
