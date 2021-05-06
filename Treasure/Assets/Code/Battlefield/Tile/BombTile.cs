using UnityEngine;
using System.Collections;
using Bunker.Game;
using Bunker.Module;
using System.Collections.Generic;

namespace Bunker.Game
{
    public class BombTile : InputTile
    {
        bool isBroken = false;

        public override void Init()
        {
            base.Init();
            isBroken = false;
        }

        protected override int TileSize
        {
            get { return 1; }
        }

        public override bool CanWalk()
        {
            return false;
        }

        protected override void OnClick()
        {
            base.OnClick();

            if (CanClick())
            {
                Debug.Log("BombTile OnClick");

                Bomb();
            }
        }

        protected void Bomb()
        {
            isBroken = true;

            var collettiles = CollectBombTile();
            //1 消失
            var bcm = ModuleManager.getInstance.GetModule<BattleControllerModule>();

            bcm.UseController<GridFieldController_DestroyTile>(collettiles);


            //2 爆炸
            //VFXManager.getInstance.VFX_RedSPR(GetSelfWorldPos(), dest);
        }

        void AddToList(Bunker.Game.Grid g, ref List<BaseTile> tiles)
        {
            if (g == null || g.AttachTile == null)
            {
                return;
            }
            foreach (var t in tiles)
            {
                if (t == g.AttachTile)
                {
                    return;
                }
                if (!g.AttachTile.CanBreak())
                {
                    return;
                }
            }

            tiles.Add(g.AttachTile);
        }
        //
        virtual protected BaseTile[] CollectBombTile()
        {
            List<BaseTile> kist = new List<BaseTile>();
            kist.Add(this);

            var upGrid = this.ParentGrid?.Up;
            AddToList(upGrid, ref kist);
            var downGrid = this.ParentGrid?.Down;
            AddToList(downGrid, ref kist);
            var leftGrid = this.ParentGrid?.Left;
            AddToList(leftGrid, ref kist);
            var rightGrid = this.ParentGrid?.Right;
            AddToList(rightGrid, ref kist);

            return kist.ToArray();
        }

        public override bool CanBreak()
        {
            return !isBroken;
        }

        public override BaseTile Break()
        {
            if(isBroken)
            {
                return base.Break();
            }


            return this;
        }

        public override void OnBreakon()
        {
            if(!isBroken)
            {
                Bomb();
            }
            else
            {
                GridField.RemoveTile(this.ParentGrid);
            }
        }
    }
}

