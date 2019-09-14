using System;
using System.Collections.Generic;
using Bunker.Process;
using Bunker.Module;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Bunker.Game
{
    public class BattleUIPanel : UIPanel
    {
        Transform _ItemPanel;
        Transform _MissionPanel;
        //
        Text    _levelText;
        public override void OnBegin()
        {
            _ItemPanel = _transform.Find("Left_Bar/ScrollRectPanel/Viewport/ItemPanel");
            _MissionPanel = _transform.Find("Right_Bar/Mission/Panel");
            _levelText = _transform.Find("Right_Bar/Level").GetComponent<Text>();
        }
        //
        public void AddItem(GameObject item){
            item.transform.parent = _ItemPanel;
        }

        public void AddMissionItem(GameObject item){
            item.transform.parent = _MissionPanel;
        }
    }
}
