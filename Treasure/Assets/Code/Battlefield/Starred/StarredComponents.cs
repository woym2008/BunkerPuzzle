using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using Bunker.Process;
using DG.Tweening;

namespace Bunker.Game
{
    public enum StarredComponentTypes
    {
        Starred_Renderer,
        Starred_BaseMovement,
    }

    public abstract class StarredComponent : ITurn
    {
        public bool Enable { set; get; }
        public StarredTurn starredTurn;
        public StarringRole starredRole;
        public Transform transform { get { return starredRole.transform; } }
        public StarredComponent(StarredTurn st)
        {
            starredTurn = st;
        }
        public virtual void OnAdd()
        {
            starredRole = starredTurn.starredRoleObj.GetComponent<StarringRole>();
        }
        public virtual void OnRemove()
        {

        }
        public virtual void OnUpdateTurn()
        {
            if (!Enable) return;
        }
        public virtual void OnStartTurn()
        {
            if (!Enable) return;
        }
        public virtual void OnEndTurn()
        {
            if (!Enable) return;
        }
    }

    public class SC_StarredRenderer : StarredComponent
    {
        Animator animator;
        Animator dead_effect;
        public SC_StarredRenderer(StarredTurn st) : base(st)
        {

        }
        //
        public override void OnAdd()
        {
            base.OnAdd();
            //得到控制组件
            animator = starredRole.transform.Find("PorterSprite").GetComponent<Animator>();
            dead_effect = starredRole.transform.Find("PorterSprite/dead_effect").GetComponent<Animator>();
            //
        }
        //这里应该是调用各个动画片段的的
        public void DoWalk()
        {

        }
        public void DoShot()
        {

        }
        public void DoDead()
        {

        }

    }

    public class SC_BaseMovement : StarredComponent
    {
        public SC_BaseMovement(StarredTurn st) : base(st)
        {

        }

        protected Grid          _curNode;
        protected Vector2Int    pos { get { return new Vector2Int(_curNode.ColID, _curNode.RowID); } }

        public const int IDLE = 0;
        public const int PREPARE = 1;
        public const int WALK = 2;
        public const int END = 3;

        public int state = IDLE;
        public Vector3[] _path;

        //
        public override void OnRemove()
        {
            base.OnRemove();
            transform.DOKill();
        }
        //


        public void UpdateCurNode()
        {
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var tile = m.Field.FindTileObject(transform.parent.gameObject);
            SetToGird(tile.ParentGrid);
        }

        public void SetToGird(Grid grid)
        {
            if (grid.AttachTile == null)
            {
                return;
            }
            _curNode = grid;
            transform.parent = grid.AttachTile.Node.transform;
            transform.localPosition = Vector3.zero;
            starredRole.SetSpriteSortingOrder(_curNode.RowID * 2 + 1);
        }

        public void OnStartMove()
        {
            if (state == WALK) return;

            state = WALK;

            if (_path == null)
            {
                OnFinishMove();
            }
            else
            {
                //transform.DOMove(transform.position + _dir, unitTime).OnComplete(OnFinishMove);
                transform.DOPath(_path, _path.Length * 0.2f).OnComplete(OnFinishMove);
            }
        }
        //
        public void SetPath(Vector2Int[] path)
        {
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            _path = new Vector3[path.Length];
            for(int i = 0 ; i<path.Length ; ++i)
            {
                var t = m.Field.GetTile(path[i].x, path[i].y);
                //此处得到的t，应该是有tile的，因为路径都是可行走区域，不再做判断了
                _path[i] = t.Node.transform.position;
            }
        }
        public void SetPath(Vector2Int next_node)
        {
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var t = m.Field.GetTile(next_node.x, next_node.y);
            _path = new Vector3[] { t.Node.transform.position };
        }
        public void SetPath(Vector3[] path)
        {
            _path = path;
        }
        //
        public virtual void OnFinishMove()
        {
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var g = m.Field.GetGrid(transform.position);
            if (g != null) SetToGird(g);
            state = END;
            _path = null;   //清除一下path
            starredTurn.OnFinishMovement();
        }

    }

}
