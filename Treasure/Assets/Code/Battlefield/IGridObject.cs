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

        void UpdateGrid(int x, int y);

        //void SetPos(int x, int y);
    }
}

