using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class WedgeRightTile : BaseGrid
    {
        public override void Init()
        {
            base.Init();
        }

        public override bool CanMove()
        {
            return base.CanMove();
        }

        public override void UpdateGrid(int x, int y)
        {
            base.UpdateGrid(x, y);
        }

        public override bool CanElimination()
        {
            return true;
        }

        public override string GetGridType()
        {
            return this.GetType().ToString();
        }

        public override bool CanEliminationByOther(string gridtype, int direct)
        {
            /*
            if (gridtype == "Bunker.Game.WedgeLeftTile" && direct == 2)
            {
                return true;
            }
            return false;
            */
            return base.CanEliminationByOther(gridtype,direct);
        }
    }
}
