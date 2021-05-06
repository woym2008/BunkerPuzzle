using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;

namespace Bunker.Game
{
    public abstract class InputTile : BaseTile
    {
        protected BattleControllerModule _battlefield;
        protected BattlefieldInputModule _inputModule;

        public static int sInputCount = 0;

        private InputController _inputController;

        private Material _highLightMat;

        string _highColorStr = "_HighColor";
        int _highlightID;
        Color HighColor = new Color(0.6f, 0.6f, 0.6f, 0);
        Color NormalColor = new Color(0, 0, 0, 0);

        public override void Init()
        {
            base.Init();

            //get battlemodule
            _battlefield = ModuleManager.getInstance.GetModule<BattleControllerModule>();
            _inputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();

            //_inputModule.onPressClick += OnInputClick;

            _highLightMat = this._object.GetComponentInChildren<SpriteRenderer>().material;
            _highlightID = Shader.PropertyToID(_highColorStr);
            _highLightMat.SetColor(_highlightID, HighColor);
            //

            _inputController = _object.GetComponent<InputController>();
            if(_inputController == null)
            {
                _inputController = _object.AddComponent<InputController>();
            }
            _inputController.onPressClick = OnInputClick;
            _inputController.onTouchEnter = OnTouchEnter;
            _inputController.onTouchExit = OnTouchExit;

            ToNormalLight();
        }

        public override void OnDestroy()
        {
            Debug.LogWarning("destroy InputTile");
            //_inputModule.onPressClick -= OnInputClick;
            if(_inputController != null)
            {
                _inputController.onPressClick = null;
                _inputController.onTouchEnter = null;
                _inputController.onTouchExit = null;
            }

            base.OnDestroy();
        }

        public override void UpdateGrid(Grid grid)
        {
            base.UpdateGrid(grid);
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

        void OnTouchEnter()
        {
            _inputController.StartCoroutine(ToHighLight());
        }
        void OnTouchExit()
        {
            _inputController.StopAllCoroutines();
            ToNormalLight();
        }
        IEnumerator ToHighLight()
        {
            float maxt = 0.2f;
            float t = 0;
            while(t < maxt)
            {
                t += Time.deltaTime;

                if(_highLightMat != null)
                {
                    _highLightMat.SetColor(_highlightID,Color.Lerp(NormalColor,HighColor, Mathf.Clamp(t / maxt,0,1)));
                }
                yield return 0;
            }
        }

        void ToNormalLight()
        {
            _highLightMat.SetColor(_highlightID, NormalColor);
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

