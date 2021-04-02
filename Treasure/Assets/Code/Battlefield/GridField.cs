using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//------------------------------
//(0,0)********************
//*************************
//*************************
//*************************
//*************************
//*************************
//*************************
//*************************
//*************************
//******************(10,10)
//------------------------------
//grid[row][column]
//grid[列][行]
//
//------------------------------
namespace Bunker.Game
{
    public enum GridFieldState
    {
        Idle,
        Moving,
    }
    public enum MoveDirect
    {
        Left,
        Right,
        Up,
        Down
    }
    public struct GridPos
    {
        public int x;
        public int y;
    }
    public class GridField //: MonoBehaviour
    {
        //-------------------------------------
        GridFieldControllerBase _gridFieldController;
        //-------------------------------------
        //存行与列的节点集合，便于找到各个节点的开始数据
        //行开头的节点集合
        //0
        //1
        //:
        //:
        //9
        public Grid[] rowStartGrids;
        //列开头的节点集合
        //0 1 ... 9
        public Grid[] colStartGrids;

        //格子数组
        //这个数组主要为了方便访问格子
        //格子数组和上面两个格子起点数组不同，上面那里里存的都是非空白点
        //格子数组可能会存空格子点，即和其他格子没有链接关系的点，
        //这主要为了小人行走方便，可能有飞空怪之类的，格子数组作为整个地图，包括地面和天空
        public Grid[,] gridArray;

        static Transform _zeroPoint;
        static public Vector3 ZeroPos
        {
            get
            {
                if(_zeroPoint != null)
                {
                    return _zeroPoint.position;
                }
                return Vector3.zero;
            }
        }

        string _currectArea = "";
        string _currectLevel = "";
        //-------------------------------------
        public event Action<int> OnElimination;
        //-------------------------------------
        // Use this for initialization
        void Start()
        {
            //test

            _zeroPoint = GameObject.Find("ZeroPoint")?.transform;
            //just debug level
            //GridLoader.LoadGrid("Area_1","Level_1", out _grids);
        }

        // Update is called once per frame
        void Update()
        {

        }
        //---------------------------------------------------------------------
        public T SwitchController<T>() where T : GridFieldControllerBase, new()
        {
            _gridFieldController = new T();
            _gridFieldController.SetGridField(this);

            return _gridFieldController as T;
        }
        //---------------------------------------------------------------------
        public bool Load(string areaName, string levelName)
        {
            _currectArea = areaName;
            _currectLevel = levelName;

            _zeroPoint = GameObject.Find("ZeroPoint")?.transform;
            return GridLoader.LoadGrid(_currectArea, _currectLevel, out rowStartGrids, out colStartGrids, out gridArray);
        }

        public void RestartLevel()
        {
            if(_currectLevel != null && _currectArea != null)
            {
                if(rowStartGrids != null)
                {
                    for(int i = 0; i < rowStartGrids.Length; ++i)
                    {
                        rowStartGrids[i].AttachTile?.Delete();
                    }
                }
                
                _zeroPoint = GameObject.Find("ZeroPoint")?.transform;
                GridLoader.LoadGrid(_currectArea,_currectLevel, out rowStartGrids, out colStartGrids, out gridArray);
            }
        }
        //---------------------------------------------------------------------
        /*
        public void Move(MoveDirect dir, int gridx, int gridy, int offsetValue)
        {
            if(_gridFieldController == null)
            {
                SwitchController<GridFieldController_Idle>();
            }

            if(_gridFieldController.CanMove())
            {
                _gridFieldController.Move(dir, gridx, gridy, offsetValue);
            }
            /////
            switch (dir)
            {
                case MoveDirect.Left:
                    {
                        GetHorizontalLine(gridy, out var datas);
                        MoveHorizontal(datas, -offsetValue);
                    }
                    break;
                case MoveDirect.Right:
                    {
                        GetHorizontalLine(gridy, out var datas);
                        MoveHorizontal(datas, offsetValue);
                    }
                    break;
                case MoveDirect.Up:
                    {
                        GetVerticalLine(gridx, out var datas);
                        MoveVertical(datas, -offsetValue);
                    }
                    break;
                case MoveDirect.Down:
                    {
                        GetVerticalLine(gridx, out var datas);
                        MoveVertical(datas, offsetValue);
                    }
                    break;

            }
            /////
        }*/
        public void DestroyGrids(List<GridPos> gridpoints)
        {
            ;
        }
        //---------------------------------------------------------------------
        //add by wwh / Oh no!Just Test!
        public Grid GetGrid(Vector3 pos)
        {
            pos = pos - ZeroPos;
            var x = (pos.x - Constant.TileSize.x * 0.5f) / Constant.TileSize.x;
            var y = (pos.y + Constant.TileSize.y * 0.5f) / Constant.TileSize.y * -1;
            return GetGrid(Mathf.CeilToInt(x), Mathf.CeilToInt(y));
        }

