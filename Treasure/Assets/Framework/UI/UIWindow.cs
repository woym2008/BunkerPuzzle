#region 描述
//-----------------------------------------------------------------------------
// 类 名 称: UIWindow
// 作    者：zhangfan
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIWindow
{
    protected Transform _transform;
    public void OnInit(string name, Transform parent = null)
    {
        var obj = GameObject.Instantiate(Resources.Load("UI/window/" + name)) as GameObject;
        _transform = obj.transform;
        //_transform.parent = parent;
    }

    virtual public void OnOpen(params object[] datas)
    {
        _transform.gameObject.SetActive(true);
    }

    virtual public void OnClose()
    {
        _transform.gameObject.SetActive(false);
    }
}
