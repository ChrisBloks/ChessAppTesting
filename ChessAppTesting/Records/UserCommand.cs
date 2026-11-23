using ChessAppTesting.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessAppTesting.Tools.Utils;

namespace ChessAppTesting.Records
{
    

    abstract record UserCommand
    {
        public sealed record Quit() : UserCommand;
        public sealed record Move((int x, int y) From, (int x, int y) To) : UserCommand
        {
            public override string ToString()
            {
                TryNumberToCol(From.x, out char from_x);
                TryNumberToCol(To.x, out char to_x);
                return $"Move {from_x}{From.y} to {to_x}{To.y}";
            }
        }
        public sealed record Invalid(string Message) : UserCommand;


    }
}