        public Grid GetGrid(int column_value, int row_value)
        {
            if (gridArray == null || column_value >= gridArray.GetLength(0) || row_value >= gridArray.GetLength(1) ||
                column_value < 0 || row_value < 0)
            {
                return null;
            }

            return gridArray[column_value, row_value];
        }

        //public BaseTile GetTile(int column_value, int row_value)
        //{
        //    if (rowStartGrids != null || colStartGrids == null)
        //    {
        //        return null;
        //    }

        //    if (column_value >= colStartGrids.Length || column_value < 0)
        //    {
        //        return null;
        //    }

        //    if (row_value >= rowStartGrids.Length || row_value < 0)
        //    {
        //        return null;
        //    }

        //    var firstnode = colStartGrids[column_value];
        //    var curnode = firstnode;
        //    if (curnode != null)
        //    {
        //        if (curnode.AttachTile != null)
        //        {
        //            if (curnode.ColID == column_value && curnode.RowID == row_value)
        //            {
        //                return curnode.AttachTile;
        //            }
        //        }
        //        curnode = curnode.Down;
        //        while (curnode != firstnode)
        //        {
        //            if (curnode.AttachTile != null)
        //            {
        //                if (curnode.ColID == column_value && curnode.RowID == row_value)
        //                {
        //                    return curnode.AttachTile;
        //                }                        
        //            }
        //            curnode = curnode.Down;
        //        }
        //    }
        //    return null;
        //}

        public BaseTile GetTile(int column_value, int row_value)
        {
            if(gridArray == null || column_value >= gridArray.GetLength(0) || row_value >= gridArray.GetLength(1) ||
                column_value < 0 || row_value < 0)
            {
                return null;
            }

            var grid = gridArray[column_value, row_value];

            return grid.AttachTile;
        }
        private void SetTile(Grid grid, BaseTile tile)
        {
            SetTile(grid.ColID, grid.RowID, tile);
        }        
        private void SetTile(int column_value, int row_value, BaseTile tile)
        {
            if(rowStartGrids !=null || colStartGrids == null)
            {
                return;
            }

            if(column_value >= colStartGrids.Length || column_value < 0)
            {
                return;
            }

            if (row_value >= rowStartGrids.Length || row_value < 0)
            {
                return;
            }

            if(colStartGrids[column_value] != null)
            {
                var firstnode = colStartGrids[column_value];
                var curnode = firstnode;
                if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
                {
                    curnode.AttachTile = tile;
                    tile.ParentGrid = curnode;
                    return;
                }
                curnode = curnode.Down;

                while (curnode != firstnode)
                {
                    if((curnode.RowID == row_value) && (curnode.ColID == column_value))
                    {
                        curnode.AttachTile = tile;
                        tile.ParentGrid = curnode;
                        return;
                    }
                    curnode = curnode.Down;
                }
            }
            else if (rowStartGrids[row_value] != null)
            {
                var firstnode = rowStartGrids[row_value];
                var curnode = firstnode;
                if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
                {
                    curnode.AttachTile = tile;
                    tile.ParentGrid = curnode;
                    return;
                }
                curnode = curnode.Right;

                while (curnode != firstnode)
                {
                    if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
                    {
                        curnode.AttachTile = tile;
                        tile.ParentGrid = curnode;
                        return;
                    }
                    curnode = curnode.Right;
                }
            }
        }

