using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class DownDirectTile : InputTile
    {
        protected override void OnClick()
        {
            base.OnClick();

            if (CanClick())
            {
                Debug.Log("DownDirectTile OnClick");

                _battlefield.UseController<GridFieldController_Moving>(MoveDirect.Down, ParentGrid.ColID, ParentGrid.RowID, -1);
            }

        }

        public override void UpdateGrid(Grid grid)
        {
            base.UpdateGrid(grid);
        }
    }
}
