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
        //
        GameObject _command_sad;
        GameObject _command_happy;
        //
        Text _end_text;


        public override void OnBegin()
        {
            _resetBtn = _transform.Find("Root/ResetBtn").GetComponent<Button>();
            _nextBtn = _transform.Find("Root/NextBtn").GetComponent<Button>();

            _resetBtn.onClick.AddListener(OnClickReset);
            _nextBtn.onClick.AddListener(OnClickNext);

            _command_sad = _transform.Find("Root/Commander_Sad").gameObject;
            _command_happy = _transform.Find("Root/Commander_Happy").gameObject;

            _end_text = _transform.Find("Root/WinText").GetComponent<Text>();
        }

        public void SetCommandMood(int mood)
        {
            if (mood == 1)  //Win
            {
                _command_happy.SetActive(true);
                _command_sad.SetActive(false);
                _end_text.text = "Great!\nTake into next battle.";
            }
            else
            {
                _command_happy.SetActive(false);
                _command_sad.SetActive(true);
                _end_text.text = "Oh Dame!\nYou must try to save us.";
            }
        }

        private void OnClickReset()
        {
            ModuleManager.getInstance.SendMessage("Bunker.Game.BattlefieldModule", "RestartLevel");

            UIModule.getInstance.Close<FinishPanel>();
        }

        private void OnClickNext()
        {
            ModuleManager.getInstance.SendMessage("Bunker.Game.BattlefieldModule", "NextLevel");

            UIModule.getInstance.Close<FinishPanel>();
        }
    }
}

