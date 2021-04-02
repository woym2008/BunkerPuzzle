using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class WedgeRightTile : BaseTile
    {
        public override void Init()
        {
            base.Init();
        }

        public override bool CanMove()
        {
            return base.CanMove();
        }

        public override void UpdateGrid(Grid grid)
        {
            base.UpdateGrid(grid);
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
