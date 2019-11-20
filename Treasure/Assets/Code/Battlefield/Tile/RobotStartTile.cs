using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunker.Game
{
    public class RobotStartTile : BaseGrid
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

        public override string GetGridType()
        {
            return this.GetType().ToString();
        }
    }
}
