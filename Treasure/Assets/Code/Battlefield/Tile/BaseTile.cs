using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    abstract public class BaseTile : ITile
    {
        public Grid ParentGrid { get; set; }

        protected Vector3 _zeropos;

        protected GameObject _object;
        protected GameObject _copyobject_ori;
        protected GameObject _copyobject_target;
        protected MaskTile _maskobject_ori;
        protected MaskTile _maskobject_copy;

        public GameObject Node
        {
            get
            {
                return _object;
            }
        }

        protected abstract int TileSize {
            get;
        }
        
        string _name;

        private bool _isUpdated = false;

        MaskTileModule _maskModule;

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

            _maskModule = ModuleManager.getInstance.GetModule<MaskTileModule>();

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

        virtual public void MoveTo(int x, int y, float movetime, bool usecopy = false)
        {
            var targetpos = _zeropos + new Vector3(x * Constant.TileSize.x, -y * Constant.TileSize.y, 0);

            if (_object == null)
            {
                return;
            }
            if (usecopy)
            {
                _copyobject_ori = GameObject.Instantiate(Resources.Load("Prefabs/MaskTiles/" + _name)) as GameObject;
                _copyobject_ori.SetActive(true);
                var gridctrl_ori = _copyobject_ori.gameObject.GetComponent<GridMotionController>();
                if (gridctrl_ori == null) gridctrl_ori = _copyobject_ori.gameObject.AddComponent<GridMotionController>();
                gridctrl_ori.transform.position = _object.transform.position;
                gridctrl_ori.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = _object.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder;
                gridctrl_ori?.MoveToPosition(targetpos, movetime);
                _object?.SetActive(false);
                return;
            }
            var gridctrl = _object.gameObject.GetComponent<GridMotionController>();
            gridctrl?.MoveToPosition(targetpos, movetime);
        }
        virtual public void CopyMoveTo(int currentTargetX, int currentTargetY ,int startX, int startY, int endX, int endY, float movetime)
        {
            if (_object != null)
            {
                var currentTargetPos = _zeropos + new Vector3(currentTargetX * Constant.TileSize.x, -currentTargetY * Constant.TileSize.y, 0);

                var copycurrentpos = _zeropos + new Vector3(startX * Constant.TileSize.x, -startY * Constant.TileSize.y, 0);
                var targetpos = _zeropos + new Vector3(endX * Constant.TileSize.x, -endY * Constant.TileSize.y, 0);

                //_copyobject = GameObject.Instantiate(_object) as GameObject;

                _copyobject_target = GameObject.Instantiate(Resources.Load("Prefabs/MaskTiles/" + _name)) as GameObject;
                _copyobject_target.SetActive(true);

                var gridctrl_target = _copyobject_target.gameObject.GetComponent<GridMotionController>();
                //add by wwh
                if (gridctrl_target == null) gridctrl_target = _copyobject_target.gameObject.AddComponent<GridMotionController>();
                gridctrl_target.transform.position = copycurrentpos;
                gridctrl_target.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = startY * 2;
                gridctrl_target?.MoveToPosition(targetpos, movetime);

                _maskobject_ori = _maskModule?.GetMaskTile();
                _maskobject_copy = _maskModule?.GetMaskTile();
                if(_maskobject_ori)
                {
                    _maskobject_ori.InitMask(TileSize);
                    _maskobject_ori.gameObject.transform.position = currentTargetPos;
                }
                if (_maskobject_copy)
                {
                    _maskobject_copy.InitMask(TileSize);
                    _maskobject_copy.gameObject.transform.position = copycurrentpos;
                }
            }
        }

        virtual public void UpdateGrid(Grid grid)
        {
            if(_object != null)
            {
                if (_copyobject_ori != null)
                {
                    _object.SetActive(true);
                }
                //SetPos(x, y);
                grid.AttachTile = this;
                this.ParentGrid = grid;
                var selfpos = _zeropos + new Vector3(grid.ColID * Constant.TileSize.x, -grid.RowID * Constant.TileSize.y, 0);
                _object.transform.position = selfpos;
            }
            if (_copyobject_ori != null)
            {
                GameObject.Destroy(_copyobject_ori);
                _copyobject_ori = null;
            }
            if (_copyobject_target != null)
            {
                GameObject.Destroy(_copyobject_target);
                _copyobject_target = null;
            }
            if (_maskobject_ori != null)
            {
                _maskModule?.SetMaskTile(_maskobject_ori);
                _maskobject_ori = null;
            }
            if (_maskobject_copy != null)
            {
                _maskModule?.SetMaskTile(_maskobject_copy);
                _maskobject_copy = null;
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

