﻿using UnityEngine;
using System.Collections;
using Bunker.Module;
using System;

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
    }
}