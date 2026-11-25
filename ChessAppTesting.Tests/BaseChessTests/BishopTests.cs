using ChessAppTesting.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using ChessAppTesting.Pieces;
using ChessAppTesting.Records;
using ChessAppTesting.GUI;
using static ChessAppTesting.Enums.GameErrorExtensions;

namespace ChessAppTesting.Tests.BaseChessTests
{
    public class BishopTests
    {
        [Fact]
        public void Bishop_IsAddedToBoard()
        {
            ChessBoard board = new ChessBoard(2, 2);
            var bishop = new Bishop(
                board: board,
                teamColor: TeamColor.White,
                position: (0, 0));

            Assert.True(
                board.TryGetPieceAtPosition(
                    (0, 0),
                    out Piece? mystery_piece,
                    out GameError error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mystery_piece);
        }

        [Fact]

        public void Bishop_CanMoveDiagonallyInAllDirections()
        {
            GameError error;
            MoveDelta moveDelta;
            Piece? mysteryPiece;

            ChessBoard board = new ChessBoard(5, 5);
            var bishop = new Bishop(
                board: board,
                teamColor: TeamColor.White,
                position: (0,0));

            // First move
            moveDelta = (3, 3);
            bishop.TryRegularMove(
                moveDelta: moveDelta,
                result: out _);

            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);

            // Second move
            moveDelta = (1, -1);
            bishop.TryRegularMove(
                 moveDelta: moveDelta,
                 result: out _);

            Assert.Equal((4, 2), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (4, 2),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);

            // Third move
            moveDelta = (-2, -2);
            bishop.TryRegularMove(
                 moveDelta: moveDelta,
                 result: out _);

            Assert.Equal((2, 0), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (2, 0),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);

            // Fourth move
            moveDelta = (-2, 2);
            bishop.TryRegularMove(
                 moveDelta: moveDelta,
                 result: out _);

            Assert.Equal((0, 2), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (0, 2),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);
        }

        [Fact]
        public void Bishop_CannotMoveOrthogonallyInAnyDirection()
        {
            MoveDelta moveDelta;

            ChessBoard board = new ChessBoard(5, 5);
            var bishop = new Bishop(
                board: board,
                teamColor: TeamColor.White,
                position: (2, 2));

            // First move
            moveDelta = (1, 0);
            Assert.False(bishop.TryRegularMove(
                 moveDelta: moveDelta,
                 result: out _));
            Assert.Equal((2, 2), bishop.Position);

            moveDelta = (0, -1);
            Assert.False(bishop.TryRegularMove(
                 moveDelta: moveDelta,
                 result: out _));
            Assert.Equal((2, 2), bishop.Position);

            moveDelta = (-1, 0);
            Assert.False(bishop.TryRegularMove(
                 moveDelta: moveDelta,
                 result: out _));
            Assert.Equal((2, 2), bishop.Position);

            moveDelta = (0, 1);
            Assert.False(bishop.TryRegularMove(
                 moveDelta: moveDelta,
                 result: out _));
            Assert.Equal((2, 2), bishop.Position);

        }

        [Fact]
        public void Bishop_IsBlockedByOtherPieces()
        {
            GameError error;
            MoveDelta moveDelta;
            Piece? mysteryPiece;

            ChessBoard board = new ChessBoard(7, 7);
            var bishop = new Bishop(
                board: board,
                teamColor: TeamColor.White,
                position: (3, 3));

            // Add pawns at (4,4), (5,1), (1, 1), (2,4)
            (int x, int y)[] pawnPositions = [(4, 4), (5, 1), (1, 1), (2, 4)];
            for(int i = 0; i < 4; i ++)
            { 
                new Pawn(
                    board: board,
                    teamColor: i % 2 == 0 ? TeamColor.White : TeamColor.Black,
                    position: pawnPositions[i]);
                Assert.True(
                    board.TryGetPieceAtPosition(
                        position: pawnPositions[i],
                        piece: out mysteryPiece,
                        error: out error),
                    GameErrorExtensions.ToString(error));
            }

            ConsoleGui.Instance.DisplayBoard(board);
            

            // First move
            moveDelta = (2, 2);
            Assert.False(bishop.TryRegularMove(
                moveDelta: moveDelta,
                result: out _));
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);

            // second move
            moveDelta = (3, -3);
            Assert.False(bishop.TryRegularMove(
                moveDelta: moveDelta,
                result: out _));
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);

            // third move
            moveDelta = (-3, -3);
            Assert.False(bishop.TryRegularMove(
                moveDelta: moveDelta,
                result: out _));
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);

            // Fourth move
            moveDelta = (-3, 3);
            Assert.False(bishop.TryRegularMove(
                moveDelta: moveDelta,
                result: out _));
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);
        }

        [Fact]
        public void Bishop_CanTakeEnemyPieces()
        {
            GameError error;
            MoveResult result;
            MoveDelta moveDelta;
            Piece? mysteryPiece;

            ChessBoard board = new ChessBoard(5, 5);
            var bishop = new Bishop(
                board: board,
                teamColor: TeamColor.White,
                position: (0, 0));

            (int x, int y)[] pawnPositions = [(3, 3), (4, 2)];
            for (int i = 0; i < 2; i++)
            {
                new Pawn(
                    board: board,
                    teamColor: TeamColor.Black,
                    position: pawnPositions[i]);
                Assert.True(
                    board.TryGetPieceAtPosition(
                        position: pawnPositions[i],
                        piece: out mysteryPiece,
                        error: out error),
                    GameErrorExtensions.ToString(error));
            }
            Assert.Equal(3, board.Pieces.Count);

            moveDelta = (3, 3);
            Assert.True(
                bishop.TryRegularMove(
                    moveDelta: moveDelta,
                    result: out result),
                result.Message);
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);
            Assert.Equal(2, board.Pieces.Count);

            moveDelta = (1, -1);
            Assert.True(
                bishop.TryRegularMove(
                    moveDelta: moveDelta,
                    result: out result),
                result.Message);
            Assert.Equal((4, 2), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (4, 2),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);
            Assert.Single(board.Pieces);

        }

        [Fact]
        public void Bishop_CannotTakeOwnPieces()
        {
            GameError error;
            MoveResult result;
            MoveDelta moveDelta;
            Piece? mysteryPiece;

            ChessBoard board = new ChessBoard(5, 5);
            var bishop = new Bishop(
                board: board,
                teamColor: TeamColor.White,
                position: (3, 3));

            (int x, int y)[] pawnPositions = [(0, 0), (4, 2)];
            for (int i = 0; i < 2; i++)
            {
                new Pawn(
                    board: board,
                    teamColor: TeamColor.White,
                    position: pawnPositions[i]);
                Assert.True(
                    board.TryGetPieceAtPosition(
                        position: pawnPositions[i],
                        piece: out mysteryPiece,
                        error: out error),
                    GameErrorExtensions.ToString(error));
            }
            Assert.Equal(3, board.Pieces.Count());

            moveDelta = (-3, -3);
            Assert.False(
                bishop.TryRegularMove(
                    moveDelta: moveDelta,
                    result: out result),
                result.Message);
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);
            Assert.Equal(3, board.Pieces.Count);

            moveDelta = (1, -1);
            Assert.False(
                bishop.TryRegularMove(
                    moveDelta: moveDelta,
                    result: out result),
                result.Message);
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                GameErrorExtensions.ToString(error));
            Assert.Equal(bishop, mysteryPiece);
            Assert.Equal(3, board.Pieces.Count);

        }
    }
}
