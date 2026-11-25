using ChessAppTesting.Enums;
using ChessAppTesting.Pieces;
using ChessAppTesting.Records;
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

        public bool TryHandleUserCommand(PlayerAction command, out IActionResult result)
        { 
            bool success;
            switch (command)
            {
                case MoveAction move:
                    // Handle move command
                    success = TryHandleMove(move, out MoveResult moveResult);
                    result = moveResult;
                    break;
                case Quit:
                    // Handle quit command
                    HandleQuitGame();
                    result = new ActionResult(GameError.None);
                    success = true;
                    break;
                default:
                    result = new ActionResult(GameError.PlayerGaveUnkownCommand);
                    success = false;
                    break;
            }

            if (success)
            {
                UpdateCurrentPlayer();
            }

            return success;
        }
        private bool TryHandleMove(MoveAction moveAction, out MoveResult result)
        {
            // Get the piece on the From location
            if (!Board.TryGetPieceAtPosition(moveAction.Move.From, out Piece? selectedPiece, out GameError error))
            {
                result = new MoveResult(error, moveAction);
                return false;
            }

            if (selectedPiece.Color != current_player)
            {
                result = new MoveResult(GameError.PlayerCannotMoveOthersPieces, moveAction);
                return false;
            }

            // See if the To location is a valid move-to
            if (!selectedPiece.IsLegalMove(moveAction.Move, out error))
            {
                result = new MoveResult(error, moveAction);
                return false;
            }

            // update the game
            return selectedPiece.TryRegularMove(moveAction.Move, out result);
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
