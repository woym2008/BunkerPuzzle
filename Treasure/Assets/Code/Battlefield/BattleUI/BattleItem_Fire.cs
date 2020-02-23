#region 描述
#endregion
using System;
using System.Collections.Generic;
using Bunker.Process;
using Bunker.Module;
using UnityEngine;
using UnityEngine.UI;

namespace Bunker.Game
{
    public class BattleItem_Fire : BattleItem {
        public override void OnInit(){
            base.OnInit();
            BattleItemFactory.getInstance.RegistItemType<BattleItem_Fire>();
        }
        public override void OnClick(){
            base.OnClick();
            //if can use,use it!
            OnUse();
        }

        public override void OnUse(){
            base.OnUse();
            Debug.Log("Catch Fire!");
            MissionManager.getInstance.RegainStep(5);
            Remove();
        }
        public override void Remove()
        {
            BattleItemFactory.getInstance.ConsumeItem<BattleItem_Fire>();
            base.Remove();
        }
    }
    
}