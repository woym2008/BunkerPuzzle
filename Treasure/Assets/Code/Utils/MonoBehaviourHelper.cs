using UnityEngine;
using System.Collections;

public static class MonoBehaviourHelper
{
    static public CommonMonoBehaviour CommonObject;

    static public CommonMonoBehaviour CreateObject(string obj_name)
    {
        CommonMonoBehaviour cmb = null;
        var go = GameObject.Find(obj_name);
        if (go != null)
        {
            cmb = go.GetComponent<CommonMonoBehaviour>();

            if(cmb != null)
            {
                return cmb;
            }
        }
        else
        {
            go = new GameObject();
        }

        go.name = obj_name;

        cmb = go.AddComponent<CommonMonoBehaviour>();

        return cmb;
    }

    static public CommonMonoBehaviour GetCommonObject()
    {
        if(CommonObject == null)
        {
            CommonObject = CreateObject("CommonObject");
        }

        return CommonObject;
    }

    static public Coroutine StartCoroutine(IEnumerator routine)
    {
        var obj = GetCommonObject();

        return obj.StartCoroutine(routine);
    }
}
