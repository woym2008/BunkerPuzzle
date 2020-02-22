using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Bunker.Process;

namespace Bunker.Game
{
    public class AreaPanel : UIPanel
    {
        List<Button> _levelButtons = new List<Button>();
        ScrollRect _levelList;
        Transform _buttonsParent;
        GameObject _btnPrefab;
        GameObject _areaFrame;

        GameObject _currentArea;

        Button _nextBtn;
        Button _preBtn;

        const string buttonPathBase = "UI/Icon";
        public override void OnBegin()
        {
            base.OnBegin();

            _levelList = _transform.Find("LevelList").GetComponent<ScrollRect>();

            _buttonsParent = _levelList.content;

            _btnPrefab = _buttonsParent.Find("LevelBtn").gameObject;
            _areaFrame = _buttonsParent.Find("AreaFrame").gameObject;
            _btnPrefab.transform.parent = _buttonsParent.parent;
            _areaFrame.transform.parent = _buttonsParent.parent;
            _btnPrefab.SetActive(false);
            _areaFrame.SetActive(false);

            _buttonsParent.DetachChildren();

            _nextBtn = _transform.Find("NextButton").GetComponent<Button>();
            _preBtn = _transform.Find("PreButton").GetComponent<Button>();

            _nextBtn.onClick.AddListener(NextArea);
            _preBtn.onClick.AddListener(PreArea);
        }

        public override void OnOpen(params object[] datas)
        {
            base.OnOpen(datas);
            string[] levels = (string[])datas[0];
            int areaID = (int)datas[1];
            OpenArea(areaID, levels);
        }

        public override void OnClose()
        {
            CloseArea();
            base.OnClose();
        }

        void OpenArea(int areaID, string[] levels)
        {
            var areaframe = GameObject.Instantiate(_areaFrame);
            _currentArea = areaframe;
            _currentArea.SetActive(true);
            areaframe.transform.parent = _buttonsParent;
            foreach (var level in levels)
            {
                var btn = GameObject.Instantiate(_btnPrefab).GetComponent<Button>();
                var btnctrl = btn.gameObject.GetComponent<LevelBtnController>();
                btnctrl.SetName(level);
                string name = string.Format("{0}/{1}", buttonPathBase, level);
                var sp = Resources.Load<Sprite>(name);
                if (sp != null)
                {
                    btn.image.sprite = sp;
                }

                _levelButtons.Add(btn);
                btn.transform.parent = areaframe.transform;

                btn.onClick.AddListener(delegate { OnClickBtn(int.Parse(level), areaID); });

                btn.gameObject.SetActive(true);
            }
        }

        void CloseArea()
        {
            _buttonsParent.DetachChildren();
            foreach (var btn in _levelButtons)
            {
                GameObject.Destroy(btn.gameObject);
            }
            _levelButtons.Clear();

            GameObject.Destroy(_currentArea);
        }

        void NextArea()
        {
            var levels = LevelManager.getInstance.GetAreaLevels(LevelManager.getInstance.CurArea + 1);
            if(levels != null)
            {
                CloseArea();
                OpenArea(LevelManager.getInstance.CurArea + 1, levels);
                LevelManager.getInstance.CurArea++;
            }
        }

        void PreArea()
        {
            var levels = LevelManager.getInstance.GetAreaLevels(LevelManager.getInstance.CurArea - 1);
            if (levels != null)
            {
                CloseArea();
                OpenArea(LevelManager.getInstance.CurArea - 1, levels);
                LevelManager.getInstance.CurArea--;
            }
        }

        public void OnClickBtn(int level, int area)
        {
            ProcessManager.getInstance.Switch<BattlefieldProcess>(level,area);
        }
    }

}
