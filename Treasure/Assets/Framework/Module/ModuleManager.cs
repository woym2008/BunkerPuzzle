#region 描述
//-----------------------------------------------------------------------------
// 类 名 称: ModuleManager
// 作    者：zhangfan
// 创建时间：2019/8/28 11:39:04
// 描    述：
// 版    本：
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bunker.Module
{
    public class ModuleManager : ServicesModule<ModuleManager>
    {
        class MessageObject
        {
            public string target;
            public string msg;
            public object[] args;

        }
        Dictionary<string, LogicModule> _modules;

        private Dictionary<string, EventTable> _listenEvents;

        private Dictionary<string, List<MessageObject>> _cacheMessages;

        public ModuleManager()
        {
            _modules = new Dictionary<string, LogicModule>();
            _listenEvents = new Dictionary<string, EventTable>();
            _cacheMessages = new Dictionary<string, List<MessageObject>>();
        }
        //-----------------------------------------
        private EventTable GetPreEventTable(string target)
        {
            EventTable table = null;
            if (!_listenEvents.ContainsKey(target))
            {
                table = new EventTable();
                _listenEvents.Add(target, table);
            }
            else
            {
                table = _listenEvents[target];
            }
            return table;
        }

        public ModuleEvent Event(string target, string evttype)
        {
            ModuleEvent evt = null;
            var module = GetModule(target);

            if(module!=null)
            {
                evt = module.Event(evttype);
            }
            else
            {
                var table = GetPreEventTable(target);
                evt = table.GetEvent(evttype);
            }

            return evt;
        }
        //-----------------------------------------
        private List<MessageObject> GetCacheMessageList(string target)
        {
            List<MessageObject> list = null;
            if (!_cacheMessages.ContainsKey(target))
            {
                list = new List<MessageObject>();
                _cacheMessages.Add(target, list);
            }
            else
            {
                list = _cacheMessages[target];
            }
            return list;
        }

        public LogicModule GetModule(string name)
        {
            if (_modules.ContainsKey(name))
            {
                return _modules[name];
            }

            return null;
        }
        public T GetModule<T>() where T : LogicModule
        {
            var name = typeof(T).ToString();
            if (_modules.ContainsKey(name))
            {
                return _modules[name] as T;
            }

            return default(T);
        }

        public void SendMessage(string target, string msg, params object[] datas)
        {
            if(_modules.ContainsKey(target))
            {
                _modules[target].HandleMessage(msg, datas);
            }
            else
            {
                var cache = GetCacheMessageList(target);
                var m = new MessageObject();
                m.msg = msg;
                m.target = target;
                m.args = datas;
                cache.Add(m);
            }
        }
        //-----------------------------------------
        string _domain = "Bunker.Game";
        public void SetDomain(string str)
        {
            _domain = str;
        }

        public T CreateModule<T>() where T: LogicModule
        {
            return CreateModule(typeof(T).ToString()) as T;
        }
        
        public LogicModule CreateModule(string name)
        {
            LogicModule module = null;
            //Type type = Type.GetType(string.Format("{0}.{1}", _domain, name));
            Type type = Type.GetType(name);
            if (type != null)
            {
                var m = Activator.CreateInstance(type);
                module = m as LogicModule;
            }

            _modules.Add(name, module);

            if (_listenEvents.ContainsKey(name))
            {
                EventTable mgrEvent = null;
                mgrEvent = _listenEvents[name];
                _listenEvents.Remove(name);

                module.SetEventTable(mgrEvent);
            }

            module.Create();

            if (_cacheMessages.ContainsKey(name))
            {
                List<MessageObject> list = _cacheMessages[name];
                for (int i = 0; i < list.Count; i++)
                {
                    MessageObject msgobj = list[i];
                    module.HandleMessage(msgobj.msg, msgobj.args);
                }
                _cacheMessages.Remove(name);
            }

            return module;
        }


        public void ReleaseModule(LogicModule module)
        {
            if (module != null)
            {
                if (_modules.ContainsKey(module.Name))
                {
                    _modules.Remove(module.Name);
                    module.Release();
                }
                else
                {
                    Debug.Log("ReleaseModule() dont has module");
                }
            }
            else
            {
                Debug.Log("ReleaseModule() module = null!");
            }
        }

        public void ReleaseAll()
        {
            foreach (var e in _listenEvents)
            {
                e.Value.Clear();
            }
            _listenEvents.Clear();

            _cacheMessages.Clear();

            foreach (var module in _modules)
            {
                module.Value.Release();
            }
            _modules.Clear();
        }
        //-----------------------------------------
        public void StartModule<T>(params object[] data) where T : LogicModule
        {
            var module = GetModule(typeof(T).ToString());

            if(module != null)
            {
                module.OnStart(data);
            }
        }

        public void StopModule<T>() where T : LogicModule
        {
            var module = GetModule(typeof(T).ToString());

            if (module != null)
            {
                module.OnStop();
            }
        }
        //-----------------------------------------

    }
}
