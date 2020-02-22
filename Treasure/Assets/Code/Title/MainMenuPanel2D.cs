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
        SpriteRenderer selector;
        float rate;

        public override void OnBegin()
        {
            startBtn = _transform.Find("Panel/Start").GetComponent<Button>();
            startBtn.onClick.AddListener(OnStartGameClick);

            loopInfo = _transform.Find("Bar_mask/Text").GetComponent<Text>();
            selector = _transform.Find("Panel/Sel/Selector").GetComponent<SpriteRenderer>();

        }

        public void OnStartGameClick()
        {
            //ProcessManager.getInstance.Switch<BattlefieldProcess>();
            ProcessManager.getInstance.Switch<SelectLevelProcess>();
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
