using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunker.Game
{
    public class PorterStartTile : BaseTile
    {
        override protected int TileSize
        {
            get { return 1; }
        }
        public override void Init()
        {
            base.Init();
        }

        public override void UpdateGrid(Grid grid)
        {
            base.UpdateGrid(grid);
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

