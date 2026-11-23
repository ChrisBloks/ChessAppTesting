using ChessAppTesting.Enums;
using ChessAppTesting.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using UserCommand = ChessAppTesting.Records.UserCommand;

namespace ChessAppTesting
{
    internal class BaseGameManager
    {
        internal bool quit_game { get; set; } = false;
        internal required ChessBoard Board { get; set; }

        internal TeamColor current_player = TeamColor.White;

        // == Constructors ==========================
        [SetsRequiredMembers]
        public BaseGameManager(ChessBoard board)
        {
            Board = board;
        }

        public bool TryHandleUserCommand(UserCommand command, out string error)
        {
            error = "";
            switch (command)
            {
                case UserCommand.Move move:
                    // Handle move command
                    return TryHandleMove(move, out error);
                case UserCommand.Quit:
                    // Handle quit command
                    HandleQuitGame();
                    return true;
                default:
                    error = "Unknown command type.";
                    return false;
            }
        }
        private bool TryHandleMove(UserCommand.Move move, out string error)
        {
            // Get the piece on the From location
            if (!Board.TryGetPieceAtPosition(move.From, out Piece? selected_piece, out error))
            {
                return false;
            }

            if (!selected_piece.Color == current_player)
            {
                error = "Cannot move other players' pieces!";
                return false;
            }
            // See if the To location is a valid move-to
            if (!selected_piece.IsLegalMove(move.From, move.To))
            {
                error = "Illegal move!";
                return false;
            }
            // update the game
            return selected_piece.TryRegularMove(move.From, move.To, out error);
        }

        private void HandleQuitGame()
        {
            Console.WriteLine("Handling quit command.");
            this.quit_game = true;
            return;
        }
    }


}
