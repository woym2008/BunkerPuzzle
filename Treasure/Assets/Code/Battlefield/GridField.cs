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
//grid[horizontal][vertical]
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
                        MoveHorizontal(datas, -offsetValue);
                    }
                    break;
                case MoveDirect.Down:
                    {
                        GetVerticalLine(gridx, out var datas);
                        MoveHorizontal(datas, offsetValue);
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
                int targety = y + offset;
                int lengthLine = _grids.GetLength(1);
                if (targety < 0)
                {
                    targety = lengthLine + targety;
                }
                if (targety >= lengthLine)
                {
                    targety = targety - lengthLine;
                }
                _grids[x, targety] = datas[i];
            }
        }
        private void MoveHorizontal(IGridObject[] datas, int offset)
        {
            //var temp = datas[0];
            for (int i = 0; i < datas.Length; ++i)
            {
                int x = datas[i].X;
                int y = datas[i].Y;
                int targetx = x + offset;
                int lengthLine = _grids.GetLength(0);
                if (targetx < 0)
                {
                    targetx = targetx + lengthLine;
                }
                _grids[x, y + offset] = datas[i];
                if (targetx >= lengthLine)
                {
                    targetx = targetx - lengthLine;
                }
                _grids[targetx, y] = datas[i];
            }
        }

        private void GetAroundGrids(int x, int y, out IGridObject[] datas)
        {
            if (x >= _grids.GetLength(0) || y >= _grids.GetLength(1))
            {
                datas = null;
                return;
            }

            List<IGridObject> selects = new List<IGridObject>();
            var centerGrid = _grids[x, y];
            var up = GetGrid(x, y - 1);
            var down = GetGrid(x, y + 1);
            var left = GetGrid(x - 1, y);
            var right = GetGrid(x + 1, y);
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

        private IGridObject GetGrid(int x, int y)
        {
            if (x >= _grids.GetLength(0) || y >= _grids.GetLength(1) || x < 0 || y < 0)
            {
                return null;
            }

            return _grids[x, y];
        }

        private bool GetVerticalLine(int number, out IGridObject[] datas)
        {
            if (number >= _grids.GetLength(1))
            {
                datas = null;
                return false;
            }

            List<IGridObject> gs = new List<IGridObject>();
            for (int i = 0; i < _grids.GetLength(1); ++i)
            {

                gs.Add(_grids[i, number]);
            }
            datas = gs.ToArray();

            return true;
        }

        private bool GetHorizontalLine(int number, out IGridObject[] datas)
        {
            if (number >= _grids.GetLength(0))
            {
                datas = null;
                return false;
            }

            List<IGridObject> gs = new List<IGridObject>();
            for (int i = 0; i < _grids.GetLength(0); ++i)
            {
                gs.Add(_grids[number, i]);
            }
            datas = gs.ToArray();

            return true;
        }
    }
}

