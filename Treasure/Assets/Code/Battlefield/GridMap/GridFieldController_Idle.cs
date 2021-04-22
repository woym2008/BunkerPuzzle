using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class GridFieldController_Idle : GridFieldControllerBase
    {
        override public string ControllerType
        {
            get
            {
                return "Idle";
            }
        }

        public override bool CanWaitInReadyStack
        {
            get
            {
                return false;
            }
        }
        /*
        override public void Move(MoveDirect dir, int gridx, int gridy, int offsetValue)
        {
        GridFieldController_Moving gfc = _gridfield.SwitchController<GridFieldController_Moving>();

        gfc.Move(dir, gridx, gridy, offsetValue);
        }

        public override bool CanMove()
        {
        return true;
        }
        */
    }
}
