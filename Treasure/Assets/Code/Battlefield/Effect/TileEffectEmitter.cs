using UnityEngine;
using System.Collections;
using Bunker.Game;
using Bunker.Module;

public class TileEffectEmitter
{
    EffectModule _effectmodule;

    EffectController[] _effects;

    public void StartEmitter(string type, BaseTile[] tiles)
    {
        if(_effectmodule == null)
        {
            _effectmodule = ModuleManager.getInstance.GetModule<EffectModule>();
        }

        _effects = new EffectController[tiles.Length];

        for(var i = 0; i<tiles.Length; ++i)
        {
            var pos = tiles[i].GetSelfWorldPos();

            _effects[i] = _effectmodule.CreateEffect(type, pos, Quaternion.identity);

            _effects[i].renderer.sortingOrder = tiles[i].GetSortingOrder() + 1;
        }
    }

    public void CloseEmitter()
    {
        for (var i = 0; i < _effects.Length; ++i)
        {
            _effects[i].Release();
        }
    }
}
