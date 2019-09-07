#region 描述
//-----------------------------------------------------------------------------
// 类 名 称: BasicProcess
// 作    者：zhangfan
// 创建时间：2019/8/28 17:06:35
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
using UnityEngine;

namespace Bunker.Process
{
    public abstract class BasicProcess
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

        public virtual void Create()
        {
            Debug.Log("Create()");
        }

        public virtual void Release()
        {
            Debug.Log("Release()");
        }

        public virtual void StartProcess(params object[] args)
        {
            Debug.Log("StartProcess() args:{0}" + args);
        }

        public virtual void EndProcess()
        {
            Debug.Log("EndProcess() args:{0}");
        }
    }
}
