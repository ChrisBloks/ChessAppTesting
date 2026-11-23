using System;
using System.Collections.Generic;
using System.Text;
using UserCommand = ChessAppTesting.Records.UserCommand;

namespace ChessAppTesting.GUI
{
    internal interface IGUI
    {
        UserCommand Display(ChessBoard board);
    }
}
