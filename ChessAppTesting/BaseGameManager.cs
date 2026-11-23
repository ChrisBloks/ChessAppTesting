using ChessAppTesting.Enums;
using ChessAppTesting.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static ChessAppTesting.Records.PlayerAction;
using PlayerAction = ChessAppTesting.Records.PlayerAction;

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

        public bool TryHandleUserCommand(PlayerAction command, out string? error)
        {
            error = "";
            bool success = false;
            switch (command)
            {
                case PlayerAction.MoveAction move:
                    // Handle move command
                    success = TryHandleMove(move, out error);
                    break;
                case PlayerAction.TakeAction take:
                    success = TryHandleTake(take, out error);
                    break;
                case PlayerAction.Quit:
                    // Handle quit command
                    HandleQuitGame();
                    success = true;
                    break;
                default:
                    error = "Unknown command type.";
                    success = false;
                    break;
            }
            if (success)
            {
                UpdateCurrentPlayer();
            }

            return success;
        }
        private bool TryHandleMove(MoveAction move, out string? error)
        {
            // Get the piece on the From location
            if (!Board.TryGetPieceAtPosition(move.From, out Piece? selected_piece, out error))
            {
                return false;
            }

            if (selected_piece.Color != current_player)
            {
                error = "Cannot move other players' pieces!";
                return false;
            }

            // See if the To location is a valid move-to
            if (!selected_piece.IsLegalMove(move.From, move.To, out error))
            {
                error = "Illegal move!";
                return false;
            }
            // update the game
            return selected_piece.TryRegularMove(move.From, move.To, out error);
        }

        private bool TryHandleTake(TakeAction take, out string? error)
        {
            // Get the piece on the From location
            if (!Board.TryGetPieceAtPosition(take.From, out Piece? selected_piece, out error))
            {
                return false;
            }

            if (selected_piece.Color != current_player)
            {
                error = "Cannot move other players' pieces!";
                return false;
            }
            // update the game
            return selected_piece.TryRegularTake(take.From, take.To, out error);
        }

        private void HandleQuitGame()
        {
            Console.WriteLine("Handling quit command.");
            this.quit_game = true;
            return;
        }

        private void UpdateCurrentPlayer()
        {
            switch (current_player)
            {
                case TeamColor.White:
                    current_player = TeamColor.Black;
                    break;
                case TeamColor.Black:
                    current_player = TeamColor.White;
                    break;
                default:
                    break;
            }
        }
    }


}
