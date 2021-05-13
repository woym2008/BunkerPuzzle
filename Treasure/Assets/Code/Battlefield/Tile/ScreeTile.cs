using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    /// <summary>
    /// 碎石堆 这个简单 直接作为挡路的
    /// </summary>
    public class ScreeTile : BaseTile
    {
        string _hideTileName;
        protected override int TileSize
        {
            get
            {
                return 2;
            }
        }

        public override void Init(string additionalData)
        {
            base.Init(additionalData);

            _hideTileName = additionalData;
        }

        public override BaseTile Break()
        {
            if(_hideTileName == "")
            {
                return base.Break();
            }

            var newTile = GridLoader.CreateTile("NormalTile", this.ParentGrid, _hideTileName);
            return newTile;
        }

        public override void OnBreakon()
        {
            base.OnBreakon();
        }

        public override bool CanBreak()
        {
            return true;
        }

        public override bool CanWalk()
        {
            return false;
        }
    }
}

