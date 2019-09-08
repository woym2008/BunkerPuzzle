using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    public abstract class InputTile : BaseGrid
    {
        BattlefieldModule _battlefield;
        BattlefieldInputModule _inputModule;
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

        public override void PressGrid()
        {
            base.PressGrid();
        }

        void OnInputClick(Vector3 clickpos)
        {
            if (CheckInTile(clickpos))
            {
                OnClick();
            }
        }

        virtual protected void OnClick()
        {

        }

        private bool CheckInTile(Vector3 pos)
        {
            Vector2 size = Constant.TileSize;
            var max_x = _zeropos.x + size.x;
            var max_y = _zeropos.y + size.y;

            var min_x = _zeropos.y - size.x;
            var min_y = _zeropos.y - size.y;

            if (pos.x < max_x && pos.x > min_x && pos.y < max_y && pos.y > min_y)
            {
                return true;
            }

            return false;
        }
    }
}

