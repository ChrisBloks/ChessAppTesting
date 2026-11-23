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
        public void WhitePawn_CanMoveForwardTwiceFromSecondRow()
        {
            ChessBoard board = new ChessBoard(5, 5);

            var pawn = new WhitePawn(board, (0, 1));
            pawn.TryRegularMove((0, 2), out string _);

            Assert.Equal((0, 3), pawn.Position);
            Assert.True(board.TryGetPieceAtPosition(pawn.Position, out Piece? mystery_piece, out string error), error);
            Assert.Equal(pawn, mystery_piece);
        }

        [Fact]
        public void WhitePawn_IsBlockedByOtherPiece()
        {
            ChessBoard board = new ChessBoard(5, 5);
            var pawn = new WhitePawn(board, (0, 1));
            var other_pawn = new WhitePawn(board, (0, 2));

            Assert.False(pawn.TryRegularMove(move: (0, 1), out string _));
            Assert.Equal(pawn.Position, (0, 1));
            Assert.Equal(other_pawn.Position, (0, 2));
        }

        [Fact]
        public void WhitePawn_CanTakePiece()
        {
            ChessBoard board = new ChessBoard(2, 2);
            var pawn = new WhitePawn(board, (0, 0));
            var other_pawn = new BlackPawn(board, (1, 1));

            Assert.True(
                pawn.TryRegularTake(
                    move: (1, 1),
                    out string error
                ),
                error
             );
            Assert.Equal((1, 1), pawn.Position);
            Assert.True(board.TryGetPieceAtPosition(pawn.Position, out Piece? mystery_piece, out error), error);
            Assert.Equal(pawn, mystery_piece);
        }

        [Fact]
        public void WhitePawn_CannotTakeOwnPiece()
        {
            ChessBoard board = new ChessBoard(2, 2);
            var pawn = new WhitePawn(board, (0, 0));
            var other_pawn = new WhitePawn(board, (1, 1));

            Assert.False(
                pawn.TryRegularTake(
                    move: (1, 1),
                    out string error
                ),
                error
             );
            Assert.Equal((0, 0), pawn.Position);
            Assert.True(board.TryGetPieceAtPosition((1,1), out Piece? mystery_piece, out error), error);
            Assert.Equal(other_pawn, mystery_piece);
        }
    }
}
