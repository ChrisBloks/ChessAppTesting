using ChessAppTesting.Enums;
using ChessAppTesting.GUI;
using ChessAppTesting.Pieces;
using ChessAppTesting.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using PlayerAction = ChessAppTesting.Records.PlayerAction;

namespace ChessAppTesting
{
    internal class Test
    {
        public static void Run()
        {
            string BoardsPath = @"C:\Users\chris\source\repos\ChessAppTesting\ChessAppTesting\InitialBoardStates\";
            // Load the chessboard from a file
            string file = BoardsPath + "Test_1.csv";

            if (!BoardLoader.TryLoadFromCSV(file, out ChessBoard? board, out string? error))
            {
                Console.WriteLine($"Error: {error}");
            }
            else
            {
                ConsoleGui gui = ConsoleGui.Instance;

                // Display the chessboard
                List<string> test_moves = [
                    "move A2 A3",
                      "move D7 d5",
                    "move C2 C4",
                      "move D5 C4",
                    "move B2 B3",
                      "move E7 E5",
                    "move C1 B2",
                      "move C4 B3",
                    "move B2 E5",

                    ];
                SimulateGameDebug(
                    board: board,
                    moves: test_moves,
                    error: out error
                );
                Console.WriteLine($"Error?: {error}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(false);
            }

        }

        public static void SimulateGame(ChessBoard board, IGUI gui, int i_max, out string? error)
        {
            IActionResult actionResult;
            error = string.Empty;
            BaseGameManager gm = new BaseGameManager(board);
            Records.PlayerAction? userCommand = null;
            for (int i = 0; i <= i_max; i++)
            {
                userCommand = gui.Display(board);
                if (!gm.TryHandleUserCommand(userCommand, out actionResult))
                {
                    error = actionResult.Message;
                    Console.WriteLine($"Error: {error}");
                }
                else
                {
                    Console.WriteLine("Success");
                }
            }
        }

        public static void SimulateGameDebug(
            ChessBoard board,
            List<string> moves,
            out string? error)
        {
            error = string.Empty;
            BaseGameManager gm = new BaseGameManager(board);
            ConsoleDebugGui gui = ConsoleDebugGui.Instance;

            List<PlayerAction> cmds = [];
            string input = string.Empty;
            bool parsed_correctly = true;
            for(int i = 0; i < moves.Count; i++)
            {
                input = moves[i];
                if (!gui.TryInterpretCommand(board, input, out PlayerAction? cmd, out error))
                {
                    Console.WriteLine($"Error parsing commands, reading line {i}:");
                    Console.WriteLine(input);
                    Console.WriteLine(error);
                    parsed_correctly = false;
                }
                else
                {
                    cmds.Add(cmd);
                }
            }

            if (!parsed_correctly)
            {
                Console.WriteLine("Parsing failed, skipping display");
                return;
            }

            foreach (var cmd in cmds)
            {
                gui.Display(board, cmd);
                if (!gm.TryHandleUserCommand(cmd, out IActionResult result))
                {
                    error = result.Message; 
                    Console.WriteLine($"Error handling command {cmd}");
                    return;
                }
            }
            gui.DisplayBoard(board);
            error = "Simulation successful, no errors encountered.";
        }
    }
}
