using ChessAppTesting.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessAppTesting.Enums.GameErrorExtensions;

namespace ChessAppTesting.Records
{
    public record ActionResult(GameError Error) : IActionResult
    {

        public GameError Error { get; } = Error;

        public bool IsLegal => Error == GameError.None;

        public string Message => GameErrorExtensions.ToString(Error);
    }
}
