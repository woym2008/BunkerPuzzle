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

        const string buttonPathBase = "UI/Icon";
        public override void OnBegin()
        {
            base.OnBegin();

            _levelList = _transform.Find("LevelList").GetComponent<ScrollRect>();

            _buttonsParent = _levelList.content;

            _btnPrefab = _buttonsParent.GetChild(0).gameObject;
            _btnPrefab.transform.parent = _buttonsParent.parent;
            _btnPrefab.SetActive(false);

            _buttonsParent.DetachChildren();
        }

        public override void OnOpen(params object[] datas)
        {
            base.OnOpen(datas);
            string[] levels = (string[])datas[0];
            int areaID = (int)datas[1];
            foreach (var level in levels)
            {
                var btn = GameObject.Instantiate(_btnPrefab).GetComponent<Button>();
                var btnctrl = btn.gameObject.GetComponent<LevelBtnController>();
                btnctrl.SetName(level);
                string name = string.Format("{0}/{1}", buttonPathBase, level);
                var sp = Resources.Load<Sprite>(name);
                if(sp != null)
                {
                    btn.image.sprite = sp;
                }

                _levelButtons.Add(btn);
                btn.transform.parent = _buttonsParent;

                btn.onClick.AddListener(delegate { OnClickBtn(int.Parse(level), areaID); });

                btn.gameObject.SetActive(true);
            }
        }

        public override void OnClose()
        {
            _buttonsParent.DetachChildren();
            foreach(var btn in _levelButtons)
            {
                GameObject.Destroy(btn.gameObject);
            }
            _levelButtons.Clear();
            base.OnClose();
        }

        public void OnClickBtn(int level, int area)
        {
            ProcessManager.getInstance.Switch<BattlefieldProcess>(level,area);
        }
    }

}
