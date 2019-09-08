using UnityEngine;
using System.Collections;
using System;
using Bunker.Module;

namespace Bunker.Game
{
    public class BattlefieldInputModule : LogicModule
    {
        Action PressClick;
        Action ReleaseClick;

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
                PressClick?.Invoke();
            }

            if (Input.GetMouseButtonDown(1))
            {
                ReleaseClick?.Invoke();
            }

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    PressClick?.Invoke();
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    ReleaseClick?.Invoke();
                }
            }

        }
    }
}

