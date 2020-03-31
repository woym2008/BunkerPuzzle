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
        List<RobotBase>.Enumerator _CurRobotIter;
        List<Vector2Int> _WillBeVisitedList;
        public bool robotTurn;
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
        }
        public override void Release()
        {
            base.Release();
        }
        public override void OnStart()
        {
            base.OnStart();
            //TODO create robot at Special Tile from BattleField!
            var m = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var spawn_list = m.Field.FindGrids("Bunker.Game.RobotStartTile");
            foreach(var g in spawn_list)
            {
                var robot = CreateRobot<RobotThief>();
                robot.SetToGird(g);
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
        public T CreateRobot<T>() where T : RobotBase, new()
        {
            var bot_name = typeof(T).Name;
            var go = GameObject.Instantiate(Resources.Load("Prefabs/Robot/" + bot_name)) as GameObject;
            var bot = go.AddComponent<T>();
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
    }
}
