using System;
using System.Collections.Generic;
using System.Text;
using ChessAppTesting.Pieces;

namespace ChessAppTesting.Tests.BaseChessTests
{
    public class PawnTests
    {
        [Fact]
        public void WhitePawn_IsAddedToBoard()
        {
            ChessBoard board = new ChessBoard(5, 5);
            var pawn = new WhitePawn(board, (0, 1));

            Assert.True(board.TryGetPieceAtPosition((0,1), out Piece? mystery_piece, out string error), error);
            Assert.Equal(pawn, mystery_piece);

            
        }

        [Fact]
        public void WhitePawn_CanForwardMove()
        {
            ChessBoard board = new ChessBoard(5, 5);

            var pawn = new WhitePawn(board, (0, 1));
            pawn.TryRegularMove(move: (0, 1), out string _);


            Assert.Equal((0, 2), pawn.Position);
            Assert.True(board.TryGetPieceAtPosition(pawn.Position, out Piece? mystery_piece, out string error), error);
            Assert.Equal(pawn, mystery_piece);
        }

        [Fact]
        public void WhitePawn_IsBlockedByOtherPiece()
        {
            ChessBoard board = new ChessBoard(5, 5);
            var pawn = new WhitePawn(board, (0, 1));
            var other_pawn = new WhitePawn(board, (0, 2));

            pawn.TryRegularMove(move: (0, 1), out string _);

            Assert.Equal(pawn.Position, (0, 1));
        }
    }
}
