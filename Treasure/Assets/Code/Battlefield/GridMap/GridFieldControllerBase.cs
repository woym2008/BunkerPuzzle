using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public abstract class GridFieldControllerBase
    {
        protected GridField _gridfield;

        protected object[] _cacheobjs;

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

        /// <summary>
        /// 这个controller 是不是执行后可以在等待队列中等待(这个操作是不是必须是一个立即执行的操作)
        /// </summary>
        /// <returns><c>true</c>, if wait in ready stack was caned, <c>false</c> otherwise.</returns>
        abstract public bool CanWaitInReadyStack
        {
            get;
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
        virtual public void Init(params object[] objs)
        {
            _cacheobjs = objs;
        }
        virtual public void Excute()
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

