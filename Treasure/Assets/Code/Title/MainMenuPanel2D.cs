using System;
using System.Collections.Generic;
using Bunker.Process;
using Bunker.Module;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace Bunker.Game
{
    public class MainMenuPanel2D : UIPanel
    {
        const float loopSpeed = 20;
        float flashSpeed = 5;
        Text loopInfo;
        Button startBtn;
        Button creditsBtn;
        SpriteRenderer selector;
        float rate;
        //
        Animator titleAnimator;

        public override void OnBegin()
        {
            titleAnimator = _transform.Find("Panel/Title").GetComponent<Animator>();

            startBtn = _transform.Find("Panel/Start").GetComponent<Button>();
            startBtn.onClick.AddListener(OnStartGameClick);

            creditsBtn = _transform.Find("Panel/Credits").GetComponent<Button>();
            creditsBtn.onClick.AddListener(OnCreditsClick);

            loopInfo = _transform.Find("Bar_mask/Text").GetComponent<Text>();
            selector = _transform.Find("Panel/Sel/Selector").GetComponent<SpriteRenderer>();

            selector.transform.SetParent(startBtn.transform);

        }

        public void OnStartGameClick()
        {
            //ProcessManager.getInstance.Switch<BattlefieldProcess>();
            ProcessManager.getInstance.Switch<SelectLevelProcess>();
        }

        public void OnCreditsClick()
        {
            Debug.Log("Show Credits");
            titleAnimator.SetBool("ShowList", true);
        }

        public void SelectorMove()
        {
            if (selector.transform.parent == startBtn.transform)
            {
                selector.transform.SetParent(creditsBtn.transform);
                selector.transform.localPosition = Vector3.zero;
            }
            else
            {
                selector.transform.SetParent(startBtn.transform);
                selector.transform.localPosition = Vector3.zero;
            }
        }

        public void SelectorSelected()
        {
            if(selector.transform.parent == startBtn.transform)
            {
                OnStartGameClick();
            }
            else
            {
                OnCreditsClick();
            }
        }

        public void LoopInfoRun()
        {
            loopInfo.transform.localPosition += Vector3.left * Time.deltaTime * loopSpeed;
            if(loopInfo.transform.localPosition.x < -220)
            {
                loopInfo.transform.localPosition = new Vector3(60, loopInfo.transform.localPosition.y, loopInfo.transform.localPosition.z);
            }
        }

        public void FlashSelector()
        {
            if(rate > 1 || rate < 0)
            {
                rate = Mathf.Clamp(rate, 0, 1);
                flashSpeed = flashSpeed  * - 1;
            }
            rate += flashSpeed * Time.deltaTime;
            selector.color = Color.Lerp(Color.white, Color.grey, rate);
        }
    }
}
