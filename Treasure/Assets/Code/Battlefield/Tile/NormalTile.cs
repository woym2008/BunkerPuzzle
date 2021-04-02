using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class NormalTile : BaseTile
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

        public override string GetGridType()
        {
            return this.GetType().ToString();
        }

    }
}

