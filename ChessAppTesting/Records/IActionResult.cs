using ChessAppTesting.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChessAppTesting.Records
{
    internal interface IActionResult
    {
        GameError Error { get; }
        bool IsLegal { get; }
        string Message { get ; }
    }
}
