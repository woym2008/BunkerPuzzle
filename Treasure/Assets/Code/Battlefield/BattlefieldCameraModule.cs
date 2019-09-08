using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    public class BattlefieldCameraModule : LogicModule
    {
        Camera _gameCamera;

        public Camera GameCam
        {
            get
            {
                if(_gameCamera == null)
                {
                    var cam = GameObject.Find("GameCamera");
                    if (cam != null)
                    {
                        _gameCamera = cam.GetComponent<Camera>();
                    }
                }

                return _gameCamera;
            }
        }

        public BattlefieldCameraModule() : base(typeof(BattlefieldCameraModule).ToString())
        {

        }
        BattlefieldCameraModule(string name) : base(name)
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

            var cam = GameObject.Find("GameCamera");
            if(cam != null)
            {
                _gameCamera = cam.GetComponent<Camera>();
            }

        }

        public override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }
    }
}

