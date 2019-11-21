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
            var g = m.Field.FindGrid("Bunker.Game.RobotStartTile");
            Debug.LogError("OnStart");
            var robot = CreateRobot<RobotThief>();
            robot.SetToGird(g);
        }
        public override void OnStop()
        {
            RemoveRobots();
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
            Debug.LogError("RemoveRobots");
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

                foreach (var bot in _RobotList)
                {
                    bot.OnPrepareMove();
                }

                _CurRobotIter = _RobotList.GetEnumerator();
                _CurRobotIter.MoveNext();

                if (_CurRobotIter.Current != null)
                {
                    _CurRobotIter.Current.OnStartMove();
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
    }
}
