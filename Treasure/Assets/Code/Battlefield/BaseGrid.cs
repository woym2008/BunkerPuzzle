using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    abstract public class BaseGrid : IGridObject
    {
        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }

        protected int _x;
        protected int _y;
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

        ~BaseGrid()
        {
            //OnDestroy();
        }


        public void CreateGrid(string name, Vector3 zeropos, int x = 0, int y = 0)
        {
            _name = name;

            var prefab = Resources.Load("Prefabs/Tiles/" + name) as GameObject;
            _object = GameObject.Instantiate(prefab);
            _x = x;
            _y = y;
            _zeropos = zeropos;
            var selfpos = _zeropos + new Vector3(_x*Constant.TileSize.x, -_y * Constant.TileSize.y, 0);

            _object.transform.position = selfpos;

            Init();
        }

        public void Delete()
        {
            OnDestroy();
        }

        public void OnDestroy()
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
            if (_object.transform.childCount > 0)
            {
                var kid = _object.transform.GetChild(0);
                var sr = kid.GetComponent<SpriteRenderer>();
                sr.sortingOrder = _y;
            }
        }

        private void SetPos(int x, int y)
        {
            _x = x;
            _y = y;
        }

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
                gridctrl.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = startY;
                gridctrl?.MoveToPosition(targetpos, movetime);
            }
        }

        virtual public void UpdateGrid(int x, int y)
        {
            if(_object != null)
            {
                SetPos(x, y);
                var selfpos = _zeropos + new Vector3(x * Constant.TileSize.x, -y * Constant.TileSize.y, 0);
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

        virtual public bool CanEliminationByOther(string gridtype)
        {
            Debug.LogFormat("相邻的{0}块要消除，我的类型是{1}，判断能否消除", gridtype, GetGridType());
            return true;
        }
    }
}

