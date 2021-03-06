﻿using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    public class LeftDirectTile : InputTile
    {
        override protected int TileSize {
            get { return 2; }
        }
        protected override void OnClick()
        {
            base.OnClick();

            if(CanClick())
            {
                Debug.Log("OnClick");

                //_battlefield.Field.Move(MoveDirect.Left, X, Y, 1);
                _battlefield.UseController<GridFieldController_Moving>(MoveDirect.Left, ParentGrid.ColID, ParentGrid.RowID, 1);
            }

        }
    }
}

