using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Bunker.Module;

namespace Bunker.Game
{
    public class FinishPanel : UIPanel
    {
        Button _resetBtn;
        Button _nextBtn;

        public override void OnBegin()
        {
            _resetBtn = _transform.Find("ResetBtn").GetComponent<Button>();
            _nextBtn = _transform.Find("NextBtn").GetComponent<Button>();


        }

        private void OnClickReset()
        {
            Debug.LogError("OnClickReset");
            ModuleManager.getInstance.SendMessage("BattleModule", "RestartLevel");
        }

        private void OnClickNext()
        {

        }
    }
}

