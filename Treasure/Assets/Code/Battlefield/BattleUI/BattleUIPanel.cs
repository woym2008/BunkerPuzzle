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
        //
        Transform _ProgressBar;
        int _ProgressNum;
        public int ProgressNum
        {
            get
            {
                return _ProgressNum;
            }
        }
        //
        public override void OnBegin()
        {
            _ItemPanel = _transform.Find("Left_Bar/ScrollRectPanel/Viewport/ItemPanel");
            _MissionPanel = _transform.Find("Right_Bar/Mission/Panel");
            _levelText = _transform.Find("Right_Bar/Level_BG/Level").GetComponent<Text>();
            _ProgressBar = _transform.Find("Right_Bar/ProgressBar");
        }
        //
        public void SetLevelText(int num)
        {
            _levelText.text = num.ToString();
        }
        //
        public void AddItem(GameObject item){
            item.transform.SetParent(_ItemPanel);
            item.transform.localScale = Vector3.one;
        }

        public void AddMissionItem(GameObject item){
            item.transform.SetParent(_MissionPanel);
            item.transform.localScale = Vector3.one;

        }
        /* 0~1 */
        public void SetProgressNum(float n)
        {
            n = Mathf.Clamp(n, 0, 1);
            _ProgressNum = Mathf.CeilToInt(n * 7);
            for (int i = 1;i <= 7;++i)
            {
                _ProgressBar.Find("N" + i).GetComponent<Image>().enabled = false;
                if (i < _ProgressNum)
                {
                    _ProgressBar.Find("N" + i).GetComponent<Image>().enabled = true;
                }
            }         
        }
    }
}
