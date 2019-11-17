using UnityEngine;
using System.Collections;
using Bunker.Module;
using System;
using Bunker.Process;
using DG.Tweening;

namespace Bunker.Game
{
    public class RobotBase : MonoBehaviour
    {
        public float unitTime = 1f;
        public int moveStepMax = 1;
        public int state = IDLE;
        //
        public const int IDLE = 0;
        public const int PREPARE = 1;
        public const int WALK = 2;
        public const int END = 3;

        //
        private int _moveStep;
        private Vector3Int _dir;

        public virtual void OnInit()
        {

        }

        public void OnPrepareMove()
        {
            if (state == IDLE)
            {
                state = PREPARE;
                _moveStep = moveStepMax; 
                _dir = FindWay();
            }
        }

        public virtual void OnStartMove()
        {
            state = WALK;
            transform.DOMove(transform.position + _dir, unitTime).OnComplete(OnFinishMove);
        }

        public virtual void OnFinishMove()
        {
            state = END;
            _moveStep = 0;
            ModuleManager.getInstance.SendMessage("Bunker.Game.RobotManagerModule", "NextRobot");
        }

        public void SetIdle()
        {
            state = IDLE;
        }

        public int GetState()
        {
            return state;
        }
        //这个非常简单，只需要向目的地靠近就好
        /// <summary>
        /// 
        /// </summary>
        /// <returns>返回一个方向</returns>
        public virtual Vector3Int FindWay()
        {
            return Vector3Int.zero;
        }
    }
}
