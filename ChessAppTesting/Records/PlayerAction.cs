using ChessAppTesting.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessAppTesting.Tools.Utils;

namespace ChessAppTesting.Records
{
    

    public abstract record PlayerAction
    {
        public sealed record Quit() : PlayerAction;
        public sealed record MoveAction((int x, int y) From, (int x, int y) To) : PlayerAction
        {
            public override string ToString()
            {
                TryNumberToCol(From.x, out char from_x);
                TryNumberToCol(To.x, out char to_x);
                return $"Move {from_x}{From.y+1} to {to_x}{To.y+1}";
            }
        }

        public sealed record TakeAction((int x, int y) From, (int x, int y) To) : PlayerAction
        {
            public override string ToString()
            {
                TryNumberToCol(From.x, out char from_x);
                TryNumberToCol(To.x, out char to_x);
                return $"Take {from_x}{From.y + 1} to {to_x}{To.y + 1}";
            }
        }
        public sealed record InvalidAction(string Message) : PlayerAction;


    }
}
