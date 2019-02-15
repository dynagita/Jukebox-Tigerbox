using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Objects
{
    public static class Enums
    {
        public enum TigerActions
        {
            Undefined,
            MoveForward,
            MoveBackward,
            MoveSongListDown,
            MoveSongListUp,
            TakeOutFirstSelectedSong,
            IncreaseCredit,
            SelectSong,
            ShowLetterPanel,
            HideLetterPanel,
            MoveEntirePage,
            Play,
            Stop,
            IncreaseVolume,
            DecreaseVolume,
            SetFullScreen
        }
    }
}
