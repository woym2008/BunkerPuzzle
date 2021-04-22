using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using Bunker.Process;

namespace Bunker.Game
{
    public class RobotManagerModule : LogicModule
    {
        List<RobotBase> _RobotList;
        List<RobotBase> _WillRemoveRobotList;
        List<RobotBase>.Enumerator _CurRobotIter;
        List<Vector2Int> _WillBeVisitedList;
        public bool robotTurn;
        //
        public List<string> enemyTypeList;
        public List<string> friendlyTypeList;
        //
        public RobotManagerModule() : base(typeof(RobotManagerModule).ToString())
        {

        }
        public RobotManagerModule(string name) : base(name)
        {

        }

        public override void Create()
        {
            _RobotList = new List<RobotBase>();
            _WillBeVisitedList = new List<Vector2Int>();
            _WillRemoveRobotList = new List<RobotBase>();
        }
        public override void Release()
        {
            base.Release();
        }
        public override void OnStart(params object[] data)
        {
            base.OnStart();
            //TODO create robot at Special Tile from BattleField!
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var spawn_list = m.Field.FindTileds("Bunker.Game.RobotStartTile");
            foreach(var t in spawn_list)
            {
                if(enemyTypeList != null && enemyTypeList.Count != 0)
                {
                    var robot = CreateRobot(enemyTypeList[UnityEngine.Random.Range(0, enemyTypeList.Count)]);
                    robot.SetToGird(t.ParentGrid);
                }
                else
                {
                    var robot = CreateRobot<RobotThief>();
                    robot.SetToGird(t.ParentGrid);
                }
            }
            //加入其他robot的出生点
            spawn_list = m.Field.FindTileds("Bunker.Game.PorterStartTile");
            foreach (var t in spawn_list)
            {
                if (friendlyTypeList != null && friendlyTypeList.Count != 0)
                {
                    var robot = CreateRobot(friendlyTypeList[UnityEngine.Random.Range(0, friendlyTypeList.Count)]);
                    robot.SetToGird(t.ParentGrid);
                }
                else
                {
                    var robot = CreateRobot<RobotPorter>();
                    robot.SetToGird(t.ParentGrid);
                }
            }

        }
        public override void OnStop()
        {
            Debug.Log("RobotManagerModule OnStop");
            //
            RemoveRobots();
            //这个值也要初始化，否则下次游戏就只能进行一个回合
            robotTurn = false;

            base.OnStop();
        }
        public void Update(float dt)
        {

        }
        ///------------
        public RobotBase CreateRobot<T>() where T : RobotBase, new()
        {
            var bot_name = typeof(T).Name;
            return CreateRobot(bot_name);
            /*
            var go = GameObject.Instantiate(Resources.Load("Prefabs/Robot/" + bot_name)) as GameObject;
            var bot = go.AddComponent<T>();
            bot.OnInit();
            _RobotList.Add(bot);
            Debug.Log("_RobotList count" + _RobotList.Count);
            return bot;
            */
        }

        public RobotBase CreateRobot(string bot_name)
        {
            var go = GameObject.Instantiate(Resources.Load("Prefabs/Robot/" + bot_name)) as GameObject;
            Type robot_t = Type.GetType(string.Format("{0}{1}", Constant.DOMAIN_PREFIX, bot_name));
            var bot = go.AddComponent(robot_t) as RobotBase;
            bot.OnInit();
            _RobotList.Add(bot);
            Debug.Log("_RobotList count" + _RobotList.Count);
            return bot;
        }



        public void RemoveRobots()
        {
            foreach (var bot in _RobotList)
            {
                GameObject.Destroy(bot.gameObject);
            }
            _RobotList.Clear();
            _WillRemoveRobotList.Clear();

        }
        //
        public void StartRobotsTurn()
        {
            if (!robotTurn)
            {
                robotTurn = true;

                _WillBeVisitedList.Clear();

                foreach (var bot in _RobotList)
                {
                    bot.OnPrepareMove();
                    _WillBeVisitedList.Add(bot.GetDestination());
                }

                _CurRobotIter = _RobotList.GetEnumerator();
                _CurRobotIter.MoveNext();

                if (_CurRobotIter.Current != null)
                {
                    _CurRobotIter.Current.OnStartMove();
                }
                else
                {
                    EndRobotsTurn();
                }
            }

        }
        public void EndRobotsTurn()
        {
            foreach (var bot in _RobotList)
            {
                bot.SetIdle();
            }
            //清理一下删除的robot
            foreach (var bot in _WillRemoveRobotList)
            {
                _RobotList.Remove(bot);
                GameObject.Destroy(bot.gameObject);
            }
            _WillRemoveRobotList.Clear();
            robotTurn = false;
        }
        public void NextRobot()
        {
            Debug.Log("NextRobot...");
            if (_CurRobotIter.MoveNext())
            {
                _CurRobotIter.Current.OnStartMove();
            }
            else
            {
                EndRobotsTurn();
            }
        }

        public bool IsInWillBeVisitedList(int x,int y)
        {
            foreach (var pos in _WillBeVisitedList)
            {
                if (pos.x == x && pos.y == y) return true;
            }
            return false;
        }

        public void UpdateEnemyTypeList(string[] array)
        {
            if(array!= null && array.Length > 0)
                enemyTypeList = new List<string>(array);
        }

        public void UpdateFriendlyTypeList(string[] array)
        {
            if (array != null && array.Length > 0)
                friendlyTypeList = new List<string>(array);
        }

        ////////////////////////////////
        
        public int GetRobotCount(string robot_type)
        {
            int n = 0;
            foreach (var robot in _RobotList)
            {
                if (robot.GetType().Name == robot_type)
                {
                    n++;
                }
            }
            return n;
        }

        public RobotBase GetRobot(string robot_type)
        {
            RobotBase r = null;
            foreach (var robot in _RobotList)
            {
                if (robot.GetType().Name == robot_type)
                {
                    r = robot;
                    break;
                }
            }
            return r;
        }

        public RobotBase GetRobot(int col,int row)
        {
            RobotBase r = null;
            foreach (var robot in _RobotList)
            {
                var g = robot.GetGridXY();
                if (g.x == col && g.y == row)
                {
                    r = robot;
                    break;
                }
            }
            return r;
        }

        public void RemoveRobot(RobotBase r)
        {
            foreach (var robot in _RobotList)
            {
                if (robot == r)
                {
                    _WillRemoveRobotList.Add(robot);
                    //先不清理，只进行隐藏
                    robot.gameObject.SetActive(false);
                    break;
                }
            }
        }
  
    }
}
