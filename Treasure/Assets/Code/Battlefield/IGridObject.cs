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

        string GetGridType();

        bool CanElimination();

        void Elimination();

        void Delete();

        //void SetPos(int x, int y);
    }
}

