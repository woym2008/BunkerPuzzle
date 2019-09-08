using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bunker.Module
{
    public class UIModule : ServicesModule<UIModule>
    {
        Dictionary<string, UIPanel> _UIlist = new Dictionary<string, UIPanel>();

        Stack<UIPanel> _backstack = new Stack<UIPanel>();

        public T Open<T>(params object[] datas) where T : UIPanel, new()
        {
            var uiname = typeof(T).Name;
            if (!_UIlist.ContainsKey(uiname))
            {
                var uipanel = new T();

                uipanel.OnInit(uiname);

                uipanel.OnOpen(datas);

                _UIlist[uiname] = uipanel;

                if (_backstack.Count > 0)
                {
                    var lastpanel = _backstack.Peek();
                    lastpanel.OnClose();
                }
                _backstack.Push(uipanel);

                return uipanel;
            }
            var retpanel = _UIlist[uiname];
            retpanel.OnOpen(datas);

            if (_backstack.Count > 0)
            {
                var lastpanel = _backstack.Peek();
                lastpanel.OnClose();
            }
            _backstack.Push(retpanel);

            return retpanel as T;
        }

        public void Close<T>()
        {
            var uiname = typeof(T).Name;
            if (_UIlist.ContainsKey(uiname))
            {
                _UIlist[uiname].OnClose();
            }

            _backstack.Pop();
        }

        public void Back()
        {
            if (_backstack.Count > 1)
            {
                var curpanel = _backstack.Pop();
                curpanel.OnClose();
                var lastpanel = _backstack.Peek();
                lastpanel.OnOpen();
            }
        }
        public UIPanel CurrentPanel()
        {
            if (_backstack.Count > 0)
            {
                return _backstack.Peek();
            }

            return null;
        }
        //---------------------------------------------------

        //---------------------------------------------------
        public T OpenWindow<T>(params object[] datas) where T : UIWindow, new()
        {
            var curpanel = CurrentPanel();
            if (curpanel != null)
            {
                return curpanel.OpenWindow<T>(datas);
            }

            return null;
        }

        public void CloseWindow<T>()
        {
            var curpanel = CurrentPanel();
            if (curpanel != null)
            {
                curpanel.CloseWindow<T>();
            }
        }

        public void ClearAll()
        {
            //panel
            _UIlist.Clear();

            _backstack.Clear();
        }
    }
}

