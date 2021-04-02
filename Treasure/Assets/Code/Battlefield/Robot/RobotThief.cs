using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using Bunker.Process;


namespace Bunker.Game
{
    public class RobotThief : RobotBase
    {
        Vector3Int[] DirList = { Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left };
        List<KeyValuePair<float, int>> weightDir = new List<KeyValuePair<float, int>>();

        public override void OnPrepareMove()
        {
            base.OnPrepareMove();
            //update cur tile
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var g = m.Field.GetGrid(transform.position);
            if (g != null)
            {
                //SetToGird(g);
                //
                var sr = GetComponentInChildren<SpriteRenderer>();
                int offset = _dir.y == 0 ? 1 : 3;
                sr.sortingOrder = g.ColID * 2 + offset;
            }
            else
            {
                Debug.Log("RobotThief OnPrepareMove ERROR!");
            }

        }

        public override void OnFinishMove()
        {
            //recalc new grid XY
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            _pos = m.Field.ClampGridPos(_pos.x + _dir.x , _pos.y - _dir.y); // here need use -
            var g = m.Field.GetTile(_pos.x, _pos.y);

            //we check wheather the robot is catched the gemTile
            if (g != null && g is GemTile)
            {
                //Game Over
                ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_LOSE);
            }
            //
            base.OnFinishMove();

        }

        public override Vector3Int FindWay()
        {
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var igo = m.Field.FindTile("Bunker.Game.GemTile");
            if (igo != null)
            {
                var wp = igo.Node.transform.position;
                //首先确定一个方向
                var dx = wp.x - transform.position.x;
                var dy = wp.y - transform.position.y;
                //
                weightDir.Clear();
                //
                for (int i = 0; i < DirList.Length; ++i)
                {
                    weightDir.Add(new KeyValuePair<float, int>(Vector3.Dot(new Vector3(dx,dy,0),DirList[i]),i));
                }

                weightDir.Sort((left, right) =>
                {
                    if (left.Key > right.Key)
                        return -1;
                    else if (left.Key == right.Key)
                        return 0;
                    else
                        return 1;
                });

                //循环判断是否可行
                for (int i = 0; i < weightDir.Count; ++i)
                {
                    var dir = DirList[weightDir[i].Value];
                    var rmm = ModuleManager.getInstance.GetModule<RobotManagerModule>();
                    if (m.Field.CanWalk(_curNode.ColID + dir.x, _curNode.RowID - dir.y) &
                        !rmm.IsInWillBeVisitedList(_curNode.ColID + dir.x, _curNode.RowID - dir.y))
                    {
                        return dir;
                    }
                }
            }
            return Vector3Int.zero;
        }
    }
}
