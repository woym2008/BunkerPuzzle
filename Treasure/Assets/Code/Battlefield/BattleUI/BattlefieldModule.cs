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
        /// <summary>
        /// 
        /// </summary>
        int _curLevel = Constant.debug_start_level;
        int _areaIndex = 1;
        public int LevelNum
        {
            get
            {
                return _curLevel;
            }
        }
        public int AreaNum
        {
            get
            {
                return _areaIndex;
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

        public override void OnStart(params object[] data)
        {
            base.OnStart();

            _field = new GridField();

            var battlecontroller = ModuleManager.getInstance.GetModule<BattleControllerModule>();
            battlecontroller.Field = _field;

            var areastr = string.Format("Area_{0}",_areaIndex);
            var levelstr = string.Format("Level_{0}", _curLevel);

            if(_field.Load(areastr, levelstr))
            {
                _field.OnElimination += OnElimination;
            }

            //_field.Load(areastr, levelstr);
            //_field.OnElimination = OnElimination;

        }

        public override void OnStop()
        {
            if(_field != null)
            {
                _field.OnElimination -= OnElimination;
            }
            base.OnStop();
        }

        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }

        public void Update(float dt)
        {
        }

        //GridFieldControllerBase _currentController;
        //public bool UseController<T>(params object[] datas) where T : GridFieldControllerBase
        //{
        //    if(_currentController != null)
        //    {
        //        if(!_currentController.IsFinish())
        //        {
        //            return false;
        //        }
        //    }
        //    var controller = Activator.CreateInstance(typeof(T)) as T;

        //    controller.SetGridField(_field);

        //    controller.Excute(datas);

        //    _currentController = controller;

        //    return true;
        //}

        public void RestartLevel()
        {
            UIModule.getInstance.ClearAll();
            var battleInputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
            if (battleInputModule != null)
            {
                battleInputModule.Rest();
            }

            ProcessManager.getInstance.Switch<BattlefieldProcess>(LevelManager.getInstance.CurLevel, LevelManager.getInstance.CurArea);

            //_field.RestartLevel();
        }

        public void NextLevel()
        {
            UIModule.getInstance.ClearAll();
            var battleInputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
            if (battleInputModule != null)
            {
                battleInputModule.Rest();
            }

            //保存一下进度
            string level = LevelManager.getInstance.GetNextLevel(_areaIndex, _curLevel.ToString());
            if(level == "")
            {
                ProcessManager.getInstance.Switch<FinalSceneProcess>();
                return;
            }
            //这里将关数累加
            _curLevel++;
            //SAVE
            SaveLoader.getInstance.SaveGameCurProgress(
               LevelManager.getInstance.CurArea,
               LevelManager.getInstance.CurLevel
            );
            SaveLoader.getInstance.SavePlayerCurItems(
                LevelManager.getInstance.CurArea
            );
            ProcessManager.getInstance.Switch<BattlefieldProcess>(LevelManager.getInstance.CurLevel, LevelManager.getInstance.CurArea);
            
        }

        public void SelectLevel(int areaIndex, int levelIndex)
        {
            UIModule.getInstance.ClearAll();
            var battleInputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
            if (battleInputModule != null)
            {
                battleInputModule.Rest();
            }
            //
            _areaIndex = areaIndex;
            _curLevel = levelIndex;
            LevelManager.getInstance.SetCurrentLevel(_areaIndex,_curLevel);
            //
            //ProcessManager.getInstance.Switch<BattlefieldProcess>();
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