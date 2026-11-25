using ChessAppTesting.Enums;
using ChessAppTesting.Pieces;
using ChessAppTesting.Records;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ChessAppTesting.Tests.BaseChessTests
{
    public class PawnTests
    {
        [Fact]
        public void WhitePawn_IsAddedToBoard()
        {
            GameError error;
            ChessBoard board = new ChessBoard(5, 5);
            var pawn = new Pawn(board, TeamColor.White, (0, 1));

            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (0,1),
                    piece: out Piece? mystery_piece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(pawn, mystery_piece);
        }

        [Fact]
        public void WhitePawn_CanForwardMove()
        {
            GameError error;
            ChessBoard board = new ChessBoard(5, 5);

            var pawn = new Pawn(board, TeamColor.White, (0, 1));
            pawn.TryRegularMove(moveDelta: (0, 1), out _);


            Assert.Equal((0, 2), pawn.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: pawn.Position,
                    piece: out Piece? mystery_piece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(pawn, mystery_piece);
        }

        [Fact]
        public void BlackPawn_CanForwardMove()
        {
            GameError error;
            ChessBoard board = new ChessBoard(5, 5);

            var pawn = new Pawn(board, TeamColor.Black, (0, 4));
            pawn.TryRegularMove(moveDelta: (0, -1), out _);


            Assert.Equal((0, 3), pawn.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: pawn.Position,
                    piece: out Piece? mystery_piece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(pawn, mystery_piece);
        }

        [Fact]
        public void WhitePawn_CanMoveForwardTwiceFromSecondRow()
        {
            MoveResult result;
            GameError error;
            ChessBoard board = new ChessBoard(5, 5);

            var pawn = new Pawn(board, TeamColor.White, (0, 1));
            Assert.True(
                pawn.TryRegularMove((0, 2), out result),
                result.Message);

            Assert.Equal((0, 3), pawn.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: pawn.Position,
                    piece: out Piece? mystery_piece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(pawn, mystery_piece);
        }

        [Fact]
        public void BlackPawn_CanMoveForwardTwiceFromSecondToLastRow()
        {
            MoveResult result;
            GameError error;
            ChessBoard board = new ChessBoard(5, 5);

            var pawn = new Pawn(board, TeamColor.Black, (0, 4));
            Assert.True(
                pawn.TryRegularMove((0, -2), out result),
                result.Message);

            Assert.Equal((0, 2), pawn.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: pawn.Position,
                    piece: out Piece? mystery_piece,
                    error: out error),
                GameErrorExtensions.ToString(error)); 
            Assert.Equal(pawn, mystery_piece);
        }



        [Fact]
        public void WhitePawn_IsBlockedByOtherPiece()
        {
            ChessBoard board = new ChessBoard(5, 5);
            var pawn = new Pawn(board, TeamColor.White, (0, 1));
            var other_pawn = new Pawn(board, TeamColor.White, (0, 2));

            Assert.False(pawn.TryRegularMove(moveDelta: (0, 1), out _));
            Assert.Equal(pawn.Position, (0, 1));
            Assert.Equal(other_pawn.Position, (0, 2));
        }

        [Fact]
        public void BlackPawn_IsBlockedByOtherPiece()
        {
            ChessBoard board = new ChessBoard(5, 5);
            var pawn = new Pawn(board, TeamColor.Black, (0, 4));
            var other_pawn = new Pawn(board, TeamColor.White, (0, 3));

            Assert.False(pawn.TryRegularMove(moveDelta: (0, 1), out _));
            Assert.Equal(pawn.Position, (0, 4));
            Assert.Equal(other_pawn.Position, (0, 3));
        }

        [Fact]
        public void WhitePawn_CanTakePiece()
        {
            MoveResult result;
            GameError error;
            Piece? mysteryPiece;
            ChessBoard board = new ChessBoard(2, 3);
            var pawn = new Pawn(board, TeamColor.White, (0, 0));
            new Pawn(board, TeamColor.Black, (1, 1));
            new Pawn(board, TeamColor.Black, (0, 2));

            Assert.True(
                pawn.TryRegularMove(
                    moveDelta: (1, 1),
                    out result),
                result.Message);
            Assert.Equal((1, 1), pawn.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: pawn.Position,
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(pawn, mysteryPiece);

            Assert.True(
                pawn.TryRegularMove(
                    moveDelta: (-1, 1),
                    out result),
                result.Message);
            Assert.Equal((0, 2), pawn.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: pawn.Position,
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(pawn, mysteryPiece);
        }

        [Fact]
        public void BlackPawn_CanTakePiece()
        {
            MoveResult result;
            GameError error;
            Piece? mysteryPiece;
            ChessBoard board = new ChessBoard(2, 3);
            var pawn = new Pawn(board, TeamColor.Black, (0, 2));
            new Pawn(board, TeamColor.White, (1, 1));
            new Pawn(board, TeamColor.White, (0, 0));

            Assert.True(
                pawn.TryRegularMove(
                    moveDelta: (1, -1),
                    out result),
                result.Message);
            Assert.Equal((1, 1), pawn.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: pawn.Position,
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error)); 
            Assert.Equal(pawn, mysteryPiece);

            Assert.True(
                pawn.TryRegularMove(
                    moveDelta: (-1, -1),
                    out result),
                result.Message);
            Assert.Equal((0, 0), pawn.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: pawn.Position,
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(pawn, mysteryPiece);
        }

        [Fact]
        public void WhitePawn_CannotTakeOwnPiece()
        {
            MoveResult result;
            GameError error;
            ChessBoard board = new ChessBoard(2, 2);
            var pawn = new Pawn(board, TeamColor.White, (0, 0));
            var other_pawn = new Pawn(board, TeamColor.White, (1, 1));

            Assert.False(
                pawn.TryRegularMove(
                    moveDelta: (1, 1),
                    out result),
                result.Message);
            Assert.Equal((0, 0), pawn.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (1,1),
                    piece: out Piece? mystery_piece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(other_pawn, mystery_piece);
        }

        [Fact]
        public void BlackPawn_CannotTakeOwnPiece()
        {
            MoveResult result;
            GameError error;
            ChessBoard board = new ChessBoard(2, 2);
            var pawn = new Pawn(board, TeamColor.Black, (0, 1));
            var other_pawn = new Pawn(board, TeamColor.Black, (1, 0));

            Assert.False(
                pawn.TryRegularMove(
                    moveDelta: (1, -1),
                    out result
                ), 
            result.Message);
            Assert.Equal((0, 1), pawn.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (1, 0),
                    piece: out Piece? mystery_piece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(other_pawn, mystery_piece);
        }
    }
}
