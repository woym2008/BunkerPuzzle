using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;

namespace Bunker.Game
{
    public class GridFieldController_Moving : GridFieldControllerBase
    {
        const float MoveOneGridTime = 1.0f;

        bool _enableTimeCount = false;

        CommonMonoBehaviour _updateObject;

        bool _isMoving = false;

        public GridFieldController_Moving()
        {
            _enableTimeCount = false;
        }

        override public string ControllerType
        {
            get
            {
                return "Moving";
            }
        }
        /*
        public override void Update(float dt)
        {
            base.Update(dt);
        }*/
        //-------------------------------------------------------------------------------------
        public override bool IsFinish()
        {
            return !_isMoving;
        }
        public void OnFinishMovingAnmia()
        {
            //不知道在这里写这样的代码合适么？
            Debug.Log(" -- OnFinishMovingAnmia --");
            var btm = ModuleManager.getInstance.GetModule<BattleTurnsModule>();
            if(btm != null)
            {
                if (btm.IsPlayerTurn())
                {
                    btm.NextTurn();
                }
            }

        }

        public override void Excute(params object[] objs)
        {
            base.Excute(objs);

            _isMoving = true;

            MoveDirect dir = (MoveDirect)objs[0];
            int gridx = (int)objs[1];
            int gridy = (int)objs[2];
            int offsetValue = (int)objs[3];

            Move(dir, gridx, gridy, offsetValue);
        }
        //-------------------------------------------------------------------------------------
        private void Move(MoveDirect dir, int gridx, int gridy, int offsetValue)
        {
            switch (dir)
            {
                case MoveDirect.Left:
                    {
                        GetHorizontalLine(gridy, out var datas);
                        MoveHorizontal_Animation(datas, -offsetValue);
                    }
                    break;
                case MoveDirect.Right:
                    {
                        GetHorizontalLine(gridy, out var datas);
                        MoveHorizontal_Animation(datas, -offsetValue);
                    }
                    break;
                case MoveDirect.Up:
                    {
                        GetVerticalLine(gridx, out var datas);
                        MoveVertical_Animation(datas, -offsetValue);
                    }
                    break;
                case MoveDirect.Down:
                    {
                        GetVerticalLine(gridx, out var datas);
                        MoveVertical_Animation(datas, -offsetValue);
                    }
                    break;

            }
        }
        //-------------------------------------------------------------------------------------
        private void MoveHorizontal_Animation(IGridObject[] datas, int offset)
        {
            var movetime = Mathf.Abs(MoveOneGridTime * offset);
            for (int i = 0; i < datas.Length; ++i)
            {
                int x = datas[i].X;
                int y = datas[i].Y;
                int column_value = x + offset;
                int lengthLine = _gridfield.Grids.GetLength(0);
                if (column_value < 0)
                {
                    var copytarget = lengthLine + column_value;
                    var copyori = copytarget - offset;

                    datas[i].CopyMoveTo(copyori, y, copytarget, y, movetime);
                }
                if (column_value >= lengthLine)
                {
                    var copytarget = lengthLine - column_value;
                    var copyori = copytarget - offset;

                    datas[i].CopyMoveTo(copyori, y, copytarget, y, movetime);
                }

                datas[i].MoveTo(column_value, y, movetime);
                Debug.Log("mh: " + datas[i].X);
            }

            MonoBehaviourHelper.StartCoroutine(WaitforUpdateHorizontal(datas, offset, movetime));
        }
        private void MoveVertical_Animation(IGridObject[] datas, int offset)
        {
            var movetime = Mathf.Abs(MoveOneGridTime * offset);
            for (int i = 0; i < datas.Length; ++i)
            {
                int x = datas[i].X;
                int y = datas[i].Y;
                int row_value = y + offset;
                int lengthLine = _gridfield.Grids.GetLength(1);
                if (row_value < 0)
                {
                    var copytarget = lengthLine + row_value;
                    var copyori = copytarget - offset;

                    datas[i].CopyMoveTo(x,copyori,x,copytarget,movetime);
                }
                if (row_value >= lengthLine)
                {
                    var copytarget = row_value - lengthLine;
                    var copyori = copytarget - offset;

                    datas[i].CopyMoveTo(x, copyori, x, copytarget, movetime);
                }
                datas[i].MoveTo(x, row_value, movetime);
            }

            MonoBehaviourHelper.StartCoroutine(WaitforUpdateVertical(datas, offset, movetime));
        }

