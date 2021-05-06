using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    /// <summary>
    /// 碎石堆 这个简单 直接作为挡路的
    /// </summary>
    public class ScreeTile : BaseTile
    {
        protected override int TileSize
        {
            get
            {
                return 2;
            }
        }

        public override BaseTile Break()
        {
            return base.Break();
        }

        public override void OnBreakon()
        {
            base.OnBreakon();
        }

        public override bool CanWalk()
        {
            return false;
        }
    }
}

