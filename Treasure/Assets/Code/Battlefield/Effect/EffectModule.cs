using Bunker.Module;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectModule : LogicModule
{
    Dictionary<string, Stack<GameObject>> _effectsCache;

    public EffectModule() : base(typeof(EffectModule).ToString())
    {

    }
    public EffectModule(string name) : base(name)
    {

    }

    public override void Create()
    {
        base.Create();

        _effectsCache = new Dictionary<string, Stack<GameObject>>();
    }

    public override void OnStart(params object[] data)
    {
        base.OnStart(data);

        _effectsCache = new Dictionary<string, Stack<GameObject>>();
    }

    public override void OnStop()
    {
        base.OnStop();

        ClearEffects();
    }
    //-------------------------------------------------------------------
    //private
    //-------------------------------------------------------------------
    void ClearEffects()
    {
        foreach(var objs in _effectsCache)
        {
            while(objs.Value.Count > 0)
            {
                var obj = objs.Value.Pop();
                if(obj != null)
                {
                    GameObject.Destroy(obj);
                }
            }
        }

        _effectsCache.Clear();
    }

    GameObject GetEffectObject(string name)
    {
        if(_effectsCache.ContainsKey(name))
        {
            var currenteffects = _effectsCache[name];
            if(currenteffects.Count > 0)
            {
                return currenteffects.Pop();
            }
        }

        return null;
    }
    //-------------------------------------------------------------------
    //public 
    //-------------------------------------------------------------------
    const string bastpath = "Prefabs/Effect";
    public EffectController CreateEffect(string name, Vector3 pos, Quaternion rot)
    {
        var obj = GetEffectObject(name);
        if(obj == null)
        {
            obj = GameObject.Instantiate(Resources.Load(string.Format("{0}/{1}", bastpath, name))) as GameObject;
        }
        var effectcontroller = obj.GetComponent<EffectController>();
        if (effectcontroller == null)
        {
            effectcontroller = obj.AddComponent<EffectController>();
        }
        effectcontroller.module = this;
        effectcontroller.type = name;

        obj.SetActive(true);

        obj.transform.position = pos;
        obj.transform.rotation = rot;

        return effectcontroller;
    }

    public void ReleaseEffect(string name, GameObject obj)
    {
        if(!_effectsCache.ContainsKey(name))
        {
            _effectsCache.Add(name, new Stack<GameObject>());
        }
        var effects = _effectsCache[name];
        obj.SetActive(false);
        effects.Push(obj);
    }
}
