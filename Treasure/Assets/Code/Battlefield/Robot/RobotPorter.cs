using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using Bunker.Process;


namespace Bunker.Game
{
    public class RobotPorter : RobotBase
    {
        Vector3Int[] DirList = { Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left };
        List<KeyValuePair<float, int>> weightDir = new List<KeyValuePair<float, int>>();

        Animator dead_effect;
        Animator robot_dead;


        public override void OnInit()
        {
            base.OnInit();
            robot_dead = transform.Find("PorterSprite").GetComponent<Animator>();
            dead_effect = transform.Find("PorterSprite/dead_effect").GetComponent<Animator>();
        }

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
                sr.sortingOrder = g.RowID * 2 + offset;
            }
            else
            {
                Debug.Log("RobotPorter OnPrepareMove ERROR!");
            }

        }
        //
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
                    weightDir.Add(new KeyValuePair<float, int>(Vector3.Dot(new Vector3(dx, dy, 0), DirList[i]), i));
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
        //
        public override void OnFinishMove()
        {
            //recalc new grid XY
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            _pos = m.Field.ClampGridPos(_pos.x + _dir.x, _pos.y - _dir.y); // here need use -
            var g = m.Field.GetTile(_pos.x, _pos.y);

            //如果机器人走到死亡机关上，则会播放完死亡动画，然后游戏结束，不再更新robotManager了
            if (g != null)
            {
                if(g is GemTile)
                {
                    ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_WIN);
                }
                if(g is DiskTile)
                {
                    dead_effect.gameObject.SetActive(true);
                    dead_effect.SetTrigger("play");
                    robot_dead.SetTrigger("dead");
                    var rac = dead_effect.runtimeAnimatorController;
                    if (rac != null)
                    {
                        foreach (AnimationClip clip in rac.animationClips)
                        {
                            if(clip.name == "bloody")
                            {
                                Invoke("OnDeadEffectFinish", clip.length);
                                break;
                            }
                        }
                    }
                    return;
                }
            }
            //
            base.OnFinishMove();

        }

        public void OnDeadEffectFinish()
        {
            ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_LOSE);
        }
    }
}
