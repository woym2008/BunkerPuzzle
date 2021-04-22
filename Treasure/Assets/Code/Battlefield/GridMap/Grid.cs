using UnityEngine;
using UnityEditor;

namespace Bunker.Game
{
    public class Grid
    {
        public int RowID { get; set; }
        public int ColID { get; set; }

        public Grid Up { get; set; }
        public Grid Down { get; set; }
        public Grid Left { get; set; }
        public Grid Right { get; set; }

        public BaseTile AttachTile;

        public Grid(int col, int row)
        {
            ColID = col;
            RowID = row;
        }

        public Grid GetColOffsetGrid(int offsetCol)
        {
            Grid ret = this;
            int offsetValue = Mathf.Abs(offsetCol);
            for(int i = 0;i < offsetValue; ++i)
            {
                if(offsetCol > 0)
                {
                    ret = ret.Right;
                }
                else
                {
                    ret = ret.Left;
                }
            }

            return ret;
        }

        public Grid GetRowOffsetGrid(int offsetRow)
        {
            Grid ret = this;
            int offsetValue = Mathf.Abs(offsetRow);
            for (int i = 0; i < offsetValue; ++i)
            {
                if (offsetRow > 0)
                {
                    ret = ret.Down;
                }
                else
                {
                    ret = ret.Up;
                }
            }

            return ret;
        }

        //add by wwh 
        public static bool Equal(Grid lhs, Grid rhs)
        {
            bool status = false;
            if (lhs.RowID == rhs.RowID && lhs.ColID == rhs.ColID)
            {
                status = true;
            }
            return status;
        }
    }
}
