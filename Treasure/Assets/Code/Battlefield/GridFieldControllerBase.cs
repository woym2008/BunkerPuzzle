using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public abstract class GridFieldControllerBase
    {
        protected GridField _gridfield;

        public GridFieldControllerBase()
        {
        }

        abstract public string ControllerType
        {
            get;
        }

        public void SetGridField(GridField gf)
        {
            _gridfield = gf;
        }
        /*
        virtual public void Update(float dt)
        {

        }

        virtual public void Move(MoveDirect dir, int gridx, int gridy, int offsetValue)
        {

        }

        virtual public bool CanMove()
        {
            return false;
        }*/
        //---------------------------------
        virtual public void Excute(params object[] objs)
        {
            ;
        }
        virtual public bool IsFinish()
        {
            return false;
        }
        //---------------------------------
    }
}

