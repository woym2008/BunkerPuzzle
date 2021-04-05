using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Bunker.Module;
using System;

namespace Bunker.Game
{
    public class BattleUIModule : LogicModule
    {
        IBattleUI _UIPanel;
        int _uiType = 0;
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

        public override void OnStart(params object[] data)
        {
            base.OnStart();
            if(data.Length == 1 && data[0].ToString() == "Guide")
            {
                _uiType = 1;
                _UIPanel = UIModule.getInstance.Open<GuideUIPanel>();
            }
            else
            {
                _uiType = 0;
                _UIPanel = UIModule.getInstance.Open<BattleUIPanel>();
            }
            

        }

        public override void OnStop()
        {
            UIModule.getInstance.Close<BattleUIPanel>();
            base.OnStop();
        }

        public void DisplayBattleUIPanel()
        {
            _uiType = 0;
            _UIPanel = UIModule.getInstance.Open<BattleUIPanel>();
        }

        public void HideBattleUIPanel()
        {
            UIModule.getInstance.Close<BattleUIPanel>();
        }

        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }

        public void Update(float dt)
        {
            //Test Here
            if(Input.GetKeyDown(KeyCode.I)){
                BattleItemFactory.getInstance.CreateBattleItem<BattleItem_Fire>(1);
            }
            if(Input.GetKeyDown(KeyCode.O)){
                BattleItemFactory.getInstance.CreateBattleItem<BattleItem_Lightning>(1);
            }
            if (Input.GetKeyDown(KeyCode.P)){
                BattleItemFactory.getInstance.CreateBattleItem<BattleItem_Recovery>(1);
            }
            //
            if (Input.GetKeyDown(KeyCode.Alpha9)){
                //CreateMissionItem("IconSet_0",6);
            }
            if(Input.GetKeyDown(KeyCode.Alpha0)){
                //CreateMissionItem("IconSet_1",8);
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
        public void CreateItem(string item_type)
        {
            //var uiname = item_type.Substring(item_type.LastIndexOf('.') + 1);    //去除命名空间
            var uiname = item_type;
            var item = GameObject.Instantiate(Resources.Load("UI/" + uiname)) as GameObject;
            item.name = uiname;
            var bi = item.AddComponent(Type.GetType(Constant.DOMAIN_PREFIX + item_type)) as BattleItem;
            bi.OnInit();
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

        public IBattleUI GetBattleUIPanel()
        {
            return _UIPanel;
        }

        public void ShowBossWarningVFX(float t,UnityAction evt)
        {
            MonoBehaviourHelper.StartCoroutine(_BossWarningVFX(t,evt));
        }

        IEnumerator _BossWarningVFX(float t, UnityAction evt)
        {
            _UIPanel.ShowBossWarning(true);
            yield return new WaitForSeconds(t);
            _UIPanel.ShowBossWarning(false);
            if (evt != null) evt.Invoke();
            yield return null;
        }

        public void ShowBossTalkingVFX(float t, string str,UnityAction evt)
        {
            MonoBehaviourHelper.StartCoroutine(_BossTalkingVFX(t, str ,evt));
        }

        IEnumerator _BossTalkingVFX(float t, string str, UnityAction evt)
        {
            _UIPanel.ShowBossBar(true);
            _UIPanel.SetBossText(str);
            yield return new WaitForSeconds(t);
            _UIPanel.ShowBossBar(false);
            if(evt != null) evt.Invoke();
            yield return null;
        }
    }   

}