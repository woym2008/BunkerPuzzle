﻿using UnityEngine;
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
        GameObject _areaFrame;      //Content

        //GameObject _currentArea;

        Button _nextBtn;
        Button _preBtn;
        //
        bool _DEBUG_UNLIMITED_ = true;

        const string buttonPathBase = "UI/Icon";
        public override void OnBegin()
        {
            base.OnBegin();

            _levelList = _transform.Find("LevelList").GetComponent<ScrollRect>();

            _buttonsParent = _levelList.content;

            _btnPrefab = _buttonsParent.Find("LevelBtn").gameObject;
            _btnPrefab.transform.SetParent(_buttonsParent.parent);
            _btnPrefab.SetActive(false);

            _areaFrame = _buttonsParent.gameObject;
            _areaFrame.transform.SetParent(_buttonsParent.parent);
            _areaFrame.SetActive(true);

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
            int active_level = 0,cur_level = 0;
            //载入进度
            SaveLoader.getInstance.LoadGameCurProgress(areaID, ref active_level, ref cur_level);
            //_currentArea = GameObject.Instantiate(_areaFrame);
            //_currentArea.SetActive(true);
            //_currentArea.transform.SetParent(_buttonsParent);
            //_currentArea.transform.localScale = Vector3.one;
            //找到boss level
            var bossLevel = LevelManager.getInstance.GetBossLevel(areaID);

            //foreach (var level in levels)
            for(int i=0;i<levels.Length;++i)
            {
                var level = levels[i];

                var btn = GameObject.Instantiate(_btnPrefab).GetComponent<Button>();
                var btnctrl = btn.gameObject.GetComponent<LevelBtnController>();
                var lock_img = btn.transform.Find("Lock").GetComponent<Image>();
                btnctrl.SetName(level);
                string name = string.Format("{0}/{1}", buttonPathBase, level);
                var sp = Resources.Load<Sprite>(name);
                if (sp != null)
                {
                    btn.image.sprite = sp;
                }
                int level_num = int.Parse(level);
                //int level_num = i;
                //
                _levelButtons.Add(btn);
                btn.transform.SetParent(_areaFrame.transform);
                btn.transform.localScale = Vector3.one;
                btn.gameObject.SetActive(true);
                //modify by wwh 2021-4-5 
                //此处不限制读取当前进度，而对关卡进行锁定
                if (_DEBUG_UNLIMITED_)
                {
                    btn.onClick.AddListener(delegate { OnClickBtn(level_num, areaID); });
                    btn.image.color = Color.white;
                    lock_img.enabled = false;
                }
                else
                {
                    if (active_level > 0)
                    {
                        active_level--;
                        btn.onClick.AddListener(delegate { OnClickBtn(level_num, areaID); });
                        btn.image.color = Color.white;
                        lock_img.enabled = false;
                    }
                    else
                    {
                        btn.image.color = Color.gray;
                        lock_img.enabled = true;
                    }
                }

                //
                var Desc_img = btn.transform.Find("Desc").GetComponent<Image>();
                if (bossLevel == level_num)
                {
                    Desc_img.gameObject.SetActive(true);
                }
                else
                {
                    Desc_img.gameObject.SetActive(false);
                }
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

            //GameObject.Destroy(_currentArea);
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
            if(area == 0)
            {
                ProcessManager.getInstance.Switch<GuideProcess>();
            }
            else
            {
                ProcessManager.getInstance.Switch<BattlefieldProcess>(level, area);
            }            
        }
    }

}
