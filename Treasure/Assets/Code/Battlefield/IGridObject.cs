using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public interface IGridObject
    {
        int X
        {
            get;
        }
        int Y
        {
            get;
        }

        bool CanMove();

        void PressGrid();

        void SetPos(int x, int y);
    }
}

