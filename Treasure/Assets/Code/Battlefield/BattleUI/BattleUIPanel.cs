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
        //Top Bar
        Button _replay;
        Button _about;
        Button _exit;

        //Replay Dlg
        Transform _replayDlg;
        Button _replay_ok;
        Button _replay_cancle;
        //About Dlg
        Transform _aboutDlg;
        Button _about_ok;
        Button _about_cancle;
        //Exit Dlg
        Transform _exitDlg;
        Button _exit_ok;
        Button _exit_cancle;





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
            //
            _replay = _transform.Find("TopBar/Replay").GetComponent<Button>();
            _about = _transform.Find("TopBar/About").GetComponent<Button>();
            _exit = _transform.Find("TopBar/Exit").GetComponent<Button>();
            _replay.onClick.AddListener(OnReplay);
            _about.onClick.AddListener(OnAbout);
            _exit.onClick.AddListener(OnExit);
            //
            _replayDlg = _transform.Find("ReplayDlg");
            _replay_ok = _transform.Find("ReplayDlg/OK").GetComponent<Button>();
            _replay_cancle = _transform.Find("ReplayDlg/Cancle").GetComponent<Button>();
            _replay_ok.onClick.AddListener(OnReplayDlgOK);
            _replay_cancle.onClick.AddListener(OnReplayDlgCancle);
            //
            _aboutDlg = _transform.Find("AboutDlg");
            _about_ok = _transform.Find("AboutDlg/OK").GetComponent<Button>();
            _about_cancle = _transform.Find("AboutDlg/Cancle").GetComponent<Button>();
            _about_ok.onClick.AddListener(OnAboutDlgCancle);
            _about_cancle.onClick.AddListener(OnAboutDlgCancle);
            //
            _exitDlg = _transform.Find("ExitDlg");
            _exit_ok = _transform.Find("ExitDlg/OK").GetComponent<Button>();
            _exit_cancle = _transform.Find("ExitDlg/Cancle").GetComponent<Button>();
            _exit_ok.onClick.AddListener(OnExitDlgOK);
            _exit_cancle.onClick.AddListener(OnExitDlgCancle);
        }
        //
        public void OnReplay()
        {
            _replayDlg.gameObject.SetActive(true);
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().locked = true;
        }
        public void OnReplayDlgOK()
        {
            ModuleManager.getInstance.GetModule<BattlefieldModule>().RestartLevel();
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().locked = false;
        }
        public void OnReplayDlgCancle()
        {
            _replayDlg.gameObject.SetActive(false);
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().locked = false;
        }
        //
        public void OnAbout()
        {
            _aboutDlg.gameObject.SetActive(true);
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().locked = true;
        }
        public void OnAboutDlgCancle()
        {
            _aboutDlg.gameObject.SetActive(false);
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().locked = false;
        }
        //
        public void OnExit()
        {
            _exitDlg.gameObject.SetActive(true);
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().locked = true;
        }
        public void OnExitDlgOK()
        {
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().locked = false;
            ProcessManager.getInstance.Switch<TitleProcess>();
        }
        public void OnExitDlgCancle()
        {
            _exitDlg.gameObject.SetActive(false);
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().locked = false;
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
