using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    abstract public class BaseTile : ITile
    {
        public Grid ParentGrid { get; set; }

        protected Vector3 _zeropos;

        protected GameObject _object;
        protected GameObject _copyobject;

        public GameObject Node
        {
            get
            {
                return _object;
            }
        }
        
        string _name;

        private bool _isUpdated = false;

        ~BaseTile()
        {
            //OnDestroy();
        }


        public void Create(string name, Vector3 zeropos, Grid grid)
        {
            _name = name;

            ParentGrid = grid;

            var prefab = Resources.Load("Prefabs/Tiles/" + name) as GameObject;
            _object = GameObject.Instantiate(prefab);

            _zeropos = zeropos;
            var selfpos = GetSelfWorldPos();

            _object.transform.position = selfpos;

            Init();
        }

        public Vector3 GetSelfWorldPos()
        {
            return _zeropos + new Vector3(ParentGrid.ColID * Constant.TileSize.x, -ParentGrid.RowID * Constant.TileSize.y, 0);
        }

        public void Delete()
        {
            OnDestroy();
        }

        public virtual void OnDestroy()
        {
            if(_object != null)
            {
                GameObject.Destroy(_object);
                _object = null;
            }
        }

        public virtual void Init()
        {
            UpdateSortingOrder();
        }

        public void UpdateSortingOrder()
        {
            //add by wwh
            if (_object!= null && _object.transform.childCount > 0)
            {
                var kid = _object.transform.GetChild(0);
                var sr = kid.GetComponent<SpriteRenderer>();
                sr.sortingOrder = ParentGrid.RowID * 2;
            }
        }

        //private void SetPos(int x, int y)
        //{
        //    _x = x;
        //    _y = y;
        //}

        virtual public bool CanMove()
        {
            return true;
        }

        virtual public void MoveTo(int x, int y, float movetime)
        {
            if(_object != null)
            {
                var targetpos = _zeropos + new Vector3(x * Constant.TileSize.x, -y * Constant.TileSize.y, 0);

                var gridctrl = _object.gameObject.GetComponent<GridMotionController>();
                gridctrl?.MoveToPosition(targetpos,movetime);
            }
        }
        virtual public void CopyMoveTo(int startX, int startY, int endX, int endY, float movetime)
        {
            if (_object != null)
            {
                var currentpos = _zeropos + new Vector3(startX * Constant.TileSize.x, -startY * Constant.TileSize.y, 0);
                var targetpos = _zeropos + new Vector3(endX * Constant.TileSize.x, -endY * Constant.TileSize.y, 0);

                _copyobject = GameObject.Instantiate(_object) as GameObject;

                var gridctrl = _copyobject.gameObject.GetComponent<GridMotionController>();
                gridctrl.transform.position = currentpos;
                gridctrl.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = startY * 2;
                gridctrl?.MoveToPosition(targetpos, movetime);
            }
        }

        virtual public void UpdateGrid(Grid grid)
        {
            if(_object != null)
            {
                //SetPos(x, y);
                grid.AttachTile = this;
                this.ParentGrid = grid;
                var selfpos = _zeropos + new Vector3(grid.ColID * Constant.TileSize.x, -grid.RowID * Constant.TileSize.y, 0);
                _object.transform.position = selfpos;
            }
            if(_copyobject != null)
            {
                GameObject.Destroy(_copyobject);
                _copyobject = null;
            }
        }

        virtual public string GetGridType()
        {
            return "";
        }

        virtual public bool CanElimination()
        {
            return false;
        }

        virtual public void Elimination()
        {

        }

        virtual public void OnEliminationed()
        {
            Debug.LogFormat("我是{0}块，我被消除了", GetGridType());
        }

        virtual public bool CanEliminationByOther(string gridtype,int direct)
        {
            Debug.LogFormat("相邻的{0}块要消除，我的类型是{1}，判断能否消除", gridtype, GetGridType());
            return (gridtype == GetGridType());
        }
        //-------------------------------------------------------------
        //小人行走相关函数
        virtual public bool CanWalk()
        {
            return false;
        }
        //-------------------------------------------------------------
    }
}

