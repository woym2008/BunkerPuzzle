using UnityEngine;
using System.Collections;

public static class MonoBehaviourHelper
{
    static public CommonMonoBehaviour CommonObject;

    static public CommonMonoBehaviour CreateObject()
    {
        var obj = new GameObject();

        var cmb = obj.AddComponent<CommonMonoBehaviour>();

        return cmb;
    }

    static public CommonMonoBehaviour GetCommonObject()
    {
        if(CommonObject == null)
        {
            CommonObject = CreateObject();
        }

        return CommonObject;
    }

    static public Coroutine StartCoroutine(IEnumerator routine)
    {
        var obj = GetCommonObject();

        return obj.StartCoroutine(routine);
    }
}