        private void RemoveTile(Grid grid)
        {
            RemoveTile(grid.ColID, grid.RowID);
        }
        private void RemoveTile(int column_value, int row_value)
        {
            if (rowStartGrids == null || colStartGrids == null)
            {
                return;
            }

            if (column_value >= colStartGrids.Length || column_value < 0)
            {
                return;
            }

            if (row_value >= rowStartGrids.Length || row_value < 0)
            {
                return;
            }

            if (colStartGrids[column_value] != null)
            {
                var firstnode = colStartGrids[column_value];
                var curnode = firstnode;
                if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
                {
                    var tile = curnode.AttachTile;
                    if(tile != null)
                    {
                        tile.Delete();
                    }
                    curnode.AttachTile = null;
                    return;
                }
                curnode = curnode.Down;

                while (curnode != firstnode)
                {
                    if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
                    {
                        var tile = curnode.AttachTile;
                        if (tile != null)
                        {
                            tile.OnDestroy();
                        }
                        curnode.AttachTile = null;
                        return;
                    }
                    curnode = curnode.Down;
                }
            }
            else if (rowStartGrids[row_value] != null)
            {
                var firstnode = rowStartGrids[row_value];
                var curnode = firstnode;
                if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
                {
                    var tile = curnode.AttachTile;
                    if (tile != null)
                    {
                        tile.OnDestroy();
                    }
                    curnode.AttachTile = null;
                    return;
                }
                curnode = curnode.Right;

                while (curnode != firstnode)
                {
                    if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
                    {
                        var tile = curnode.AttachTile;
                        if (tile != null)
                        {
                            tile.OnDestroy();
                        }
                        curnode.AttachTile = null;
                        return;
                    }
                    curnode = curnode.Right;
                }
            }

        }

        public Vector2Int ClampGridPos(int x,int y)
        {
            int nx = x, ny = y;

            if (x >= gridArray.GetLength(0)) nx = colStartGrids.GetLength(0) - 1;
            if (y >= rowStartGrids.GetLength(1)) ny = rowStartGrids.GetLength(1) - 1;

            if (x < 0)  nx = 0;
            if (y < 0)  ny = 0;

            return new Vector2Int(nx, ny);
        }

        public BaseTile FindTile(string tileType)
        {
            if (colStartGrids != null)
            {
                for(int i=0; i< colStartGrids.Length; ++i)
                {
                    var firstnode = colStartGrids[i];
                    var curnode = firstnode;
                    if (curnode != null)
                    {
                        if(curnode.AttachTile != null)
                        {
                            if(curnode.AttachTile.GetGridType() == tileType)
                            {
                                return curnode.AttachTile;
                            }
                        }
                        curnode = curnode.Down;
                        while (curnode != firstnode)
                        {
                            if(curnode.AttachTile != null)
                            {
                                if (curnode.AttachTile.GetGridType() == tileType)
                                {
                                    return curnode.AttachTile;
                                }
                            }
                            curnode = curnode.Down;
                        }
                    }
                }          
            }
            
            return null;
        }

        public List<BaseTile> FindTileds(string tileType)
        {
            List<BaseTile> out_list = new List<BaseTile>();

            if (colStartGrids != null)
            {
                for (int i = 0; i < colStartGrids.Length; ++i)
                {
                    var firstnode = colStartGrids[i];
                    var curnode = firstnode;
                    if (curnode != null)
                    {
                        if (curnode.AttachTile != null)
                        {
                            if (curnode.AttachTile.GetGridType() == tileType)
                            {
                                out_list.Add(curnode.AttachTile);
                            }                            
                        }
                        curnode = curnode.Down;
                        while (curnode != firstnode)
                        {
                            if (curnode.AttachTile != null)
                            {
                                if (curnode.AttachTile.GetGridType() == tileType)
                                {
                                    out_list.Add(curnode.AttachTile);
                                }
                            }
                            curnode = curnode.Down;
                        }
                    }
                }
            }
            
            return out_list;
        }
        
