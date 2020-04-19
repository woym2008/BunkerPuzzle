using Bunker.Module;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bunker.Game
{
    public class GuideUIPanel : UIPanel, IBattleUI
    {
        Transform _ItemPanel;
        Transform _MissionPanel;
        //
        Text _levelText;
        Text _apText;
        //
        Transform _ProgressBar;

        Image _arrow;
        // guide UI
        Transform _guideBar;
        Image _guideHead;
        Text _guideText;
        //Top Bar
        Button _replay;
        Button _about;
        Button _exit;
        // boss UI
        Transform _bossWarning;//
        Transform _bossBar;
        Image _bossHead;
        Text _bossText;
        //
        GuideClickPanel _clickPanel;
        Transform _clickMark;

        int _step = 0;
        bool _canClick;

        BattleUIModule _battleUIModule;

        int _ProgressNum;
        public int ProgressNum
        {
            get
            {
                return _ProgressNum;
            }
        }

        public override void OnBegin()
        {
            _ItemPanel = _transform.Find("Left_Bar/ScrollRectPanel/Viewport/ItemPanel");
            _MissionPanel = _transform.Find("Right_Bar/Mission/Panel");
            _levelText = _transform.Find("Right_Bar/Level_BG/Level").GetComponent<Text>();
            _ProgressBar = _transform.Find("Right_Bar/ProgressBar");
            //
            _apText = _transform.Find("Right_Bar/AP/AP_Num").GetComponent<Text>();
            //_uiCamera = _transform.Find("UICamera").GetComponent<Camera>();
            //_bossWarning = _transform.Find("BossWarning");
            _guideBar = _transform.Find("Guide_Bar");
            _guideHead = _transform.Find("Guide_Bar/Guide_Head").GetComponent<Image>();
            _guideText = _transform.Find("Guide_Bar/Guide_Text").GetComponent<Text>();
            //
            _replay = _transform.Find("TopBar/Replay").GetComponent<Button>();
            _about = _transform.Find("TopBar/About").GetComponent<Button>();
            _exit = _transform.Find("TopBar/Exit").GetComponent<Button>();

            _clickPanel = _transform.Find("ClickPanel").GetComponent<GuideClickPanel>();

            _clickMark = _transform.Find("ClickMark");
            _clickMark.gameObject.SetActive(false);

            //_replay.onClick.AddListener(OnReplay);
            //_about.onClick.AddListener(OnAbout);
            //_exit.onClick.AddListener(OnExit);
            //
            //_replayDlg = _transform.Find("ReplayDlg");
            //_replay_ok = _transform.Find("ReplayDlg/OK").GetComponent<Button>();
            //_replay_cancle = _transform.Find("ReplayDlg/Cancle").GetComponent<Button>();
            //_replay_ok.onClick.AddListener(OnReplayDlgOK);
            //_replay_cancle.onClick.AddListener(OnReplayDlgCancle);
            //
            //_aboutDlg = _transform.Find("AboutDlg");
            //_about_ok = _transform.Find("AboutDlg/OK").GetComponent<Button>();
            //_about_cancle = _transform.Find("AboutDlg/Cancle").GetComponent<Button>();
            //_about_ok.onClick.AddListener(OnAboutDlgCancle);
            //_about_cancle.onClick.AddListener(OnAboutDlgCancle);
            //
            //_exitDlg = _transform.Find("ExitDlg");
            //_exit_ok = _transform.Find("ExitDlg/OK").GetComponent<Button>();
            //_exit_cancle = _transform.Find("ExitDlg/Cancle").GetComponent<Button>();
            //_exit_ok.onClick.AddListener(OnExitDlgOK);
            //_exit_cancle.onClick.AddListener(OnExitDlgCancle);

            _step = 0;
        }

        public override void OnOpen(params object[] datas)
        {
            base.OnOpen(datas);
            _step = 0;
            _canClick = true;
            OnNext();
            _clickPanel.OnClick = OnNext;

            //_battleUIModule = ModuleManager.getInstance.GetModule<BattleUIModule>();
            
        }

        public override void OnClose()
        {
            base.OnClose();

            _clickPanel.OnClick = null;
        }

        public override void SendMessage(params object[] objects)
        {
            base.SendMessage(objects);

            if (objects.Length == 1 && objects[0].ToString() == "Next")
            {
                OnNext();
            }
        }

        public void OnNext()
        {
            if (!_canClick)
            {
                return;
            }
            

            switch (_step)
            {
                //介绍游戏玩法
                case 0:
                    {
                        ModuleManager.getInstance.GetModule<BattlefieldInputModule>().Pause(true);
                        MonoBehaviourHelper.StartCoroutine(GuideRunning(1, "介绍游戏玩法", OnStepFinish));

                    }
                    break;
                case 1:
                    {
                        MonoBehaviourHelper.StartCoroutine(GuideRunning(1, "介绍游戏玩法2", OnStepFinish));
                    }
                    break;
                //AP介绍
                case 2:
                    {
                        MonoBehaviourHelper.StartCoroutine(GuideRunning(1, "请点击有方向箭头的色块", OnStepFinish));
                    }
                    break;
                //点击移动
                case 3:
                    {
                        _clickMark.gameObject.SetActive(true);
                        OnCloseBar();
                        ModuleManager.getInstance.GetModule<BattlefieldInputModule>().Pause(false);
                        OnCloseClickPanel();
                        ModuleManager.getInstance.GetModule<BattlefieldInputModule>().onClickedTile += OnClickDirect;
                    }
                    break;
                //消除
                case 4:
                    {
                        OnCloseBar();
                        ModuleManager.getInstance.GetModule<BattlefieldInputModule>().Pause(false);
                        OnCloseClickPanel();
                        ModuleManager.getInstance.GetModule<GuideModule>().Field.OnElimination += OnElimination;
                    }
                    break;
                //过关
                case 5:
                    {
                        OnCloseBar();
                        OnCloseClickPanel();
                        ModuleManager.getInstance.GetModule<BattlefieldInputModule>().Pause(false);
                    }
                    break;
                //case 6:
                //    {
                //        ModuleManager.getInstance.GetModule<BattlefieldInputModule>().Pause(false);
                //    }
                //    break;
            }

            _step++;
        }
        IEnumerator GuideRunning(float t, string str, params UnityAction[] evt)
        {
            _guideBar.gameObject.SetActive(true);
            _guideText.text = str;
            yield return new WaitForSeconds(t);
            //_guideBar.gameObject.SetActive(false);
            foreach(var e in evt)
            {
                if (e != null) e.Invoke();
            }
            
            yield return null;
        }
        void OnClickDirect(IGridObject grid)
        {
            _clickMark.gameObject.SetActive(false);
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().Pause(true);
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().onClickedTile -= OnClickDirect;
            MonoBehaviourHelper.StartCoroutine(GuideRunning(1, "你已经成功的移动了，注意移动会消耗AP，如果AP全部消耗任务将会失败，继续移动吧", OnOpenClickPanel, OnStepFinish));
        }
        void OnElimination(int num)
        {
            ModuleManager.getInstance.GetModule<BattlefieldInputModule>().Pause(true);
            ModuleManager.getInstance.GetModule<GuideModule>().Field.OnElimination -= OnElimination;
            MonoBehaviourHelper.StartCoroutine(GuideRunning(1, "好的，你已经将同样颜色的水块消除掉了，继续将所有的水块都消除", OnOpenClickPanel, OnStepFinish));
        }

        void OnStepFinish()
        {
            _canClick = true;
        }
        void OnCloseBar()
        {
            _guideBar.gameObject.SetActive(false);
        }
        void OnCloseClickPanel()
        {
            _clickPanel.gameObject.SetActive(false);
        }
        void OnOpenClickPanel()
        {
            if(_clickPanel != null)
            {
                _clickPanel.gameObject.SetActive(true);
            }
        }

        public void ShowBossTalkingVFX(float t, string str, UnityAction evt)
        {
            MonoBehaviourHelper.StartCoroutine(_BossTalkingVFX(t, str, evt));
        }

        IEnumerator _BossTalkingVFX(float t, string str, UnityAction evt)
        {
            ShowBossBar(true);
            SetBossText(str);
            yield return new WaitForSeconds(t);
            ShowBossBar(false);
            if (evt != null) evt.Invoke();
            yield return null;
        }

        public void AddItem(GameObject item)
        {
            item.transform.SetParent(_ItemPanel);
            item.transform.localScale = Vector3.one;
        }

        public void AddMissionItem(GameObject item)
        {
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
            for (int i = 1; i <= 7; ++i)
            {
                _ProgressBar.Find("N" + i).GetComponent<Image>().enabled = false;
                if (i < _ProgressNum)
                {
                    _ProgressBar.Find("N" + i).GetComponent<Image>().enabled = true;
                }
            }
        }

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

        public void SetLevelText(int num)
        {
            _levelText.text = num.ToString();
        }
    }
}
