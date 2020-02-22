using System;
using System.Collections.Generic;
using Bunker.Process;
using Bunker.Module;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace Bunker.Game
{
    public class MainMenuPanel : UIPanel
    {
        Button _startBtn;
        Button _creditsBtn;
        Transform _stuffText;
        Transform _returnText;
        bool isCreditShot = false;
        GameObject Camera_1;
        GameObject Camera_2;


        public override void OnBegin()
        {
            Camera_1 = GameObject.Find("Main Camera");

            Camera_2 = GameObject.Find("Main Camera 2");
            Camera_2.SetActive(false);

            _startBtn = _transform.Find("Start").GetComponent<Button>();
            _creditsBtn = _transform.Find("Stuff").GetComponent<Button>();

            _stuffText = _transform.Find("Stuff/Text");
            _returnText = _transform.Find("Stuff/Text 2");

            _startBtn.onClick.AddListener(OnStartGameClick);
            _creditsBtn.onClick.AddListener(onClickCredits);

        }
        //
        public void OnStartGameClick(){
            //ProcessManager.getInstance.Switch<BattlefieldProcess>();
            ProcessManager.getInstance.Switch<SelectLevelProcess>();
        }

        public void onClickCredits(){
            if(isCreditShot){
                isCreditShot = false;
                Camera_2.SetActive(false);
                Camera_1.SetActive(true);
                //
                _stuffText.gameObject.SetActive(true);
                _returnText.gameObject.SetActive(false);
            }else{
                isCreditShot = true;
                Camera_1.SetActive(false);
                Camera_2.SetActive(true);
                //
                _stuffText.gameObject.SetActive(false);
                _returnText.gameObject.SetActive(true);
            }
        }
    }
}