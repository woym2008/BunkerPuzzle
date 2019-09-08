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
    public enum MoveDirect
    {
        Left,
        Right,
        Up,
        Down
    }
    public class GridField //: MonoBehaviour
    {
        public IGridObject[,] _grids;

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

        public void Load(string name)
        {
            _zeroPoint = GameObject.Find("ZeroPoint")?.transform;
            GridLoader.LoadGrid("Level_1", out _grids);
        }

        public void Move(MoveDirect dir, int gridx, int gridy, int offsetValue)
        {
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
        }

        private void MoveVertical(IGridObject[] datas, int offset)
        {
            //var temp = datas[0];
            for (int i = 0; i < datas.Length; ++i)
            {
                int x = datas[i].X;
                int y = datas[i].Y;
                int row_value = y + offset;
                int lengthLine = _grids.GetLength(1);
                if (row_value < 0)
                {
                    row_value = lengthLine + row_value;
                }
                if (row_value >= lengthLine)
                {
                    row_value = row_value - lengthLine;
                }
                datas[i].UpdateGrid(x, row_value);
            }
            foreach (var g in datas)
            {
                //列，行
                _grids[g.Y, g.X] = g;
            }
        }
        private void MoveHorizontal(IGridObject[] datas, int offset)
        {
            //var temp = datas[0];
            for (int i = 0; i < datas.Length; ++i)
            {
                int x = datas[i].X;
                int y = datas[i].Y;
                int column_value = x + offset;
                int lengthLine = _grids.GetLength(0);
                if (column_value < 0)
                {
                    column_value = column_value + lengthLine;
                }
                if (column_value >= lengthLine)
                {
                    column_value = column_value - lengthLine;
                }
                //_grids[targetx, y] = datas[i];

                datas[i].UpdateGrid(column_value, y);
                Debug.Log("mh: " + datas[i].X);
            }
            foreach(var g in datas)
            {
                //列，行
                _grids[g.Y, g.X] = g;
            }
        }

        private void GetAroundGrids(int column_value, int row_value, out IGridObject[] datas)
        {
            if (row_value >= _grids.GetLength(0) || column_value >= _grids.GetLength(1))
            {
                datas = null;
                return;
            }

            List<IGridObject> selects = new List<IGridObject>();
            var centerGrid = _grids[row_value, column_value];
            var up = GetGrid(column_value, row_value - 1);
            var down = GetGrid(column_value, row_value + 1);
            var left = GetGrid(column_value - 1, row_value);
            var right = GetGrid(column_value + 1, row_value);
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

        private IGridObject GetGrid(int column_value, int row_value)
        {
            if (column_value >= _grids.GetLength(0) || row_value >= _grids.GetLength(1) || column_value < 0 || row_value < 0)
            {
                return null;
            }

            return _grids[row_value, column_value];
        }

        private bool GetVerticalLine(int number, out IGridObject[] datas)
        {
            if (number >= _grids.GetLength(0))
            {
                datas = null;
                return false;
            }

            List<IGridObject> gs = new List<IGridObject>();
            for (int i = 0; i < _grids.GetLength(0); ++i)
            {

                gs.Add(_grids[i, number]);
            }
            datas = gs.ToArray();

            return true;
        }

        private bool GetHorizontalLine(int number, out IGridObject[] datas)
        {
            if (number >= _grids.GetLength(1))
            {
                datas = null;
                return false;
            }

            List<IGridObject> gs = new List<IGridObject>();
            for (int i = 0; i < _grids.GetLength(1); ++i)
            {
                gs.Add(_grids[number, i]);
            }
            datas = gs.ToArray();

            return true;
        }
    }
}

