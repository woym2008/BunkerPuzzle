using UnityEngine;
using System.Collections;
using Bunker.Module;
using System.Collections.Generic;

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

        TileEffectEmitter _emitter;

        string effecttype = "DisturbEffect";

        public override BaseTile Break()
        {
            return base.Break();
        }

        public override void OnBreakon()
        {
            base.OnBreakon();

            _emitter?.CloseEmitter();
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

        public override void Init(string additionalData)
        {
            base.Init(additionalData);
        }

        public override void OnStart()
        {
            base.OnStart();

            var battlemodule = ModuleManager.getInstance.GetModule<BattleControllerModule>();

            var filed = battlemodule.Field;

            _emitter = new TileEffectEmitter();

            List<BaseTile> tiles = new List<BaseTile>();
            if (filed.GetHorizontalLine(this.ParentGrid.RowID, out BaseTile[] htiles))
            {
                for(int i=0;i< htiles.Length; ++i)
                {
                    tiles.Add(htiles[i]);
                }

            }

            if (filed.GetVerticalLine(this.ParentGrid.ColID, out BaseTile[] vtiles))
            {
                for (int i = 0; i < vtiles.Length; ++i)
                {
                    bool findsame = false;
                    foreach(var t in tiles)
                    {
                        if(t == vtiles[i])
                        {
                            findsame = true;
                            break;
                        }
                    }

                    if(!findsame)
                    {
                        tiles.Add(vtiles[i]);
                    }
                }

            }
            _emitter.StartEmitter(effecttype, tiles.ToArray());
        }
    }
}

