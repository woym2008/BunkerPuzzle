using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    /// <summary>
    /// 锁定块 这个块出现后
    /// </summary>
    public class LockTile : BaseTile
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

        public override LockState GetLockState()
        {
            return LockState.LockAll;
        }

        override public bool CanBreak()
        {
            return true;
        }
    }
}

