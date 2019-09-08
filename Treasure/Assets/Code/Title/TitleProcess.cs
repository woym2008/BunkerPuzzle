#region 描述
//-----------------------------------------------------------------------------
// 类 名 称: TitleProcess
// 作    者：zhangfan
// 创建时间：2019/8/28 17:15:10
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
using Bunker.Process;

public class TitleProcess : BasicProcess
{
    public override void Create()
    {
        base.Create();
        //加载资源
    }

    public override void StartProcess(params object[] args)
    {
        base.StartProcess(args);
        //显示ui
    }

    public override void EndProcess()
    {
        base.EndProcess();
    }

    public override void Release()
    {
        base.Release();
    }
}
