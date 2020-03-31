using System;
using System.Collections;
using System.Collections.Generic;
using Bunker.Process;
using Bunker.Module;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
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
        Text    _apText;
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
        //Camera _uiCamera;
        //

        // boss UI
        Transform _bossWarning;//
        Transform _bossBar;
        Image _bossHead;
        Text _bossText;
        


        public override void OnBegin()
        {
            _ItemPanel = _transform.Find("Left_Bar/ScrollRectPanel/Viewport/ItemPanel");
            _MissionPanel = _transform.Find("Right_Bar/Mission/Panel");
            _levelText = _transform.Find("Right_Bar/Level_BG/Level").GetComponent<Text>();
            _ProgressBar = _transform.Find("Right_Bar/ProgressBar");
            //
            _apText = _transform.Find("Right_Bar/AP/AP_Num").GetComponent<Text>();
            //_uiCamera = _transform.Find("UICamera").GetComponent<Camera>();
            _bossWarning = _transform.Find("BossWarning");
            _bossBar = _transform.Find("Boss_Bar");
            _bossHead = _transform.Find("Boss_Bar/Boss_Head").GetComponent<Image>();
            _bossText = _transform.Find("Boss_Bar/Boss_Text").GetComponent<Text>();

        }
        //
        public void SetLevelText(int num)
        {
            _levelText.text = num.ToString();
        }
        //-----BOSS
        public void SetBossHead(Sprite head)
        {
            _bossHead.sprite = head;
        }
        public void SetBossText(string sentence)
        {
            _bossText.text = sentence;
        }
        public void ShowBossWarning(bool s)
        {
            _bossWarning.gameObject.SetActive(s);
        }
        public void ShowBossBar(bool s)
        {
            _bossBar.gameObject.SetActive(s);
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

        public void SetAPNum(float n)
        {
            int i = Mathf.RoundToInt(n);
            _apText.text = i.ToString("00");
        }

        public void FlashVFX(float t)
        {
            //MonoBehaviourHelper.StartCoroutine(Coroutine_Flash(t));
        }
        /*
        IEnumerator Coroutine_Flash(float t)
        {
            _uiCamera.GetComponent<PostProcessLayer>().enabled = true;
            yield return new WaitForSeconds(t);
            _uiCamera.GetComponent<PostProcessLayer>().enabled = false;
            yield return null;
        }
        */
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
