#region 描述
//-----------------------------------------------------------------------------
// 类 名 称: ProcessManager
// 作    者：zhangfan
// 创建时间：2019/8/28 17:04:09
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
using Bunker.Module;
using UnityEngine;

namespace Bunker.Process
{
    public class ProcessManager : ServicesModule<ProcessManager>
    {
        Dictionary<string, BasicProcess> _processes;
        string domain = "Limo.Process";
        BasicProcess _currectProcess = null;
        public void Switch<T>(params object[] args) where T : BasicProcess
        {
            if(_processes != null)
            {
                _processes = new Dictionary<string, BasicProcess>();
            }
            //---------------------------------------------------- 
            var name = typeof(T).ToString();
            var process = GetProcess(name);
            if(process != null)
            {
                //close old
                if (_currectProcess != null)
                {
                    _currectProcess.EndProcess();
                }
                //----------------------------------------------------
                //open new
                process.StartProcess();

                _currectProcess = process;
            }
        }

        private BasicProcess GetProcess(string name)
        {
            BasicProcess process = null;
            if (!_processes.ContainsKey(name))
            {
                process = Activator.CreateInstance(Type.GetType(string.Format("{0}.{1}", domain, name))) as BasicProcess;
                if(process == null)
                {
                    Debug.LogError("Not Find Process: " + name);
                }
                process.Create();
                _processes.Add(name, process);
            }
            process = _processes[name];

            return process;
        }

        public void ReleaseProcess<T>()
        {
            var processName = typeof(T).ToString();
            var process = GetProcess(processName);
            if (process != null)
            {
                if (_processes.ContainsKey(process.Name))
                {
                    Debug.Log("ReleaseProcess() name = " + process.Name);
                    _processes.Remove(process.Name);
                    process.Release();
                }
                else
                {
                    Debug.Log("ReleaseProcess() 流程不是由ProcessManager创建的！ name = " + process.Name);
                }
            }
            else
            {
                Debug.Log("ReleaseProcess() process = null!");
            }
        }

        public void ReleaseAll()
        {
            if(_currectProcess != null)
            {
                _currectProcess = null;
            }
            
            foreach (var process in _processes)
            {
                process.Value.Release();
            }
            _processes.Clear();
        }
    }
}
