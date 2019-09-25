using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    public class LeftDirectTile : InputTile
    {
        protected override void OnClick()
        {
            base.OnClick();

            if(CanClick())
            {
                Debug.Log("OnClick");

                _battlefield.Field.Move(MoveDirect.Left, X, Y, 1);

                _battlefield.Field.EliminationUpdate();
            }

        }
    }
}

