using UnityEngine;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    /// <summary>
    /// 格子锁定的状态
    /// </summary>
    public enum LockState
    {
        None,
        LockUPDown,
        LockLeftRight,
        LockAll
    }
    abstract public class BaseTile : ITile
    {
        public Grid ParentGrid { get; set; }

        protected Vector3 _zeropos;

        protected GameObject _object;
        protected GameObject _copyobject_ori;
        protected GameObject _copyobject_target;
        protected MaskTile _maskobject_ori;
        protected MaskTile _maskobject_copy;

        public GameObject Node {
            get {
                return _object;
            }
        }

        protected abstract int TileSize {
            get;
        }

        protected string _name;

        private bool _isUpdated = false;

        MaskTileModule _maskModule;

        ~BaseTile()
        {
            //OnDestroy();
        }

        public void Create(string name, Vector3 zeropos, Grid grid, string additionalData)
        {
            _name = name;

            ParentGrid = grid;

            OnLoadRes();

            Init(additionalData);

            _zeropos = zeropos;
            var selfpos = GetSelfWorldPos();

            _object.transform.position = selfpos;

            _maskModule = ModuleManager.getInstance.GetModule<MaskTileModule>();

            UpdateSortingOrder();

            if(GameDebug.EnableDebug)
            {
                var tgdc = _object.AddComponent<TileGridDebugController>();
                tgdc.GridNumber = new Vector2Int(ParentGrid.ColID, ParentGrid.RowID);
            }
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
            if (_object != null)
            {
                GameObject.Destroy(_object);
                _object = null;
            }
        }

        protected virtual void OnLoadRes()
        {
            var prefab = Resources.Load("Prefabs/Tiles/" + _name) as GameObject;
            _object = GameObject.Instantiate(prefab);
        }
        //初始化块，当块被生成的一刹那
        public virtual void Init(string additionalData)
        {
            
        }
        //初始化快，当所有的块都已经生成了，初始化块
        public virtual void OnStart()
        {

        }

        public virtual void UpdateSortingOrder()
        {
            //add by wwh
            if (_object!= null && _object.transform.childCount > 0)
            {
                var kid = _object.transform.GetChild(0);
                var sr = kid.GetComponent<SpriteRenderer>();
                sr.sortingOrder = ParentGrid.RowID * 2;
            }
        }

        //add by wwh 2021-4-21
        public int GetSortingOrder()
        {
            UpdateSortingOrder();
            return Node.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder;
        }

        //private void SetPos(int x, int y)
        //{
        //    _x = x;
        //    _y = y;
        //}

        /// <summary>
        /// 锁定状态
        /// </summary>
        /// <returns>The move.</returns>
        virtual public LockState GetLockState()
        {
            return LockState.None;
        }

        virtual public void MoveTo(int x, int y, float movetime, int direct, bool usecopy = false)
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
                var sordorder = _object.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder;
                gridctrl_ori.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = sordorder;
                gridctrl_ori?.MoveToPosition(targetpos, movetime);
                                
                _maskobject_ori = _maskModule?.GetMaskTile();
                if (_maskobject_ori)
                {
                    _maskobject_ori.InitMask(TileSize, sordorder, direct, true);
                    _maskobject_ori.gameObject.transform.position = targetpos;
                }

                _object?.SetActive(false);

                return;
            }
            var gridctrl = _object.gameObject.GetComponent<GridMotionController>();
            gridctrl?.MoveToPosition(targetpos, movetime);
        }
        virtual public void CopyMoveTo(int currentTargetX, int currentTargetY, int startX, int startY, int endX, int endY, float movetime, int direct)
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
                var sortorder = startY * 2;
                gridctrl_target.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = sortorder;
                gridctrl_target?.MoveToPosition(targetpos, movetime);

                _maskobject_copy = _maskModule?.GetMaskTile();
                
                if (_maskobject_copy)
                {
                    _maskobject_copy.InitMask(TileSize, sortorder, direct, false);
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

        /// <summary>
        /// 可破坏判定 可以被破坏的块，爆炸，锤击等
        /// </summary>
        /// <returns><c>true</c>, if break was caned, <c>false</c> otherwise.</returns>
        virtual public bool CanBreak()
        {
            return false;
        }

        /// <summary>
        /// 可消除判定，消除意味着大于一个的块放到一起可以消除
        /// </summary>
        /// <returns><c>true</c>, if elimination was caned, <c>false</c> otherwise.</returns>
        virtual public bool CanElimination()
        {
            return false;
        }

        /// <summary>
        /// 消除格子块并用一个新的格子块替换
        /// </summary>
        virtual public BaseTile Elimination()
        {
            var newTile = GridLoader.CreateTile("NormalTile", this.ParentGrid,"");
            return newTile;
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
        //---------------------------
        /// <summary>
        /// 毁掉格子块并用一个新的格子块替换
        /// </summary>
        virtual public BaseTile Break()
        {
            var newTile = GridLoader.CreateTile("NormalTile", this.ParentGrid,"");
            return newTile;
        }

        virtual public void OnBreakon()
        {
            Debug.LogFormat("我是{0}块，我被毁掉了", GetGridType());

            GridField.RemoveTile(this.ParentGrid);
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

