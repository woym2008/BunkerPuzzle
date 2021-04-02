﻿using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class GemTile : BaseTile
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
            return false;
        }

        public override string GetGridType()
        {
            return this.GetType().ToString();
        }
    }
}
