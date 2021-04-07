using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class UpDirectTile : InputTile
    {
        override protected int TileSize {
            get { return 2; }
        }
        protected override void OnClick()
        {
            base.OnClick();

            if (CanClick())
            {
                Debug.Log("UpDirectTile OnClick");

                //_battlefield.Field.Move(MoveDirect.Up, X, Y, 1);
                _battlefield.UseController<GridFieldController_Moving>(MoveDirect.Up, ParentGrid.ColID, ParentGrid.RowID, 1);
            }

        }
    }
}
