using System;
using System.Collections.Generic;
using System.Text;
using PlayerAction = ChessAppTesting.Records.PlayerAction;

namespace ChessAppTesting.GUI
{
    public interface IGUI
    {
        PlayerAction Display(ChessBoard board);
    }
}
