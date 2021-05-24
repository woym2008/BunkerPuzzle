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
        //通行数组
        int[,] blockMap;

        static Transform _zeroPoint;
        static public Vector3 zeroOffset;
        static public Vector3 ZeroPos
        {
            get
            {
                if(_zeroPoint != null)
                {
                    return _zeroPoint.position + zeroOffset;
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

            zeroOffset = Vector3.zero;

            _zeroPoint = GameObject.Find("ZeroPoint")?.transform;
            var ret = GridLoader.LoadGrid(_currectArea, _currectLevel, out rowStartGrids, out colStartGrids, out gridArray);

            blockMap = new int[gridArray.GetLength(0), gridArray.GetLength(1)];

            return ret;
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

                zeroOffset = Vector3.zero;

                _zeroPoint = GameObject.Find("ZeroPoint")?.transform;
                GridLoader.LoadGrid(_currectArea,_currectLevel, out rowStartGrids, out colStartGrids, out gridArray);

                blockMap = new int[gridArray.GetLength(0), gridArray.GetLength(1)];
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

        public void GetAroundGrids(int column_value, int row_value, out BaseTile[] datas)
        {
            var centerTile = GetTile(column_value, row_value);

            if (centerTile == null)
            {
                datas = null;
                return;
            }

            List<BaseTile> selects = new List<BaseTile>();
            var up = GetTile(column_value, row_value - 1);
            var down = GetTile(column_value, row_value + 1);
            var left = GetTile(column_value - 1, row_value);
            var right = GetTile(column_value + 1, row_value);
            if (up != null)
            {
                selects.Add(up);
            }
            if (down != null)
            {
                selects.Add(down);
            }
            if (left != null)
            {
                selects.Add(left);
            }
            if (right != null)
            {
                selects.Add(right);
            }
            datas = selects.ToArray();
        }

        public bool GetVerticalLine(int number, out BaseTile[] datas)
        {
            if (number >= colStartGrids.Length || number < 0)
            {
                datas = null;
                return false;
            }

            var firstnode = colStartGrids[number];
            var curnode = firstnode;
            List<BaseTile> gs = new List<BaseTile>();
            if (firstnode != null)
            {
                if (firstnode.AttachTile != null)
                {
                    gs.Add(firstnode.AttachTile);
                }

                curnode = firstnode.Down;
                while (curnode != firstnode)
                {
                    if (curnode.AttachTile != null)
                    {
                        gs.Add(curnode.AttachTile);
                    }
                    curnode = curnode.Down;
                }
            }
            datas = gs.ToArray();

            return true;
        }

        public bool GetHorizontalLine(int number, out BaseTile[] datas)
        {
            if (number >= rowStartGrids.Length || number < 0)
            {
                datas = null;
                return false;
            }

            var firstnode = rowStartGrids[number];
            var curnode = firstnode;
            List<BaseTile> gs = new List<BaseTile>();
            if (firstnode != null)
            {
                if (firstnode.AttachTile != null)
                {
                    gs.Add(firstnode.AttachTile);
                }

                curnode = firstnode.Right;
                while (curnode != firstnode)
                {
                    if (curnode.AttachTile != null)
                    {
                        gs.Add(curnode.AttachTile);
                    }
                    curnode = curnode.Right;
                }
            }
            datas = gs.ToArray();

            return true;
        }

        public Grid GetHorizontalStartGrid(int col)
        {
            if (colStartGrids != null && col < colStartGrids.Length && col >= 0)
            {
                return colStartGrids[col];
            }

            return null;
        }

        public Grid GetVerticalLineStartGrid(int row)
        {
            if (rowStartGrids != null && row < rowStartGrids.Length && row >= 0)
            {
                return rowStartGrids[row];
            }

            return null;
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

            //这里为什么也是反的？modify by wwh 2021-5-14
            //return gridArray[row_value,column_value];
            return gridArray[column_value,row_value];

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

            //modify by wwh 2021-5-14 这里为什么是反过来的？！？
            //var grid = gridArray[row_value,column_value];
            var grid = gridArray[column_value,row_value];


            return grid.AttachTile;
        }
        public static void SetTile(Grid grid, BaseTile tile)
        {
            //SetTile(grid.ColID, grid.RowID, tile);
            grid.AttachTile = tile;
            tile.ParentGrid = grid;
        }        
        //private void SetTile(int column_value, int row_value, BaseTile tile)
        //{
        //    if(rowStartGrids !=null || colStartGrids == null)
        //    {
        //        return;
        //    }

        //    if(column_value >= colStartGrids.Length || column_value < 0)
        //    {
        //        return;
        //    }

        //    if (row_value >= rowStartGrids.Length || row_value < 0)
        //    {
        //        return;
        //    }

        //    if(colStartGrids[column_value] != null)
        //    {
        //        var firstnode = colStartGrids[column_value];
        //        var curnode = firstnode;
        //        if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
        //        {
        //            curnode.AttachTile = tile;
        //            tile.ParentGrid = curnode;
        //            return;
        //        }
        //        curnode = curnode.Down;

        //        while (curnode != firstnode)
        //        {
        //            if((curnode.RowID == row_value) && (curnode.ColID == column_value))
        //            {
        //                curnode.AttachTile = tile;
        //                tile.ParentGrid = curnode;
        //                return;
        //            }
        //            curnode = curnode.Down;
        //        }
        //    }
        //    else if (rowStartGrids[row_value] != null)
        //    {
        //        var firstnode = rowStartGrids[row_value];
        //        var curnode = firstnode;
        //        if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
        //        {
        //            curnode.AttachTile = tile;
        //            tile.ParentGrid = curnode;
        //            return;
        //        }
        //        curnode = curnode.Right;

        //        while (curnode != firstnode)
        //        {
        //            if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
        //            {
        //                curnode.AttachTile = tile;
        //                tile.ParentGrid = curnode;
        //                return;
        //            }
        //            curnode = curnode.Right;
        //        }
        //    }
        //}

        public static void RemoveTile(Grid grid)
        {
            //RemoveTile(grid.ColID, grid.RowID);
            var tile = grid.AttachTile;
            if(tile != null)
            {
                grid.AttachTile = null;
                tile.ParentGrid = null;
                tile.Delete();
            }
        }
        /*
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

            var tile = gridArray[];

            //if (colStartGrids[column_value] != null)
            //{
            //    var firstnode = colStartGrids[column_value];
            //    var curnode = firstnode;
            //    if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
            //    {
            //        var tile = curnode.AttachTile;
            //        if(tile != null)
            //        {
            //            tile.Delete();
            //        }
            //        curnode.AttachTile = null;
            //        return;
            //    }
            //    curnode = curnode.Down;

            //    while (curnode != firstnode)
            //    {
            //        if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
            //        {
            //            var tile = curnode.AttachTile;
            //            if (tile != null)
            //            {
            //                tile.OnDestroy();
            //            }
            //            curnode.AttachTile = null;
            //            return;
            //        }
            //        curnode = curnode.Down;
            //    }
            //}
            //else if (rowStartGrids[row_value] != null)
            //{
            //    var firstnode = rowStartGrids[row_value];
            //    var curnode = firstnode;
            //    if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
            //    {
            //        var tile = curnode.AttachTile;
            //        if (tile != null)
            //        {
            //            tile.OnDestroy();
            //        }
            //        curnode.AttachTile = null;
            //        return;
            //    }
            //    curnode = curnode.Right;

            //    while (curnode != firstnode)
            //    {
            //        if ((curnode.RowID == row_value) && (curnode.ColID == column_value))
            //        {
            //            var tile = curnode.AttachTile;
            //            if (tile != null)
            //            {
            //                tile.OnDestroy();
            //            }
            //            curnode.AttachTile = null;
            //            return;
            //        }
            //        curnode = curnode.Right;
            //    }
            //}

        }
        */
        public Vector2Int ClampGridPos(int x,int y)
        {
            int nx = x, ny = y;

            if (x >= gridArray.GetLength(0)) nx = gridArray.GetLength(0) - 1;
            if (y >= gridArray.GetLength(1)) ny = gridArray.GetLength(1) - 1;

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
            int walkable_count = 0;
            for (int i = igo.Node.transform.childCount - 1; i > 0; i--)
            {
                var kid = igo.Node.transform.GetChild(i);
                if (kid.gameObject.name.EndsWith(Constant.CAN_WALK_SUFFIX))
                {
                    walkable_count++;
                }
            }
            if (igo.Node.transform.childCount - walkable_count > 1) return false;               
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
                    if (t.ParentGrid == gridDown)
                    {
                        bHasDown = true;
                    }
                    if (t.ParentGrid == gridLeft)
                    {
                        bHasLeft = true;
                    }
                    if (t.ParentGrid == gridRight)
                    {
                        bHasRight = true;
                    }
                }

                if (gridUp.AttachTile != null)
                {
                    if(Mathf.Abs(gridUp.RowID - gridSelf.RowID) == 1)
                    {
                        if (gridUp.AttachTile.CanElimination()
                        && tile.CanEliminationByOther(gridUp.AttachTile.GetGridType(), 0)
                        && !bHasUp)
                        {
                            bUpOK = true;
                        }
                    }
                    
                }
                if (gridDown.AttachTile != null)
                {
                    if (Mathf.Abs(gridDown.RowID - gridSelf.RowID) == 1)
                    {
                        if (gridDown.AttachTile.CanElimination()
                        && tile.CanEliminationByOther(gridDown.AttachTile.GetGridType(), 1)
                        && !bHasDown)
                        {
                            bDownOK = true;
                        }
                    }                        
                }
                if (gridLeft.AttachTile != null)
                {
                    if (Mathf.Abs(gridLeft.ColID - gridSelf.ColID) == 1)
                    {
                        if (gridLeft.AttachTile.CanElimination()
                        && tile.CanEliminationByOther(gridLeft.AttachTile.GetGridType(), 2)
                        && !bHasLeft)
                        {
                            bLeftOK = true;
                        }
                    }                        
                }
                if (gridRight.AttachTile != null)
                {
                    if (Mathf.Abs(gridRight.ColID - gridSelf.ColID) == 1)
                    {
                        if (gridRight.AttachTile.CanElimination()
                        && tile.CanEliminationByOther(gridRight.AttachTile.GetGridType(), 3)
                        && !bHasRight)
                        {
                            bRightOK = true;
                        }
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
                BaseTile newtile = g.Elimination();
                var grid = g.ParentGrid;

                g.OnEliminationed();

                RemoveTile(grid);
                //var newg = GridLoader.CreateTile("NormalTile", grid);
                SetTile(grid, newtile);
            }

            OnElimination?.Invoke(elimiGrids.Count);
        }

        public void BreakGrids(List<BaseTile> tiles)
        {
            foreach (var g in tiles)
            {
                Debug.Log("Break Grid: " + g.ParentGrid.ColID + "," + g.ParentGrid.RowID);
                var newg = g.Break();
                var grid = g.ParentGrid;

                g.OnBreakon();

                //RemoveTile(grid);
                //var newg = GridLoader.CreateTile("NormalTile", grid);
                SetTile(grid, newg);
            }
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

        public int[,] GetWalkMap()
        {
            for (int i=0;i< gridArray.GetLength(0);++i)
            {
                for(int j = 0; j < gridArray.GetLength(1); ++j)
                {
                    var grid = gridArray[i, j];
                    int blockvalue = 0;
                    if (grid.AttachTile != null)
                    {
                        if (grid.AttachTile.CanWalk())
                        {
                            blockvalue = 1;
                        }
                    }
                    
                    blockMap[i, j] = blockvalue;
                }
            }

            return blockMap;
        }
    }
}

