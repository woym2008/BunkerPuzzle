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

        void MoveTo(int x, int y, float movetime);
        void CopyMoveTo(int startX, int startY, int endX, int endY, float movetime);

        void UpdateGrid(int x, int y);

        string GetGridType();

        bool CanElimination();

        void Elimination();

        void Delete();

        //void SetPos(int x, int y);
    }
}

