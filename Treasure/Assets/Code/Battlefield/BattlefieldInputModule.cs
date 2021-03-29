using UnityEngine;
using System.Collections;
using System;
using Bunker.Module;

namespace Bunker.Game
{
    //输入系统状态，点击事件发出的时候带出状态，每个格子自己处理点过来的不同状态的点击事件
    public enum InputState
    {
        Normal,
        UseItem,

    }
    public delegate void InputClickEvent(Vector3 presspos, InputState state, Action<object> onClick);
    public delegate void InputReleaseEvent();
    public delegate void TouchEnterEvent();
    public delegate void TouchExitEvent();

    public delegate void ClickedTileEvent(IGridObject grid);
    public class BattlefieldInputModule : LogicModule
    {
        public event InputClickEvent onPressClick;
        public event InputReleaseEvent onReleaseClick;

        public event ClickedTileEvent onClickedTile;

        Camera _inputcam;

        InputState _state = InputState.Normal;

        public bool locked = false;

        bool _pause = false;

        const string _layername = "Tile";
        int _layerids;

        InputController _lastInputTile = null;

        public BattlefieldInputModule() : base(typeof(BattlefieldInputModule).ToString())
        {
            _layerids = 1 << LayerMask.NameToLayer(_layername);
            _lastInputTile = null;
        }
        BattlefieldInputModule(string name) : base(name)
        {
            _layerids = 1 << LayerMask.NameToLayer(_layername);
            _lastInputTile = null;
        }
        public void Rest()
        {
            //if(onPressClick != null)
            //{
            //    Delegate[] dels = onPressClick.GetInvocationList();
            //    foreach (Delegate del in dels)
            //    {
            //        onPressClick -= del as InputClickEvent;
            //    }
            //}

            //if(onReleaseClick != null)
            //{
            //    Delegate[] dels_reles = onReleaseClick.GetInvocationList();
            //    foreach (Delegate del in dels_reles)
            //    {
            //        onReleaseClick -= del as InputReleaseEvent;
            //    }
            //}

            //if (onClickedTile != null)
            //{
            //    Delegate[] dels_clicked = onClickedTile.GetInvocationList();
            //    foreach (Delegate del in dels_clicked)
            //    {
            //        onClickedTile -= del as ClickedTileEvent;
            //    }

            //}

            _lastInputTile = null;
        }
        public override void Create()
        {
            base.Create();
        }

        public override void Release()
        {
            base.Release();
        }

        public override void OnStart(params object[] data)
        {
            base.OnStart();

            _state = InputState.Normal;

            _inputcam = null;
        }

        public override void OnStop()
        {
            base.OnStop();

            _inputcam = null;
        }

        public void Pause(bool enable)
        {
            _pause = enable;
        }

        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }

        public void Update(float dt)
        {
            if(_inputcam == null)
            {
                _inputcam = ModuleManager.getInstance.GetModule<BattlefieldCameraModule>().GameCam;
            }
            if(_pause)
            {
                return;
            }
            if (!locked)
            {
                var curmousepos = Input.mousePosition;
                var curpos = _inputcam.ScreenToWorldPoint(curmousepos);
                RaycastHit2D hit = 
                    Physics2D.Raycast(
                        curpos,                         
                        Vector2.zero,
                        100.0f,
                        _layerids
                        );
                
                if (hit.collider != null && 
                    hit.collider.gameObject.GetComponent<InputController>() != null)
                {
                    var ctrl = hit.collider.gameObject.GetComponent<InputController>();
                    if (ctrl != _lastInputTile)
                    {
                        if(_lastInputTile != null)
                            _lastInputTile?.onTouchExit?.Invoke();
                    }
                    ctrl.onTouchEnter?.Invoke();

                    _lastInputTile = ctrl;
                }
                else
                {
                    if(_lastInputTile != null)
                    {
                        _lastInputTile?.onTouchExit?.Invoke();
                        _lastInputTile = null;
                    }
                    
                }

                if(_lastInputTile != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector3 mousepos = Input.mousePosition;
                        var pos = _inputcam.ScreenToWorldPoint(mousepos);

                        //onPressClick?.Invoke(pos, _state, OnClickObject);
                        if(_lastInputTile != null)
                            _lastInputTile.onPressClick.Invoke(pos, _state, OnClickObject);
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        //onReleaseClick?.Invoke();
                        if (_lastInputTile != null)
                            _lastInputTile.onReleaseClick?.Invoke();
                    }

                    if (Input.touchCount > 0)
                    {
                        var touch = Input.GetTouch(0);

                        var pos = Camera.main.ScreenToWorldPoint(touch.position);

                        if (touch.phase == TouchPhase.Began)
                        {
                            //onPressClick?.Invoke(pos, _state, OnClickObject);
                            if (_lastInputTile != null)
                            {
                                _lastInputTile.onTouchEnter?.Invoke();
                                _lastInputTile.onPressClick?.Invoke(pos, _state, OnClickObject);
                            }
                                
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            //onReleaseClick?.Invoke();
                            if (_lastInputTile != null)
                            {
                                _lastInputTile.onTouchExit?.Invoke();
                                _lastInputTile.onReleaseClick?.Invoke();
                            }                                
                        }
                    }
                }
                
            }

            InputTile.sInputCount = 0;
        }

        ///<summary>
        ///点击后点中时的回调
        ///</summary>
        private void OnClickObject(object data)
        {
            if((IGridObject)data != null)
            {
                Debug.Log("find grid");

                onClickedTile?.Invoke((IGridObject)data);
            }
        }

        ///<summary>
        ///设置输入系统状态
        ///</summary>
        public void SetInputState(InputState st)
        {
            _state = st;
        }
    }
}