        public BaseTile GetRandomTile(string tileType, bool abs_free)
        {
            List<BaseTile> tmp_list = new List<BaseTile>();

            tmp_list.Clear();
            if (colStartGrids != null)
            {
                for (int i = 0; i < colStartGrids.Length; ++i)
                {
                    var firstnode = colStartGrids[i];
                    var curnode = firstnode;
                    if (curnode != null)
                    {
                        if (curnode.AttachTile != null)
                        {
                            if (curnode.AttachTile.GetGridType() == tileType)
                            {
                                //这里先简易判断一下，格子上没有附着其他robot
                                if (!(abs_free && curnode.AttachTile.Node.transform.childCount > 1))
                                {
                                    tmp_list.Add(curnode.AttachTile);
                                }
                            }                            
                        }
                        curnode = curnode.Down;
                        while (curnode != firstnode)
                        {
                            if (curnode.AttachTile != null)
                            {
                                if (curnode.AttachTile.GetGridType() == tileType)
                                {
                                    //这里先简易判断一下，格子上没有附着其他robot
                                    if (!(abs_free && curnode.AttachTile.Node.transform.childCount > 1))
                                    {
                                        tmp_list.Add(curnode.AttachTile);
                                    }                                    
                                }
                            }
                            curnode = curnode.Down;
                        }
                    }
                }
            }

            if(tmp_list.Count > 0)
            {
                return tmp_list[UnityEngine.Random.Range(0, tmp_list.Count)];
            }
            return null;
        }

        public bool CanWalk(int x,int y)
        {
            var igo = GetTile(x,y);
            if (igo == null) return false;
            //这里判断是否格子被占据，不一定准确
            if((igo as BaseTile).Node.transform.childCount > 1) return false;
            //
            //if (igo.GetGridType() == "Bunker.Game.NormalTile" ||
            //    igo.GetGridType() == "Bunker.Game.GemTile" ||
            //    igo.GetGridType() == "Bunker.Game.RobotStartTile") return true;
            //return false;

            return igo.CanWalk();
        } 

        public BaseTile FindTileObject(GameObject node)
        {
            if (colStartGrids != null)
            {
                for (int i = 0; i < colStartGrids.Length; ++i)
                {
                    var firstnode = colStartGrids[i];
                    var curnode = firstnode;
                    if (curnode != null)
                    {
                        if (curnode.AttachTile != null)
                        {
                            if (curnode.AttachTile.Node == node)
                            {
                                return curnode.AttachTile;
                            }                            
                        }
                        curnode = curnode.Down;
                        while (curnode != firstnode)
                        {
                            if (curnode.AttachTile != null)
                            {
                                if (curnode.AttachTile.Node == node)
                                {
                                    return curnode.AttachTile;
                                }
                            }
                            curnode = curnode.Down;
                        }
                    }
                }
            }

            return null;
        }

        //---------------------------------------------------------------------

