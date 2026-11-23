using ChessAppTesting.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using UserCommand = ChessAppTesting.Records.UserCommand;

namespace ChessAppTesting
{
    internal class Test
    {
        public static void Run()
        {
            string BoardsPath = @"C:\Users\chris\source\repos\ChessAppTesting\ChessAppTesting\InitialBoardStates\";
            // Load the chessboard from a file
            string file = BoardsPath + "Test_1.csv";

            if (!BoardLoader.TryLoadFromCSV(file, out ChessBoard? board, out string error))
            {
                Console.WriteLine($"Error: {error}");
            }
            else
            {
                ConsoleGui gui = ConsoleGui.Instance;

                // Display the chessboard
                List<string> test_moves = [
                    "move A2 A3",
                    "move A3 A4",
                    "move B2 C3",
                    "move B1 B3"
                    ];
                SimulateGameDebug(
                    board: board,
                    moves: test_moves,
                    error: out error
                );
                Console.WriteLine($"Error?: {error}");
            }

        }

        public static void SimulateGame(ChessBoard board, IGUI gui, int i_max, out string error)
        {
            error = string.Empty;
            BaseGameManager gm = new BaseGameManager(board);
            Records.UserCommand? userCommand = null;
            for (int i = 0; i <= i_max; i++)
            {
                userCommand = gui.Display(board);
                if (!gm.TryHandleUserCommand(userCommand, out error))
                {
                    Console.WriteLine($"Error: {error}");
                }
                else
                {
                    Console.WriteLine("Success");
                }
            }
        }

        public static void SimulateGameDebug(ChessBoard board, List<string> moves, out string error)
        {
            error = string.Empty;
            BaseGameManager gm = new BaseGameManager(board);
            ConsoleDebugGui gui = ConsoleDebugGui.Instance;

            List<UserCommand> cmds = [];
            string input = string.Empty;
            bool parsed_correctly = true;
            for(int i = 0; i < moves.Count; i++)
            {
                input = moves[i];
                if (!gui.TryInterpretCommand(board, input, out UserCommand? cmd, out error))
                {
                    Console.WriteLine($"Error parsing commands, reading line {i}");
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
                if (!gm.TryHandleUserCommand(cmd, out error))
                {
                    Console.WriteLine($"Error handling command {cmd}");
                    return;
                }
            }
        }
    }
}
