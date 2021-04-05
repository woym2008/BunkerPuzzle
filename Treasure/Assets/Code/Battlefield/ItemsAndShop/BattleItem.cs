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
            //在道具使用完成后也做判断查看任务是否完成
            //主动check 等待事件通知
            MissionManager.getInstance.CheckMissionState();
            //if (MissionManager.getInstance.GetMissionsState() == MissionManager.Mission_Success)
            //{
            //    ProcessManager.getInstance.Switch<EndMenuProcess>(EndMenuProcess.END_GAME_WIN);
            //}
            //else if (MissionManager.getInstance.GetMissionsState() == MissionManager.Mission_Failure)
            //{
            //    ProcessManager.getInstance.Switch<EndMenuProcess>(EndMenuProcess.END_GAME_LOSE);
            //}
        }
    }
}