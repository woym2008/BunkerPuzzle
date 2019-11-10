using UnityEngine;
using System.Collections;
using Bunker.Module;
using System;
using Bunker.Process;

namespace Bunker.Game
{ 
    public class BattlefieldModule : LogicModule
    {
        private GridField _field;
        public GridField Field
        {
            get
            {
                return _field;
            }
        }

        public BattlefieldModule() : base(typeof(BattlefieldModule).ToString())
        {
            
        }
        public BattlefieldModule(string name) :base(name)
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

            _field = new GridField();
            _field.Load("Level_1");
            _field.OnElimination = OnElimination;
        }

        public override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }

        public void Update(float dt)
        {
            //Field?.EliminationUpdate();
            //if(Input.mou)
            if(Field.IsAllGridsElimination())
            {
                ProcessManager.getInstance.Switch<EndMenuProcess>();
            }
        }

        GridFieldControllerBase _currentController;
        public void UseController<T>(params object[] datas) where T : GridFieldControllerBase
        {
            if(_currentController != null)
            {
                if(!_currentController.IsFinish())
                {
                    return;
                }
            }
            var controller = Activator.CreateInstance(typeof(T)) as T;

            controller.SetGridField(_field);

            controller.Excute(datas);

            _currentController = controller;
        }

        public void RestartLevel()
        {
            Debug.LogError("RestartLevel");
            _field.RestartLevel();
        }

        /// <summary>
        /// gridfiled 消除块后的回调
        /// </summary>
        /// <param name="num">Number.</param>
        void OnElimination(int num)
        {
            Debug.LogFormat("一共消除了{0}个块",num);
        }
    }
}