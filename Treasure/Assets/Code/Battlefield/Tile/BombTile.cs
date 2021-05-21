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

        bool isTimeBomb = false;
        int _timerValue = 1;

        SpriteRenderer _timeBoard;

        protected override void OnLoadRes()
        {
            base.OnLoadRes();
        }

        public override void Init(string additionalData)
        {
            base.Init(additionalData);
            isBroken = false;

            if(additionalData != null && additionalData != "")
            {
                Debug.LogError(additionalData);
                var datas = additionalData.Split(';');

                if (datas != null && datas.Length == 2)
                {
                    isTimeBomb = bool.Parse(datas[0]);
                    _timerValue = int.Parse(datas[1]);
                }

                var turnmodule = ModuleManager.getInstance.GetModule<BattleTurnsModule>();
                turnmodule.Notifications += OnNextTurn;


                if (isTimeBomb)
                {
                    var timeBoardobj = _object.transform.Find("TimeBoard").gameObject;

                    _timeBoard = timeBoardobj.GetComponent<SpriteRenderer>();

                    ShowTimeValue(_timerValue);
                }
            }
            
        }

        void OnNextTurn(CTurn turn)
        {
            if(turn.GetType() == typeof(PlayerTurn))
            {
                PassTurn();
            }
        }

        void PassTurn()
        {
            if(!isBroken && isTimeBomb)
            {
                _timerValue--;
                if(_timerValue <= 0)
                {
                    Bomb();
                }
                else
                {
                    ShowTimeValue(_timerValue);
                }
            }
        }

        void ShowTimeValue(int value)
        {
            Debug.Log("Texture/Tile/BombTile/bombtime_" + _timerValue);
            //var tex = Resources.Load<Texture2D>("Texture/Tile/BombTile/bombtime_" + _timerValue);
            //Sprite sp = Sprite.Create(tex, _timeBoard.sprite.textureRect, new Vector2(0.5f, 0.5f));
            //var obj = Resources.LoadAll("Texture/Tile/BombTile/bombtime_" + _timerValue);
            var sps = Resources.LoadAll<Sprite>("Texture/Tile/BombTile/bombtime");
            try
            {
                var name = "bombtime_" + _timerValue;
                foreach (var sp in sps)
                {
                    Debug.Log(sp.name);
                    if (sp.name == name)
                    {
                        Debug.Log(_timeBoard.sprite);
                        _timeBoard.sprite = GameObject.Instantiate(sp) as Sprite;
                        break;
                    }
                }
                //Sprite sp = GameObject.Instantiate(obj) as Sprite;

            }
            catch
            {

            }
        }

        public override void UpdateSortingOrder()
        {
            //add by wwh
            if (_timeBoard != null)
            {
                var kid = _object.transform.GetChild(0);
                var sr = kid.GetComponent<SpriteRenderer>();
                _timeBoard.sortingOrder = sr.sortingOrder + 1;
            }
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

