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
    public class BattleItem_Lightning : BattleItem {
        public override void OnInit(){
            base.OnInit();
        }
        public override void OnClick(){
            base.OnClick();
            //if can use,use it!
            if(_needTarget){

            }
            OnUse();
            
        }

        public override void OnUse(){
            base.OnUse();
            Debug.Log("Catch Fire!");
        }
    }
    
}