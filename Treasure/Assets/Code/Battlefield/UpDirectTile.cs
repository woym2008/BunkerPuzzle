using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class UpDirectTile : InputTile
    {
        protected override void OnClick()
        {
            base.OnClick();

            Debug.Log("OnClick");

            _battlefield.Field.Move(MoveDirect.Up, X, Y, 1);
        }
    }
}
