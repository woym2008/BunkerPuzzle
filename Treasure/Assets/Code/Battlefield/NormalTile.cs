using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class NormalTile : BaseGrid
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

