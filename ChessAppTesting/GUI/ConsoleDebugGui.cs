using ChessAppTesting.Records;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using PlayerAction = ChessAppTesting.Records.PlayerAction;

namespace ChessAppTesting.GUI
{
    internal sealed class ConsoleDebugGui
    {
        private static readonly Lazy<ConsoleDebugGui> _instance = new(() => new ConsoleDebugGui());
        public static ConsoleDebugGui Instance => _instance.Value;
        private ConsoleDebugGui() { }

        public void Display(ChessBoard board, PlayerAction cmd)
        {
            DisplayBoard(board);
            DisplayUI(cmd);
        }


        // ========================================
        // DISPLAY GAME STATE
        // ========================================
        public void DisplayBoard(ChessBoard chessBoard)
        {
            // At the top  of the Board, print the column name
            Console.WriteLine("    A  B  C  D  E  F  G  H ");
            for (int y = chessBoard.max_y - 1; y >= 0; y--)
            {
                // At the start of the Board print the row number
                Console.Write($" {y + 1} ");
                for (int x = 0; x < chessBoard.max_x; x++)
                {
                    bool isLightSquare = (x + y) % 2 == 0;
                    ConsoleColor bg_color = isLightSquare ? ConsoleColor.Gray : ConsoleColor.DarkGray;
                    ConsoleColor fg_ex_color = !isLightSquare ? ConsoleColor.Gray : ConsoleColor.DarkGray;
                    if (chessBoard.TryGetPieceAtPosition((x, y), out var piece, out _))
                    {
                        DisplayPiece(
                            piece,
                            bg_color
                        );
                    }
                    else
                    {
                        WriteColoredText(
                            "[ ]",
                            fg: fg_ex_color,
                            bg: bg_color
                        );
                    }
                }
                // At the end of the Board print the row number
                Console.Write($" {y+1} ");
                Console.WriteLine();
            }
            // At the bottom of the Board, print the column name
            Console.WriteLine("    A  B  C  D  E  F  G  H ");
        }

        public void DisplayPiece(Pieces.Piece piece, ConsoleColor bg_color)
        {
            WriteColoredText(
                $"[{GetPieceSymbol(piece.Type)}]",
                fg: GetTeamTextColor(piece.Color),
                bg: bg_color
            );
        }

        public void WriteColoredText(string text, ConsoleColor? fg = null, ConsoleColor? bg = null)
        {
            if (fg is not null) Console.ForegroundColor = fg.Value;
            if (bg is not null) Console.BackgroundColor = bg.Value;
            Console.Write(text);
            Console.ResetColor();
        }

        public string GetPieceSymbol(Enums.PieceType pieceType)
        {
            return pieceType switch
            {
                Enums.PieceType.Pawn => "P",
                Enums.PieceType.Knight => "N",
                Enums.PieceType.Bishop => "B",
                Enums.PieceType.Rook => "R",
                Enums.PieceType.Queen => "Q",
                Enums.PieceType.King => "K",
                _ => "?"
            };
        }

        public ConsoleColor GetTeamTextColor(Enums.TeamColor teamColor)
        {
            return teamColor switch
            {
                Enums.TeamColor.White => ConsoleColor.White,
                Enums.TeamColor.Black => ConsoleColor.DarkYellow,
                _ => ConsoleColor.Gray
            };
        }

        // ========================================
        // DISPLAY USER INTERFACE
        // ========================================
        public void DisplayUI(PlayerAction move)
        {
            Console.WriteLine($"Handling cmd: {move}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(false);
        }

        // ========================================
        // HANDLE USER INPUT
        // ========================================
        public bool TryInterpretCommand(ChessBoard board, string userInput, [NotNullWhen(true)] out PlayerAction? command, out string error)
        {
            error = "";
            userInput = userInput.Trim().ToLowerInvariant();
            var parts = userInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            (int x, int y) from_coords = (-1, -1);
            (int x, int y) to_coords = (-1, -1);

            switch (parts)
            {
                case ["quit"] or ["q"]:
                    command = new PlayerAction.Quit();
                    return true;

                case ["move", var from, var to]:
                    if (TryParseAsCoordinates(from, out from_coords) &&
                        TryParseAsCoordinates(to, out to_coords))
                    {
                        command = new PlayerAction.MoveAction(from_coords, to_coords);
                        return true;
                    }
                    else
                    {
                        command = null;
                        error = "Invalid coordinates.";
                        return false;
                    }
                case ["help"] or ["h"]:
                    command = null;
                    error = "Valid commands: ['q', 'quit', 'move A1 B2', 'h', 'help']";
                    return false;

                default:
                    command = null;
                    error = "Invalid command.";
                    return false;
            }
        }

        private bool TryParseAsCoordinates(string input, out (int x, int y) coordinates)
        {
            coordinates = default;

            if (input is not [var file, var rank] ||
                !char.IsAsciiLetter(file) ||
                !char.IsAsciiDigit(rank))
            {
                return false;
            }

            coordinates = (char.ToLowerInvariant(file) - 'a', rank - '1');
            return true;
        }
    }
}
