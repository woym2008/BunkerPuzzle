using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Bunker.Module
{
    public class LogicModule : BasicModule
    {
        private string m_name;
        public string Name
        {
            get
            {
                if (m_name == null)
                {
                    m_name = this.GetType().Name;
                }
                return m_name;
            }
        }

        EventTable _eventTable;

        LogicModule()
        {
        
        }

        internal LogicModule(string name)
        {
            m_name = name;
        }

        public ModuleEvent Event(string type)
        {
            return GetEventTable().GetEvent(type);
        }

        internal void SetEventTable(EventTable msgevent)
        {
            _eventTable = msgevent;
        }

        protected EventTable GetEventTable()
        {
            if (_eventTable == null)
            {
                _eventTable = new EventTable();
            }
            return _eventTable;
        }

        internal void HandleMessage(string msg, object[] args)
        {
            MethodInfo mi = this.GetType().GetMethod(msg, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (mi != null)
            {
                mi.Invoke(this, BindingFlags.NonPublic, null, args, null);
            }
            else
            {
                OnModuleMessage(msg, args);
            }
        }

        protected virtual void OnModuleMessage(string msg, object[] args)
        {
            Debug.Log("OnModuleMessage" + msg);
        }


        virtual public void Create()
        {
            ;
        }

        virtual public void OnStart(params object[] data)
        {

        }

        virtual public void OnStop()
        {

        }
    }
}