        public enum CheckEliminationType
        {
            Null,
            Horizontal_L,
            Horizontal_R,
            Vertical_U,
            Vertical_D,
        }
        public void CheckElimination(BaseTile tile, ref List<BaseTile> list, CheckEliminationType type = CheckEliminationType.Null)
        {
            if(tile != null)
            {
                var gridSelf = tile.ParentGrid;

                var gridUp = gridSelf.Up;
                var gridDown = gridSelf.Down;
                var gridLeft = gridSelf.Left;
                var gridRight = gridSelf.Right;

                bool bUpOK = false;
                bool bDownOK = false;
                bool bLeftOK = false;
                bool bRightOK = false;

                bool bHasUp = false;
                bool bHasDown = false;
                bool bHasLeft = false;
                bool bHasRight = false;

                foreach (var t in list)
                {
                    if(t.ParentGrid == gridUp)
                    {
                        bHasUp = true;
                    }
                    if (t.ParentGrid == gridUp)
                    {
                        bHasDown = true;
                    }
                    if (t.ParentGrid == gridUp)
                    {
                        bHasLeft = true;
                    }
                    if (t.ParentGrid == gridUp)
                    {
                        bHasRight = true;
                    }
                }

                if (gridUp.AttachTile != null)
                {
                    if (gridUp.AttachTile.CanElimination() 
                        && tile.CanEliminationByOther(gridUp.AttachTile.GetGridType(), 0)
                        && !bHasUp)
                    {
                        bUpOK = true;
                    }
                }
                if (gridDown.AttachTile != null)
                {
                    if (gridDown.AttachTile.CanElimination() 
                        && tile.CanEliminationByOther(gridDown.AttachTile.GetGridType(), 1)
                        && !bHasDown)
                    {
                        bDownOK = true;
                    }
                }
                if (gridLeft.AttachTile != null)
                {
                    if (gridLeft.AttachTile.CanElimination() 
                        && tile.CanEliminationByOther(gridLeft.AttachTile.GetGridType(), 2)
                        && !bHasLeft)
                    {
                        bLeftOK = true;
                    }
                }
                if (gridRight.AttachTile != null)
                {
                    if (gridRight.AttachTile.CanElimination() 
                        && tile.CanEliminationByOther(gridRight.AttachTile.GetGridType(), 3)
                        && !bHasRight)
                    {
                        bRightOK = true;
                    }
                }

                if (type == CheckEliminationType.Null)
                {
                    if (bDownOK || bUpOK)
                    {
                        if (bUpOK)
                        {
                            list.Add(gridUp.AttachTile);
                            CheckElimination(gridUp.AttachTile, ref list, CheckEliminationType.Vertical_U);
                        }
                        if (bDownOK)
                        {
                            list.Add(gridDown.AttachTile);
                            CheckElimination(gridDown.AttachTile, ref list, CheckEliminationType.Vertical_D);
                        }
                    }
                    else if (bLeftOK || bRightOK)
                    {
                        if (bLeftOK)
                        {

                            Debug.Log("bLeftOK - first: " + gridLeft.ColID + "," + gridLeft.RowID);

                            list.Add(gridLeft.AttachTile);
                            CheckElimination(gridLeft.AttachTile, ref list, CheckEliminationType.Horizontal_L);

                        }
                        if (bRightOK)
                        {

                            Debug.Log("bRightOK - first: " + gridRight.ColID + "," + gridRight.RowID);

                            list.Add(gridRight.AttachTile);
                            CheckElimination(gridRight.AttachTile, ref list, CheckEliminationType.Horizontal_R);
                        }
                    }
                }
                else if (type == CheckEliminationType.Horizontal_L)
                {
                    Debug.Log("bLeftOK: " + gridLeft.ColID + "," + gridLeft.RowID);
                    if (bLeftOK)
                    {
                        list.Add(gridLeft.AttachTile);
                        CheckElimination(gridLeft.AttachTile, ref list, CheckEliminationType.Horizontal_L);
                    }
                }
                else if (type == CheckEliminationType.Horizontal_R)
                {
                    Debug.Log("bRightOK: " + gridRight.ColID + "," + gridRight.RowID);
                    if (bRightOK)
                    {
                        list.Add(gridRight.AttachTile);
                        CheckElimination(gridRight.AttachTile, ref list, CheckEliminationType.Horizontal_R);
                    }
                }
                else if (type == CheckEliminationType.Vertical_U)
                {
                    Debug.Log("bUpOK: " + gridUp.ColID + "," + gridUp.RowID);
                    if (bUpOK)
                    {
                        list.Add(gridUp.AttachTile);
                        CheckElimination(gridUp.AttachTile, ref list, CheckEliminationType.Vertical_U);
                    }
                }
                else if (type == CheckEliminationType.Vertical_D)
                {
                    Debug.Log("bDownOK: " + gridDown.ColID + "," + gridDown.RowID);
                    if (bDownOK)
                    {
                        list.Add(gridDown.AttachTile);
                        CheckElimination(gridDown.AttachTile, ref list, CheckEliminationType.Vertical_D);
                    }
                }
            }
        }

        public void EliminationGrids(List<BaseTile> elimiGrids)
        {
            foreach(var g in elimiGrids)
            {
                Debug.Log("EliminationGrid: " + g.ParentGrid.ColID + "," + g.ParentGrid.RowID);
                g.Elimination();
                var grid = g.ParentGrid;
                RemoveTile(grid);
                var newg = GridLoader.CreateTile("NormalTile", grid);
                SetTile(grid, newg);

                g.OnEliminationed();
            }

            OnElimination?.Invoke(elimiGrids.Count);
        }

        //通过检测grids中有没有可消除grid，来判断是否都消除完了
        public bool IsAllGridsElimination()
        {
            if (colStartGrids != null)
            {
                for (int i = 0; i < colStartGrids.Length; ++i)
                {
                    var firstnode = colStartGrids[i];
                    var curnode = firstnode;
                    if (curnode != null)
                    {
                        if (curnode.AttachTile != null)
                        {
                            if (curnode.AttachTile.CanElimination())
                            {
                                return false;
                            }
                            curnode = curnode.Down;
                        }
                        while (curnode != firstnode)
                        {
                            if (curnode.AttachTile != null)
                            {
                                if (curnode.AttachTile.CanElimination())
                                {
                                    return false;
                                }
                            }
                            curnode = curnode.Down;
                        }
                    }
                }
            }

            return true;
        }
    }
}