        IEnumerator WaitforUpdateHorizontal(IGridObject[] datas, int offset, float time)
        {
            yield return new WaitForSeconds(time);

            MoveHorizontal(datas, offset);

            EliminationUpdate();

            _isMoving = false;

            //_gridfield.SwitchController<GridFieldController_Idle>();
            OnFinishMovingAnmia();
        }

        IEnumerator WaitforUpdateVertical(IGridObject[] datas, int offset, float time)
        {
            yield return new WaitForSeconds(time);

            MoveVertical(datas, offset);

            EliminationUpdate();

            _isMoving = false;
            //_gridfield.SwitchController<GridFieldController_Idle>();
            OnFinishMovingAnmia();

        }
        //-------------------------------------------------------------------------------------
        private void MoveVertical(IGridObject[] datas, int offset)
        {
            //var temp = datas[0];
            for (int i = 0; i < datas.Length; ++i)
            {
                int x = datas[i].X;
                int y = datas[i].Y;
                int row_value = y + offset;
                int lengthLine = _gridfield.Grids.GetLength(1);
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
                _gridfield.Grids[g.Y, g.X] = g;
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
                int lengthLine = _gridfield.Grids.GetLength(0);
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
            foreach (var g in datas)
            {
                //列，行
                _gridfield.Grids[g.Y, g.X] = g;
            }
        }

        private void GetAroundGrids(int column_value, int row_value, out IGridObject[] datas)
        {
            if (row_value >= _gridfield.Grids.GetLength(0) || column_value >= _gridfield.Grids.GetLength(1))
            {
                datas = null;
                return;
            }

            List<IGridObject> selects = new List<IGridObject>();
            var centerGrid = _gridfield.Grids[row_value, column_value];
            var up = _gridfield.GetGrid(column_value, row_value - 1);
            var down = _gridfield.GetGrid(column_value, row_value + 1);
            var left = _gridfield.GetGrid(column_value - 1, row_value);
            var right = _gridfield.GetGrid(column_value + 1, row_value);
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

        private bool GetVerticalLine(int number, out IGridObject[] datas)
        {
            if (number >= _gridfield.Grids.GetLength(0))
            {
                datas = null;
                return false;
            }

            List<IGridObject> gs = new List<IGridObject>();
            for (int i = 0; i < _gridfield.Grids.GetLength(0); ++i)
            {

                gs.Add(_gridfield.Grids[i, number]);
            }
            datas = gs.ToArray();

            return true;
        }

        private bool GetHorizontalLine(int number, out IGridObject[] datas)
        {
            if (number >= _gridfield.Grids.GetLength(1))
            {
                datas = null;
                return false;
            }

            List<IGridObject> gs = new List<IGridObject>();
            for (int i = 0; i < _gridfield.Grids.GetLength(1); ++i)
            {
                gs.Add(_gridfield.Grids[number, i]);
            }
            datas = gs.ToArray();

            return true;
        }
        //-------------------------------------------------------------------------------------
        public void EliminationUpdate()
        {
            //竖直
            for (int i = 0; i < _gridfield.Grids.GetLength(0); ++i)
            {
                //水平
                for (int j = 0; j < _gridfield.Grids.GetLength(1); ++j)
                {
                    List<IGridObject> tempGridList = new List<IGridObject>();
                    var grid = _gridfield.Grids[j, i];
                    //modify by wwh : this is update the sortorder!
                    (grid as BaseGrid).UpdateSortingOrder();
                    //Debug.Log("grid update: " + i + j);
                   // Debug.Log("grid update: " + grid.X + grid.Y);
                    if (grid.CanElimination())
                    {
                        tempGridList.Add(grid);

                        _gridfield.CheckElimination(grid, ref tempGridList);

                        if (tempGridList.Count >= 2)
                        {
                            Debug.Log("Find 3 same grid");
                            _gridfield.EliminationGrids(tempGridList);
                        }

                    }
                }
            }
        }

    }
}
