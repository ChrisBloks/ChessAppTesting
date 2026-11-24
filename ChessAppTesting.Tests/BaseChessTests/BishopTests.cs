using ChessAppTesting.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using ChessAppTesting.Pieces;
using ChessAppTesting.GUI;

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
                    out string error),
                error);
            Assert.Equal(bishop, mystery_piece);
        }

        [Fact]

        public void Bishop_CanMoveDiagonallyInAllDirections()
        {
            string error;
            (int dx, int dy) move;
            Piece? mysteryPiece;

            ChessBoard board = new ChessBoard(5, 5);
            var bishop = new Bishop(
                board: board,
                teamColor: TeamColor.White,
                position: (0,0));

            // First move
            move = (3, 3);
            bishop.TryRegularMove(
                move: move,
                error: out _);

            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);

            // Second move
            move = (1, -1);
            bishop.TryRegularMove(
                 move: move,
                 error: out _);

            Assert.Equal((4, 2), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (4, 2),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);

            // Third move
            move = (-2, -2);
            bishop.TryRegularMove(
                 move: move,
                 error: out _);

            Assert.Equal((2, 0), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (2, 0),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);

            // Fourth move
            move = (-2, 2);
            bishop.TryRegularMove(
                 move: move,
                 error: out _);

            Assert.Equal((0, 2), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (0, 2),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);
        }

        [Fact]
        public void Bishop_CannotMoveOrthogonallyInAnyDirection()
        {
            (int dx, int dy) move;

            ChessBoard board = new ChessBoard(5, 5);
            var bishop = new Bishop(
                board: board,
                teamColor: TeamColor.White,
                position: (2, 2));

            // First move
            move = (1, 0);
            Assert.False(bishop.TryRegularMove(
                move: move,
                error: out _));
            Assert.Equal((2, 2), bishop.Position);

            move = (0, -1);
            Assert.False(bishop.TryRegularMove(
                move: move,
                error: out _));
            Assert.Equal((2, 2), bishop.Position);

            move = (-1, 0);
            Assert.False(bishop.TryRegularMove(
                move: move,
                error: out _));
            Assert.Equal((2, 2), bishop.Position);

            move = (0, 1);
            Assert.False(bishop.TryRegularMove(
                move: move,
                error: out _));
            Assert.Equal((2, 2), bishop.Position);

        }

        [Fact]
        public void Bishop_IsBlockedByOtherPieces()
        {
            string? error;
            (int dx, int dy) move;
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
                    error);
            }

            ConsoleGui.Instance.DisplayBoard(board);
            

            // First move
            move = (2, 2);
            Assert.False(bishop.TryRegularMove(
                move: move,
                error: out _));
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);

            // second move
            move = (3, -3);
            Assert.False(bishop.TryRegularMove(
                move: move,
                error: out _));
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);

            // third move
            move = (-3, -3);
            Assert.False(bishop.TryRegularMove(
                move: move,
                error: out _));
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);

            // Fourth move
            move = (-3, 3);
            Assert.False(bishop.TryRegularMove(
                move: move,
                error: out _));
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);
        }

        [Fact]
        public void Bishop_CanTakeEnemyPieces()
        {
            string? error;
            (int dx, int dy) move;
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
                    error);
            }
            Assert.Equal(3, board.Pieces.Count);

            move = (3, 3);
            Assert.True(
                bishop.TryRegularTake(
                    move: move,
                    out error),
                error);
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);
            Assert.Equal(2, board.Pieces.Count);

            move = (1, -1);
            Assert.True(
                bishop.TryRegularTake(
                    move: move,
                    out error),
                error);
            Assert.Equal((4, 2), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (4, 2),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);
            Assert.Single(board.Pieces);

        }

        [Fact]
        public void Bishop_CannotTakeOwnPieces()
        {
            string? error;
            (int dx, int dy) move;
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
                    error);
            }
            Assert.Equal(3, board.Pieces.Count());

            move = (-3, -3);
            Assert.False(
                bishop.TryRegularTake(
                    move: move,
                    out error),
                error);
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);
            Assert.Equal(3, board.Pieces.Count);

            move = (1, -1);
            Assert.False(
                bishop.TryRegularMove(
                    move: move,
                    out error),
                error);
            Assert.Equal((3, 3), bishop.Position);
            Assert.True(
                board.TryGetPieceAtPosition(
                    position: (3, 3),
                    piece: out mysteryPiece,
                    error: out error),
                error);
            Assert.Equal(bishop, mysteryPiece);
            Assert.Equal(3, board.Pieces.Count);

        }
    }
}
