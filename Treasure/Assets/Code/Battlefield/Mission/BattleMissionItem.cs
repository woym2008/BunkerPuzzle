#region 描述
//这个是道具的类,在左侧面板中的道具格子
#endregion
using System;
using System.Collections.Generic;
using Bunker.Process;
using Bunker.Module;
using UnityEngine;
using UnityEngine.UI;

namespace Bunker.Game
{
    public class BattleMissionItem : MonoBehaviour {
        
        Text _Num;
        Image _Icon;
        public void OnInit() {
            _Num = GetComponentInChildren<Text>();
            _Icon = GetComponentInChildren<Image>();
        }
        public void OnChange(string itemName , int n ,int max){
            //这里先注释掉itemname
            _Num.text = /*itemName + */n.ToString() + '/' + max.ToString();
        }

        public void SetIcon(Sprite texture){
            //_Icon.overrideSprite = texture;
            _Icon.sprite = texture;

        }

    }
}