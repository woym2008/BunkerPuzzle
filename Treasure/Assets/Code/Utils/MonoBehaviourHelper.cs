using UnityEngine;
using System.Collections;

public static class MonoBehaviourHelper
{
    static public CommonMonoBehaviour CreateObject()
    {
        var obj = new GameObject();

        var cmb = obj.AddComponent<CommonMonoBehaviour>();

        return cmb;
    }
}
