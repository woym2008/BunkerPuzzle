using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    public abstract class InputTile : BaseGrid
    {
        protected BattlefieldModule _battlefield;
        protected BattlefieldInputModule _inputModule;
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
            //var thispos = new Vector3( Y*Constant.TileSize.y  + X * Constant.TileSize.x;
            var max_x = _object.transform.position.x + size.x * 0.5f;
            var max_y = _object.transform.position.y + size.y * 0.5f;

            var min_x = _object.transform.position.x - size.x * 0.5f;
            var min_y = _object.transform.position.y - size.y * 0.5f;

            if (pos.x < max_x && pos.x > min_x && pos.y < max_y && pos.y > min_y)
            {
                return true;
            }

            return false;
        }
    }
}

