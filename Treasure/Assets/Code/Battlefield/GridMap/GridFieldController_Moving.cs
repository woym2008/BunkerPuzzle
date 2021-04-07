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
        public void OnFinishMovingAnima()
        {
            //不知道在这里写这样的代码合适么？
            Debug.Log(" -- OnFinishMovingAnmia --");
            //这里插入一个步数的消耗
            MissionManager.getInstance.ConsumeStep();
            //换回合
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
                        GetHorizontalLine(gridy, out BaseTile[] tiles);
                        MoveHorizontal_Animation(tiles, -offsetValue);
                    }
                    break;
                case MoveDirect.Right:
                    {
                        GetHorizontalLine(gridy, out BaseTile[] tiles);
                        MoveHorizontal_Animation(tiles, -offsetValue);
                    }
                    break;
                case MoveDirect.Up:
                    {
                        GetVerticalLine(gridx, out BaseTile[] tiles);
                        MoveVertical_Animation(tiles, -offsetValue);
                    }
                    break;
                case MoveDirect.Down:
                    {
                        GetVerticalLine(gridx, out BaseTile[] tiles);
                        MoveVertical_Animation(tiles, -offsetValue);
                    }
                    break;

            }
        }
        //-------------------------------------------------------------------------------------
        private void MoveHorizontal_Animation(BaseTile[] datas, int offset)
        {
            var movetime = Mathf.Abs(MoveOneGridTime * offset);

            for (int i = 0; i < datas.Length; ++i)
            {
                int x = datas[i].ParentGrid.ColID;
                int y = datas[i].ParentGrid.RowID;

                int col_value = x + offset;
                var targetGrid = datas[i].ParentGrid.GetColOffsetGrid(offset);

                int dir = offset > 0 ? -1 : -2;

                //不连续的则说明不挨着，就需要有个用来填充表示的copytile
                if (Mathf.Abs(datas[i].ParentGrid.ColID - targetGrid.ColID) > 1)
                {
                    var copytargetColID = targetGrid.ColID - offset;
                                        
                    datas[i].CopyMoveTo(col_value, y, copytargetColID, y, targetGrid.ColID, y, movetime, dir);

                    datas[i].MoveTo(col_value, y, movetime, dir, true);
                }
                else
                {
                    datas[i].MoveTo(col_value, y, movetime, dir);
                }


                datas[i].ParentGrid = targetGrid;
            }            

            MonoBehaviourHelper.StartCoroutine(WaitforUpdateHorizontal(datas, offset, movetime));
        }
        private void MoveVertical_Animation(BaseTile[] datas, int offset)
        {
            var movetime = Mathf.Abs(MoveOneGridTime * offset);

            for (int i = 0; i < datas.Length; ++i)
            {
                int x = datas[i].ParentGrid.ColID;
                int y = datas[i].ParentGrid.RowID;

                int row_value = y + offset;
                var targetGrid = datas[i].ParentGrid.GetRowOffsetGrid(offset);

                int dir = offset > 0 ? 2 : 1;

                //不连续的则说明不挨着，就需要有个用来填充表示的copytile
                if (Mathf.Abs(datas[i].ParentGrid.RowID - targetGrid.RowID) > 1)
                {
                    var copytargetRowID = targetGrid.RowID - offset;

                    datas[i].CopyMoveTo(x, row_value, x, copytargetRowID, x, targetGrid.RowID, movetime, dir);

                    datas[i].MoveTo(x, row_value, movetime, dir, true);
                }
                else
                {
                    datas[i].MoveTo(x, row_value, movetime, dir);
                }

                datas[i].ParentGrid = targetGrid;
            }

            MonoBehaviourHelper.StartCoroutine(WaitforUpdateVertical(datas, offset, movetime));
        }

        IEnumerator WaitforUpdateHorizontal(BaseTile[] datas, int offset, float time)
        {
            yield return new WaitForSeconds(time);

            //MoveHorizontal(datas, offset);
            foreach (var tile in datas)
            {
                tile.UpdateGrid(tile.ParentGrid);
            }

            EliminationUpdate();

            _isMoving = false;

            //_gridfield.SwitchController<GridFieldController_Idle>();
            OnFinishMovingAnima();
        }

        IEnumerator WaitforUpdateVertical(BaseTile[] datas, int offset, float time)
        {
            yield return new WaitForSeconds(time);

            //MoveVertical(datas, offset);
            foreach (var tile in datas)
            {
                tile.UpdateGrid(tile.ParentGrid);
            }

            EliminationUpdate();

            _isMoving = false;
            //_gridfield.SwitchController<GridFieldController_Idle>();
            OnFinishMovingAnima();

        }
        //-------------------------------------------------------------------------------------
        //private void MoveVertical(BaseTile[] datas, int offset)
        //{
        //    //var temp = datas[0];
        //    //for (int i = 0; i < datas.Length; ++i)
        //    //{
        //    //    int x = datas[i].ParentGrid.ColID;
        //    //    int y = datas[i].ParentGrid.RowID;
        //    //    int row_value = y + offset;
        //    //    int lengthLine = _gridfield.gridArray.GetLength(1);

        //    //    var curgrid = datas[i].ParentGrid;
        //    //    int offsetvalue = Mathf.Abs(offset);
        //    //    if (offset > 0)
        //    //    {
        //    //        for (int j = 0; j < offsetvalue; ++j)
        //    //        {
        //    //            curgrid = curgrid.Right;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        for (int j = 0; j < offsetvalue; ++j)
        //    //        {
        //    //            curgrid = curgrid.Left;
        //    //        }
        //    //    }

        //    //    datas[i].UpdateGrid(x, row_value);
        //    //}
        //    //foreach (var g in datas)
        //    //{
        //    //    //列，行
        //    //    _gridfield.Grids[g.Y, g.X] = g;
        //    //}
        //    foreach (var tile in datas)
        //    {
        //        tile.ParentGrid.AttachTile = tile;
        //        tile.UpdateGrid(tile.ParentGrid.ColID, tile.ParentGrid.RowID);
        //    }
        //}
        //private void MoveHorizontal(BaseTile[] datas, int offset)
        //{
        //    //var temp = datas[0];
        //    //for (int i = 0; i < datas.Length; ++i)
        //    //{
        //    //    int x = datas[i].ParentGrid.ColID;
        //    //    int y = datas[i].ParentGrid.RowID;
        //    //    int column_value = x + offset;
        //    //    int lengthLine = _gridfield.gridArray.GetLength(0);
        //    //    if (column_value < 0)
        //    //    {
        //    //        column_value = column_value + lengthLine;
        //    //    }
        //    //    if (column_value >= lengthLine)
        //    //    {
        //    //        column_value = column_value - lengthLine;
        //    //    }
        //    //    //_grids[targetx, y] = datas[i];

        //    //    datas[i].UpdateGrid(column_value, y);
        //    //    Debug.Log("mh: " + datas[i].X);
        //    //}
        //    //foreach (var g in datas)
        //    //{
        //    //    //列，行
        //    //    _gridfield.Grids[g.Y, g.X] = g;
        //    //}
        //    foreach (var tile in datas)
        //    {
        //        tile.ParentGrid.AttachTile = tile;
        //        tile.UpdateGrid(tile.ParentGrid.ColID, tile.ParentGrid.RowID);
        //    }
        //}

        private void GetAroundGrids(int column_value, int row_value, out BaseTile[] datas)
        {
            var centerTile = _gridfield.GetTile(column_value, row_value);

            if(centerTile == null)
            {
                datas = null;
                return;
            }

            List<BaseTile> selects = new List<BaseTile>();
            var up = _gridfield.GetTile(column_value, row_value - 1);
            var down = _gridfield.GetTile(column_value, row_value + 1);
            var left = _gridfield.GetTile(column_value - 1, row_value);
            var right = _gridfield.GetTile(column_value + 1, row_value);
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

        private bool GetVerticalLine(int number, out BaseTile[] datas)
        {
            if (number >= _gridfield.colStartGrids.Length || number < 0)
            {
                datas = null;
                return false;
            }

            var firstnode = _gridfield.colStartGrids[number];
            var curnode = firstnode;
            List<BaseTile> gs = new List<BaseTile>();
            if(firstnode != null)
            {
                if(firstnode.AttachTile != null)
                {
                    gs.Add(firstnode.AttachTile);
                }                

                curnode = firstnode.Down;
                while(curnode != firstnode)
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

        private bool GetHorizontalLine(int number, out BaseTile[] datas)
        {
            if (number >= _gridfield.rowStartGrids.Length || number < 0)
            {
                datas = null;
                return false;
            }

            var firstnode = _gridfield.rowStartGrids[number];
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

        private Grid GetHorizontalStartGrid(int col)
        {
            if(_gridfield.colStartGrids != null && col < _gridfield.colStartGrids.Length && col >= 0)
            {
                return _gridfield.colStartGrids[col];
            }

            return null;
        }

        private Grid GetVerticalLineStartGrid(int row)
        {
            if (_gridfield.rowStartGrids != null && row < _gridfield.rowStartGrids.Length && row >= 0)
            {
                return _gridfield.rowStartGrids[row];
            }

            return null;
        }
        //-------------------------------------------------------------------------------------
        public void EliminationUpdate()
        {
            //竖直
            for (int i = 0; i < _gridfield.gridArray.GetLength(0); ++i)
            {
                //水平
                for (int j = 0; j < _gridfield.gridArray.GetLength(1); ++j)
                {
                    List<BaseTile> tempGridList = new List<BaseTile>();
                    var grid = _gridfield.gridArray[j, i];
                    if(grid.AttachTile == null)
                    {
                        continue;
                    }
                    var tile = grid.AttachTile;
                    //modify by wwh : this is update the sortorder!
                    tile.UpdateSortingOrder();
                    //Debug.Log("grid update: " + i + j);
                   // Debug.Log("grid update: " + grid.X + grid.Y);
                    if (tile.CanElimination())
                    {
                        tempGridList.Add(tile);

                        _gridfield.CheckElimination(tile, ref tempGridList);

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
