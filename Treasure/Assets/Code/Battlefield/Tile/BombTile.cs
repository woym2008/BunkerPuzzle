using UnityEngine;
using System.Collections;
using Bunker.Game;

public class BombTile : InputTile
{
    protected override int TileSize {
        get { return 1; }
    }

    public override bool CanMove()
    {
        return true;
    }

    public override bool CanWalk()
    {
        return false;
    }

    protected override void OnClick()
    {
        base.OnClick();
    }
}
