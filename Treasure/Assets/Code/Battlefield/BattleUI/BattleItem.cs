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
    public abstract class BattleItem : MonoBehaviour {
        
        protected bool _needTarget = false;
        protected Button _button;
        public virtual void OnInit(){
            _button = GetComponent<Button>(); 
            _button.onClick.AddListener(OnClick);
        }
        public virtual void OnClick(){
            //if can use,use it!
        }

        public virtual void OnUse(){
            Debug.Log("BattleItem Use");
        }

        public virtual void Remove(){
            transform.SetParent(null);
            Destroy(this.gameObject);
        }
    }
}