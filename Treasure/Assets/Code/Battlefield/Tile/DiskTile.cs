using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class DiskTile : BaseTile
    {
        override protected int TileSize
        {
            get { return 2; }
        }

        public override void Init()
        {
            base.Init();
        }

        public override void UpdateGrid(Grid grid)
        {
            base.UpdateGrid(grid);
        }

        public override bool CanElimination()
        {
            return false;
        }

        public override string GetGridType()
        {
            return this.GetType().ToString();
        }

        public override bool CanWalk()
        {
            return true;
        }
    }
}

