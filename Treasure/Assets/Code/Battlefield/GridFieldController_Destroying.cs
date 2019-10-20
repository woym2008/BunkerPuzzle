using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class GridFieldController_Destroying : GridFieldControllerBase
    {
        override public string ControllerType
        {
            get
            {
                return "Destroying";
            }
        }
        /*
        public override bool CanMove()
        {
            return true;
        }
        */
    }
}

