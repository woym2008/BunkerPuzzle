using UnityEngine;
using System.Collections;
using Bunker.Module;
using System.Collections.Generic;

public class MaskTileModule : LogicModule
{
    public Stack<MaskTile> _maskTileCache = new Stack<MaskTile>();

    public MaskTileModule() : base(typeof(MaskTileModule).ToString())
    {

    }
    public MaskTileModule(string name) : base(name)
    {

    }

    public override void Create()
    {
        base.Create();
    }

    public override void OnStart(params object[] data)
    {
        base.OnStart(data);

        _maskTileCache.Clear();
    }

    public override void OnStop()
    {
        foreach(var masktile in _maskTileCache)
        {
            GameObject.Destroy(masktile.gameObject);
        }
        _maskTileCache.Clear();

        base.OnStop();
    }

    public MaskTile GetMaskTile()
    {
        MaskTile retMaskTile = null;
        if (_maskTileCache.Count == 0)
        {
            retMaskTile = LoadTile();
        }
        else
        {
            retMaskTile = _maskTileCache.Pop();
        }
        

        retMaskTile.gameObject.SetActive(true);

        return retMaskTile;
    }

    public void SetMaskTile(MaskTile mt)
    {
        mt.gameObject.SetActive(false);

        _maskTileCache.Push(mt);
    }

    MaskTile LoadTile()
    {
        var prefab = GameObject.Instantiate(Resources.Load("Prefabs/MaskTiles/MaskTile")) as GameObject;

        var maskTile = prefab.GetComponent<MaskTile>();
        if(maskTile == null)
        {
            maskTile = prefab.AddComponent<MaskTile>();
        }

        return maskTile;
    }
}
