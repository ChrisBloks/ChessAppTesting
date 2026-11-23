using ChessAppTesting.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace ChessAppTesting
{
    // Class to handle loading the chessboard from a csv file
    internal class BoardLoader
    {
        public static bool TryLoadFromCSV(string csvFilePath,[NotNullWhen(true)] out ChessBoard? board, out string error)
        {

            string[] lines;
            // Read the CSV file
            try
            { 
                lines = System.IO.File.ReadAllLines(csvFilePath);
            }
            catch (Exception ex)
            {
                board = null;
                error = $"Error loading board from CSV: {ex.Message}";
                return false;
            }

            int max_x = 8;
            int max_y = 8;
            board = new ChessBoard(max_x, max_y);
            error = string.Empty;
            // Skip the header line and process each piece
            for (int i = 1; i < lines.Length; i++)
            {
                try
                {
                    var parts = lines[i].Split(',');
                    if (parts.Length != 4)
                    {
                        throw new FormatException($"Invalid line format: {lines[i]}");
                    }

                    string colour = parts[0].Trim();
                    if (colour is not "W" and not "B")
                    {
                        throw new ArgumentException($"Invalid colour: {colour}");
                    }
                    string pieceType = parts[1].Trim();
                    if (pieceType is not "P" and not "N" and not "B" and not "R" and not "Q" and not "K")
                    {
                        throw new ArgumentException($"Invalid piece type: {pieceType}");
                    }

                    int x = int.Parse(parts[2].Trim());
                    int y = int.Parse(parts[3].Trim());
                    if (!board.PositionInBoard((x, y)))
                    {
                        throw new ArgumentOutOfRangeException($"Position out of bounds: ({x}, {y})");
                    }
                    if (board.Board[x, y].ContainsPiece())
                    {
                        throw new InvalidOperationException($"Square ({x}, {y}) is already occupied.");
                    }

                    // Create a new piece based on the type and colour
                    Piece piece = CreatePiece(board, colour, pieceType, x, y);
                    // Add the piece to the chessboard
                    board.Pieces[piece.ID] = piece;
                    board.Board[x, y].SetPiece(piece.ID);
                    // Add to the appropriate list of pieces
                    if (colour == "W")
                    {
                        board.WhitePieces.Add(piece.ID);
                    }
                    else if (colour == "B")
                    {
                        board.BlackPieces.Add(piece.ID);
                    }
                }
                catch (Exception ex)
                {
                    error = $"Error processing line {i + 1}: {ex.Message}";
                    return false;
                }
            }
            return true;
        }

        public static Piece CreatePiece(ChessBoard board, string colour, string pieceType, int x, int y)
        {
            if (colour is not "W" and not "B")
            {
                throw new ArgumentException($"Invalid colour: {colour}");
            }

            if (pieceType is not "P" and not "N" and not "B" and not "R" and not "Q" and not "K")
            {
                throw new ArgumentException($"Invalid piece type: {pieceType}");
            }

            var position = (x, y);

            if (!board.PositionInBoard(position))
            {
                throw new ArgumentException($"Position out of bounds: ({x}, {y})");
            }

            return pieceType switch
            {
                "P" => (colour == "W" ? new WhitePawn(board, position) : new BlackPawn(board, position)),
                // Add other piece types here as needed
                _ => throw new ArgumentException($"Unknown piece type: {pieceType}")
            };
        }
    }

  

}
