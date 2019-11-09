using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    public class RightDirectTile : InputTile
    {
        protected override void OnClick()
        {
            base.OnClick();

            if (CanClick())
            {
                Debug.Log("RightDirectTile OnClick");

                _battlefield.UseController<GridFieldController_Moving>(MoveDirect.Right, X, Y, -1);
            }

        }
    }
}