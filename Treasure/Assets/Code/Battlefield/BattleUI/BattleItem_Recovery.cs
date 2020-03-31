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
    public class BattleItem_Recovery : BattleItem {
        public override void OnInit(){
            base.OnInit();
            BattleItemFactory.getInstance.RegistItemType<BattleItem_Recovery>();
        }
        public override void OnClick(){
            base.OnClick();
            //if can use,use it!
            OnUse();
            MissionManager.getInstance.RegainStep(5);
            Remove();
        }

        public override void OnUse(){
            base.OnUse();
            Debug.Log("Catch Fire!");
            Remove();
        }

        public override void Remove()
        {
            BattleItemFactory.getInstance.ConsumeItem<BattleItem_Recovery>();
            base.Remove();
        }
    }
    
}