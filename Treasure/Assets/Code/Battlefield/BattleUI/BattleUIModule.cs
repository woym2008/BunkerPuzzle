using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Bunker.Module;

namespace Bunker.Game
{
    public class BattleUIModule : LogicModule
    {
        BattleUIPanel   _UIPanel;
        int _progress = 50;

        //
        Dictionary<string, Sprite> _MissionIconlist = new Dictionary<string, Sprite>();
        //
        public BattleUIModule() : base(typeof(BattleUIModule).ToString())
        {
            
        }
        public BattleUIModule(string name) :base(name)
        {

        }
        public override void Create()
        {
            base.Create();
            //
            Sprite[] missionIcons = Resources.LoadAll<Sprite>("UI/IconSet");
            foreach(var spr in missionIcons){
                _MissionIconlist[spr.name] = spr;
            }
        }

        public override void Release()
        {
            base.Release();
        }

        public override void OnStart()
        {
            base.OnStart();
            _UIPanel = UIModule.getInstance.Open<BattleUIPanel>();

        }

        public override void OnStop()
        {
            UIModule.getInstance.Close<BattleUIPanel>();
            base.OnStop();

        }

        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }

        public void Update(float dt)
        {
            //Test Here
            if(Input.GetKeyDown(KeyCode.I)){
                CreateItem<BattleItem_Fire>();
            }
            if(Input.GetKeyDown(KeyCode.O)){
                CreateItem<BattleItem_Lightning>();
            }
            if(Input.GetKeyDown(KeyCode.P)){
                CreateItem<BattleItem_Recovery>();
            }
            //
            if(Input.GetKeyDown(KeyCode.Alpha9)){
                //CreateMissionItem("IconSet_0",6);
            }
            if(Input.GetKeyDown(KeyCode.Alpha0)){
                //CreateMissionItem("IconSet_1",8);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //_progress -= 5;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //_progress += 5;
            }


        }
        //////
        public void CreateItem<T>() where T : BattleItem, new()
        {
            var uiname = typeof(T).Name;
            var item = GameObject.Instantiate(Resources.Load("UI/" + uiname)) as GameObject;
            item.name = uiname;
            item.AddComponent<T>().OnInit();
            _UIPanel.AddItem(item);
        }

        /* itemType是关卡中收集的一些东西，destNum是目标数量 */
        public BattleMissionItem CreateMissionItem(string res_name,int destNum)
        {
            var go = GameObject.Instantiate(Resources.Load("UI/BattleMissionItem")) as GameObject;
            var item = go.AddComponent<BattleMissionItem>();
            item.OnInit();
            item.SetIcon(_MissionIconlist[res_name]);
            _UIPanel.AddMissionItem(go);
            return item;
        }

        public BattleUIPanel GetBattleUIPanel()
        {
            return _UIPanel;
        }
    }   

}