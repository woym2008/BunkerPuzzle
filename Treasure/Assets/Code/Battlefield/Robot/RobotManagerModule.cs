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
        }
        public override void OnStop()
        {
            base.OnStop();
        }
        public void Update(float dt)
        {

        }
        ///------------
        public void CreateRobot<T>() where T : RobotBase, new()
        {
            var bot_name = typeof(T).Name;
            var go = GameObject.Instantiate(Resources.Load("Robot/" + bot_name)) as GameObject;
            go.name = bot_name;
            var bot = go.AddComponent<T>();
            bot.OnInit();
            _RobotList.Add(bot);
        }
        public void RemoveRobot()
        {
            foreach(var bot in _RobotList)
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
                _CurRobotIter = _RobotList.GetEnumerator();
                foreach (var bot in _RobotList)
                {
                    bot.OnPrepareMove();
                }
                //
                _CurRobotIter.Current.OnStartMove();
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
