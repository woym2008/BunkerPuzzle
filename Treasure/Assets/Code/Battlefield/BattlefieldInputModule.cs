using UnityEngine;
using System.Collections;
using System;
using Bunker.Module;

namespace Bunker.Game
{
    public delegate void InputClickEvent(Vector3 presspos);
    public delegate void InputReleaseEvent();
    public class BattlefieldInputModule : LogicModule
    {
        public event InputClickEvent onPressClick;
        public event InputReleaseEvent onReleaseClick;

        public BattlefieldInputModule() : base(typeof(BattlefieldInputModule).ToString())
        {

        }
        BattlefieldInputModule(string name) : base(name)
        {

        }
        public override void Create()
        {
            base.Create();
        }

        public override void Release()
        {
            base.Release();
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }

        private void Update(float dt)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousepos = Input.mousePosition;
                var pos = Camera.main.ScreenToWorldPoint(mousepos);

                onPressClick?.Invoke(pos);
            }

            if (Input.GetMouseButtonDown(1))
            {
                onReleaseClick?.Invoke();
            }

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                var pos = Camera.main.ScreenToWorldPoint(touch.position);

                if (touch.phase == TouchPhase.Began)
                {
                    onPressClick?.Invoke(pos);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    onReleaseClick?.Invoke();
                }
            }

        }
    }
}

