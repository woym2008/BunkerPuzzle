using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public class GridField //: MonoBehaviour
    {
        //-------------------------------------
        GridFieldControllerBase _gridFieldController;
        //-------------------------------------
        private IGridObject[,] _grids;
        public IGridObject[,] Grids
        {
            get
            {
                return _grids;
            }
        }

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

        // Use this for initialization
        void Start()
        {
            //test

            _zeroPoint = GameObject.Find("ZeroPoint")?.transform;
            GridLoader.LoadGrid("Level_1", out _grids);
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
        public void Load(string name)
        {
            _zeroPoint = GameObject.Find("ZeroPoint")?.transform;
            GridLoader.LoadGrid("Level_1", out _grids);
        }

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
            /*
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
            */
        }
        //---------------------------------------------------------------------

        public IGridObject GetGrid(int column_value, int row_value)
        {
            if (column_value >= _grids.GetLength(0) || row_value >= _grids.GetLength(1) || column_value < 0 || row_value < 0)
            {
                return null;
            }

            return _grids[row_value, column_value];
        }

        private void SetGrid(int column_value, int row_value, IGridObject grid)
        {
            if (column_value >= _grids.GetLength(0) || row_value >= _grids.GetLength(1) || column_value < 0 || row_value < 0)
            {
                return;
            }
            _grids[row_value, column_value] = grid;
        }

        private void RemoveGrid(int column_value, int row_value)
        {
            if (column_value >= _grids.GetLength(0) || row_value >= _grids.GetLength(1) || column_value < 0 || row_value < 0)
            {
                return;
            }
            Debug.LogWarning("Delete Grid: " + row_value + "," + column_value);
            _grids[row_value, column_value].Delete();
            _grids[row_value, column_value] = null;
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
        public void CheckElimination(IGridObject grid, ref List<IGridObject> list, CheckEliminationType type = CheckEliminationType.Null)
        {
            if(_grids != null)
            {
                //1 up
                bool bUpOK = true;
                var gridUp = GetGrid(grid.X, grid.Y + 1);
                if (gridUp == null)
                {
                    bUpOK = false;
                }
                else
                {
                    if (gridUp.GetGridType() != grid.GetGridType() || !gridUp.CanElimination())
                    {
                        bUpOK = false;
                    }
                }



                //2 down
                bool bDownOK = true;
                var gridDown = GetGrid(grid.X, grid.Y - 1);
                if (gridDown == null)
                {
                    bDownOK = false;
                }
                else
                {
                    if (gridDown.GetGridType() != grid.GetGridType() || !gridDown.CanElimination())
                    {
                        bDownOK = false;
                    }
                }


                //3 left
                bool bLeftOK = true;
                var gridLeft = GetGrid(grid.X - 1, grid.Y);
                if (gridLeft == null)
                {
                    bLeftOK = false;
                }
                else
                {
                    if (gridLeft.GetGridType() != grid.GetGridType() || !gridLeft.CanElimination())
                    {
                        bLeftOK = false;
                    }
                }


                //4 right
                bool bRightOK = true;
                var gridRight = GetGrid(grid.X + 1, grid.Y);
                if (gridRight == null)
                {
                    bRightOK = false;
                }
                else
                {
                    if (gridRight.GetGridType() != grid.GetGridType() || !gridRight.CanElimination())
                    {
                        bRightOK = false;
                    }
                }


                if (type == CheckEliminationType.Null)
                {
                    if(bDownOK || bUpOK)
                    {
                        if(bUpOK)
                        {
                            list.Add(gridUp);
                            CheckElimination(gridUp, ref list, CheckEliminationType.Vertical_U);
                        }
                        if(bDownOK)
                        {
                            list.Add(gridDown);
                            CheckElimination(gridDown, ref list, CheckEliminationType.Vertical_D);
                        }
                    }
                    else if(bLeftOK || bRightOK)
                    {
                        if(bLeftOK)
                        {

                            Debug.Log("bLeftOK - first: " + gridLeft.X + "," + gridLeft.Y);

                            list.Add(gridLeft);
                            CheckElimination(gridLeft, ref list, CheckEliminationType.Horizontal_L);

                        }
                        if(bRightOK)
                        {

                            Debug.Log("bRightOK - first: " + gridRight.X + "," + gridRight.Y);

                            list.Add(gridRight);
                            CheckElimination(gridRight, ref list, CheckEliminationType.Horizontal_R);
                        }
                    }
                }
                else if(type == CheckEliminationType.Horizontal_L)
                {
                    Debug.Log("bLeftOK: " + gridLeft.X + "," + gridLeft.Y);
                    if (bLeftOK)
                    {
                        list.Add(gridLeft);
                        CheckElimination(gridLeft, ref list, CheckEliminationType.Horizontal_L);
                    }
                }
                else if (type == CheckEliminationType.Horizontal_R)
                {
                    Debug.Log("bRightOK: " + gridRight.X + "," + gridRight.Y);
                    if (bRightOK)
                    {
                        list.Add(gridRight);
                        CheckElimination(gridRight, ref list, CheckEliminationType.Horizontal_R);
                    }
                }
                else if(type == CheckEliminationType.Vertical_U)
                {
                    Debug.Log("bUpOK: " + gridUp.X + ","+ gridUp.Y);
                    if (bUpOK)
                    {
                        list.Add(gridUp);
                        CheckElimination(gridUp, ref list, CheckEliminationType.Vertical_U);
                    }
                }
                else if (type == CheckEliminationType.Vertical_D)
                {
                    Debug.Log("bDownOK: " + gridDown.X + "," + gridDown.Y);
                    if (bDownOK)
                    {
                        list.Add(gridDown);
                        CheckElimination(gridDown, ref list, CheckEliminationType.Vertical_D);
                    }
                }
            }
        }

        public void EliminationGrids(List<IGridObject> elimiGrids)
        {
            foreach(var g in elimiGrids)
            {
                Debug.Log("EliminationGrid: " + g.X + "," + g.Y);
                g.Elimination();
                var x = g.X;
                var y = g.Y;
                RemoveGrid(x, y);
                var newg = GridLoader.CreateGrid("NormalTile", x, y);
                SetGrid(x,y, newg);
            }
        }
    }
}

