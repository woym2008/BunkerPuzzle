using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    public class RightDirectTile : InputTile
    {
        override protected int TileSize {
            get { return 2; }
        }
        protected override void OnClick()
        {
            base.OnClick();

            if (CanClick())
            {
                Debug.Log("RightDirectTile OnClick");

                _battlefield.UseController<GridFieldController_Moving>(MoveDirect.Right, ParentGrid.ColID, ParentGrid.RowID, -1);
            }

        }
    }
}