﻿using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class NormalTile : BaseTile
    {
        override protected int TileSize {
            get { return 1; }
        }
        public override void Init(string additionalData)
        {
            base.Init(additionalData);
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

