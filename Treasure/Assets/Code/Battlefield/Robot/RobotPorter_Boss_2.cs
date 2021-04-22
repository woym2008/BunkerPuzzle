using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using Bunker.Process;
using DG.Tweening;

namespace Bunker.Game
{
    public class RobotPorter_Boss_2 : RobotBase
    {
        Vector3Int[] DirList = { Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left };
        List<KeyValuePair<float, int>> weightDir = new List<KeyValuePair<float, int>>();

        Animator dead_effect;
        Animator robot_anima;

        //missile
        GameObject missile_item = null;
        GameObject missile_obj = null;
        BaseTile saved_tile = null;
        //
        const string missile_item_name = "Boss_2_R";
        const string missile_object_name = "boss_2_missile";


        public override void OnInit()
        {
            base.OnInit();
            robot_anima = transform.Find("PorterSprite").GetComponent<Animator>();
            dead_effect = transform.Find("PorterSprite/dead_effect").GetComponent<Animator>();
            //
            CreateMissileItem();
            CreateMissileObject();
            //
            SetPenetrable(true);
        }

        public void CreateMissileObject()
        {
            ////////
            if (missile_obj == null)
                missile_obj = GameObject.Instantiate(Resources.Load("Prefabs/Boss/" + missile_object_name)) as GameObject;
            missile_obj.transform.position = transform.position;
            missile_obj.SetActive(false);
        }

        public void CreateMissileItem()
        {
            if(missile_item == null)
                missile_item = GameObject.Instantiate(Resources.Load("Prefabs/Boss/"+ missile_item_name)) as GameObject;
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            saved_tile = m.Field.GetRandomTile("Bunker.Game.NormalTile", true);
            saved_tile = m.Field.GetTile(1,0);
            missile_item.transform.SetParent(saved_tile.Node.transform);
            missile_item.transform.localPosition = Vector3.zero;
            missile_item.GetComponent<SpriteRenderer>().sortingOrder = saved_tile.GetSortingOrder() + 1;
            missile_item.SetActive(true);
            //使用名字后缀描述的方法····
            missile_item.name = missile_item.name + Constant.CAN_WALK_SUFFIX;
        }

        public override void OnPrepareMove()
        {
            base.OnPrepareMove();
            //update cur tile
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var g = m.Field.GetGrid(transform.position);
            if (g != null)
            {
                var sr = GetComponentInChildren<SpriteRenderer>();
                int offset = _dir.y == 0 ? 1 : 3;
                sr.sortingOrder = g.RowID * 2 + offset;
            }
            else
            {
                Debug.Log("RobotPorter OnPrepareMove ERROR!");
            }

        }

        public override Vector3Int FindWay()
        {
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var igo = saved_tile;
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
            //傻傻呆在原地
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
                if (g == saved_tile)
                {
                    CDebug.Log("主角获得导弹！", Constant.COLOR_NAME.RED);
                    //隐藏missile item
                    missile_item.SetActive(false);
                    //主角开始开火动画！
                    robot_anima.SetTrigger("fire");
                    //找到boss2的pos
                    //Here do Tween 
                    var btm = ModuleManager.getInstance.GetModule<BattleTurnsModule>();
                    var boss = (btm.FindTurn("Boss_2_Turn") as Boss_2_Turn).boss_body;
                    missile_obj.SetActive(true);
                    missile_obj.transform.position = transform.position;

                    Vector2 direction = boss.transform.position - missile_obj.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    missile_obj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    missile_obj.transform.DOMove(boss.transform.position,2f)
                        .SetEase(Ease.InQuad)
                        .OnComplete(FinishMissileFly);
                }
                if (g is DiskTile)
                {
                    dead_effect.gameObject.SetActive(true);
                    dead_effect.SetTrigger("play");
                    robot_anima.SetTrigger("dead");
                    var rac = dead_effect.runtimeAnimatorController;
                    if (rac != null)
                    {
                        foreach (AnimationClip clip in rac.animationClips)
                        {
                            if (clip.name == "bloody")
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

        public void FinishMissileFly()
        {
            missile_obj.SetActive(false);
            missile_obj.transform.position = transform.position;
            var btm = ModuleManager.getInstance.GetModule<BattleTurnsModule>();
            var boss_trun = btm.FindTurn("Boss_2_Turn") as Boss_2_Turn;           
            if (boss_trun != null)
            {
                //导弹爆炸特效

                //boss减少血量
                boss_trun.DecreaseHP();
                //再随机生成一个missile item
                CreateMissileItem();
            }
        }
    }
}
