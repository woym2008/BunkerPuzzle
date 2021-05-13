using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class WedgeRightTile : BaseTile
    {
        override protected int TileSize {
            get { return 2; }
        }
        public override void Init(string additionalData)
        {
            base.Init(additionalData);
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
            
            if (gridtype == "Bunker.Game.WedgeLeftTile" && direct == 2)
            {
                return true;
            }
            return false;
            
            //return base.CanEliminationByOther(gridtype,direct);
        }
    }
}
