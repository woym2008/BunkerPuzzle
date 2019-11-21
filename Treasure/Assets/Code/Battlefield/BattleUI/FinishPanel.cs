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
            _resetBtn = _transform.Find("Root/ResetBtn").GetComponent<Button>();
            _nextBtn = _transform.Find("Root/NextBtn").GetComponent<Button>();

            _resetBtn.onClick.AddListener(OnClickReset);
        }

        private void OnClickReset()
        {
            ModuleManager.getInstance.SendMessage("Bunker.Game.BattlefieldModule", "RestartLevel");

            UIModule.getInstance.Close<FinishPanel>();
        }

        private void OnClickNext()
        {

        }
    }
}

