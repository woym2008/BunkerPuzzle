using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using Bunker.Process;

namespace Bunker.Game
{
    /**
     * 
     *          主角模块
     *     主角模块跟着battle同时启动，根据地图进行相关配置
     *     根据不同的配置，给主角增加相应的组件，主角其实是一个抽象的turn，StarredTurn
     *     
     *     StarringRole 是派生自mono，是unity和代码连接的桥梁，unity中有回调和prefab设置时，
     *     可以通过这个桥梁供不同的starredComponent引用
     * 
     *     StarredComponents是主角的组件，其实主要是区分不同的逻辑的，每关根据对主角不同的配置，从而对于每一关来说
     *     主角的一些逻辑都在变化
     *     例如：
     *          1、有些关卡，没有主角的可视化本体，但是在玩家游戏中，主角回特定和玩家聊天
     *          2、有些关卡，主角需要搬运货物去某地
     *          3、有些关卡，主角需要拾取地面上的道具，进行收集
     * 
     * */
    public class StarredModule : LogicModule
    {
        StarredTurn starredTurn;
        ///***********************
        public StarredModule() : base(typeof(StarredModule).ToString())
        {

        }
        public StarredModule(string name) : base(name)
        {

        }
        #region OVERRIDE
        /*
         *  OVERRIDE 
         */
        public override void Create()
        {
            base.Create();            
        }
        public override void OnStart(params object[] data)
        {
            base.OnStart(data);
            //这里，我们就不动态向TurnsModule里面添加StarredTurn了，我们直接从TurnsModule查找出保存即可
            starredTurn = ModuleManager.getInstance.GetModule<BattleTurnsModule>().FindTurn("StarredTurn") as StarredTurn;
        }
        public override void OnStop()
        {
            base.OnStop();
        }
        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }
        #endregion
        public void CreateStarringRole(StarredComponentTypes[] sct)
        {
            foreach (var t in sct)
            {
                switch(t)
                {
                    case StarredComponentTypes.Starred_BaseMovement:
                    {
                        starredTurn.AddComponent(null);
                    }
                        break;

                    case StarredComponentTypes.Starred_Renderer:
                    {
                        starredTurn.AddComponent(null);
                    }
                        break;

                }
            }
        }


    }
}
