#region 描述
//-----------------------------------------------------------------------------
// 类 名 称: EventTable
// 作    者：zhangfan
// 创建时间：2019/8/28 11:27:40
// 描    述：
// 版    本：
//-----------------------------------------------------------------------------
// Copyright (C) 2017-2019 零境科技有限公司
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Bunker.Module
{
    public class ModuleEvent : UnityEvent<object>
    {

    }

    public class ModuleEvent<T> : UnityEvent<T>
    {

    }

    public class EventTable
    {
        Dictionary<string, ModuleEvent> _EventList;

        public ModuleEvent GetEvent(string name)
        {
            if (_EventList == null)
            {
                _EventList = new Dictionary<string, ModuleEvent>();
            }

            if (!_EventList.ContainsKey(name))
            {
                _EventList.Add(name, new ModuleEvent());
            }

            return _EventList[name];
        }

        public void Clear()
        {
            if(_EventList == null)
            {
                return;
            }

            foreach(var e in _EventList)
            {
                e.Value.RemoveAllListeners();
            }
            _EventList.Clear();
        }
    }
}
