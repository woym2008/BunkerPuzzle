using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    public class TitleModule : LogicModule
    {
        MainMenuPanel2D _Panel;
        float _ButtonTimerConter = 0;

        public TitleModule() : base(typeof(TitleModule).ToString())
        {
            
        }
        public TitleModule(string name) :base(name)
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
            _Panel = UIModule.getInstance.Open<MainMenuPanel2D>();
            SoundManager.getInstance.PlayBGM("bgm");
        }

        public override void OnStop()
        {
            UIModule.getInstance.Close<MainMenuPanel2D>();
            base.OnStop();
        }

        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }

        public void Update(float dt)
        {
            if (_Panel != null)
            {
                _Panel.LoopInfoRun();
                _Panel.FlashSelector();
                //
                _ButtonTimerConter -= dt;
                //
                var v = Input.GetAxis("Vertical");
                if ( Mathf.Abs(v) > 0 && _ButtonTimerConter <= 0)
                {
                    _Panel.SelectorMove();
                    _ButtonTimerConter = 0.2f;
                }
                if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Submit"))
                {
                    _Panel.SelectorSelected();
                }
            }
        }
    }
}