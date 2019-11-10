using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;

namespace Bunker.Game
{
    public abstract class InputTile : BaseGrid
    {
        protected BattlefieldModule _battlefield;
        protected BattlefieldInputModule _inputModule;

        public static int sInputCount = 0;

        public override void Init()
        {
            base.Init();

            //get battlemodule
            _battlefield = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            _inputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();

            _inputModule.onPressClick += OnInputClick;
            //
        }

        public override bool CanMove()
        {
            return base.CanMove();
        }

        public override void UpdateGrid(int x, int y)
        {
            base.UpdateGrid(x, y);
        }

        void OnInputClick(Vector3 clickpos, InputState state, Action<object> onClick)
        {
            if (CheckInTile(clickpos))
            {
                if (state == InputState.Normal)
                {
                    OnClick();
                }

                //onClick?.Invoke(string.Format("x:{0},y:{1}",_x,_y));
                onClick?.Invoke(this);
            }
        }

        virtual protected void OnClick()
        {
            InputTile.sInputCount++;
        }

        protected bool CanClick()
        {
            return InputTile.sInputCount > 1 ? false : true;
        }

        private bool CheckInTile(Vector3 pos)
        {
            Vector2 size = Constant.TileSize;

            var max_x = _object.transform.position.x + size.x * 0.5f;
            var max_y = _object.transform.position.y + size.y * 0.5f;

            var min_x = _object.transform.position.x - size.x * 0.5f;
            var min_y = _object.transform.position.y - size.y * 0.5f;

            //modify by wwh
            // if there has BoxCollider2D ,the bounding test first!
            var col = _object.GetComponent<BoxCollider2D>();
            if (col != null)
            {
                return col.OverlapPoint(new Vector2(pos.x, pos.y));
            }
            else if (pos.x < max_x && pos.x > min_x && pos.y < max_y && pos.y > min_y)
            {
                return true;
            }

            return false;
        }
    }
}

