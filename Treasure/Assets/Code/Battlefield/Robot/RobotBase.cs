﻿using UnityEngine;
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
        protected Vector3Int _dir;
        protected Vector2Int _pos;
        protected Grid _curNode;

        private void OnDestroy()
        {
            transform.DOKill();
        }

        public Vector2Int GetGridXY()
        {
            return new Vector2Int(_curNode.ColID, _curNode.RowID);
        }


        public virtual void OnInit()
        {

        }

        public virtual void OnPrepareMove()
        {          
            state = PREPARE;
            _moveStep = moveStepMax;
            //此处更新一下当前的格子位置
            UpdateCurNode();
            //
            _dir = FindWay();        
        }

        public virtual void OnStartMove()
        {
            state = WALK;
            if(_dir == Vector3Int.zero)
            {
                OnFinishMove();
            }
            else
            {
                transform.DOMove(transform.position + _dir, unitTime).OnComplete(OnFinishMove);
            }
        }

        public virtual void OnFinishMove()
        {
            state = END;
            _moveStep = 0;
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var g = m.Field.GetGrid(transform.position);
            if(g != null)
                SetToGird(g);
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

        public Vector2Int GetDestination()
        {
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            return m.Field.ClampGridPos(_pos.x + _dir.x, _pos.y - _dir.y);
        }

        public void SetToGird(Grid grid)
        {
            if(grid.AttachTile == null)
            {
                return;
            }
            _curNode = grid;
            _pos = new Vector2Int(grid.ColID, grid.RowID);
            transform.parent = grid.AttachTile.Node.transform;
            transform.localPosition = Vector3.zero;
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var sr = GetComponentInChildren<SpriteRenderer>();
            sr.sortingOrder = _curNode.RowID * 2 + 1;
        }

        public void UpdateCurNode()
        {
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var tile = m.Field.FindTileObject(transform.parent.gameObject);
            SetToGird(tile.ParentGrid);
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
        //是否可以被其他bot穿透through
        public bool CanPenetrable()
        {
            return gameObject.name.EndsWith(Constant.CAN_WALK_SUFFIX);
        }
        public void SetPenetrable(bool b)
        {
            if (b == CanPenetrable()) return;
            if (b)
            {
                gameObject.name = gameObject.name + Constant.CAN_WALK_SUFFIX;
            }
            else
            {
                gameObject.name = gameObject.name.Replace(Constant.CAN_WALK_SUFFIX, "");
            }
        }
    }
}
