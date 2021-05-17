using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using Bunker.Process;

namespace Bunker.Game
{
    /*
       add by wwh 2021-5-18 加一个 准备开始下一回合的通知回调       
         
    */
    public delegate void TurnsNotification(CTurn turn);

    public class BattleTurnsModule : LogicModule
    {
        Queue<CTurn> _TurnQuene;
        CTurn _CurTurn = null;
        public TurnsNotification Notifications { set; get; }

        public BattleTurnsModule() : base(typeof(BattleTurnsModule).ToString())
        {

        }
        public BattleTurnsModule(string name) : base(name)
        {

        }
        public override void OnStart(params object[] data)
        {
            base.OnStart();
            _CurTurn = null;
            _TurnQuene = new Queue<CTurn>();
            _TurnQuene.Enqueue(new PlayerTurn(this));
            _TurnQuene.Enqueue(new StarredTurn(this));
            _TurnQuene.Enqueue(new RobotTurn(this));
        }

        public void InsertTurn(string turn_type_name,bool fromTop = false)
        {
            Type type = Type.GetType(string.Format("{0}{1}",Constant.DOMAIN_PREFIX, turn_type_name));
            if (type != null) { 
                var t = Activator.CreateInstance(type,this);
                var len = _TurnQuene.Count;
                _TurnQuene.Enqueue(t as CTurn);
                while (fromTop && len > 0)
                {
                    var turn = _TurnQuene.Dequeue();
                    _TurnQuene.Enqueue(turn);
                    len--;
                }
            }
        }

        public CTurn FindTurn(string turn_type_name)
        {
            CTurn turn = null;
            foreach (var t in _TurnQuene)
            {
                if(t.GetType().Name == turn_type_name)
                {
                    turn = t;
                    break;
                }
            }
            return turn;
        }

        public override void OnStop()
        {
            _CurTurn = null;
            base.OnStop();
        }

        protected override void OnModuleMessage(string msg, object[] args)
        {
            base.OnModuleMessage(msg, args);
        }

        public void Update(float dt)
        {
            DoTurn();
        }
        //-------------------------------------------
        public void DoTurn()
        {
            if (_CurTurn != null) _CurTurn.OnUpdateTurn();
        }
        public void NextTurn()
        {
            if (_CurTurn != null)
            {
                _CurTurn.OnEndTurn();
                _TurnQuene.Enqueue(_CurTurn);
            }
            _CurTurn = _TurnQuene.Dequeue();
            Notifications?.Invoke(_CurTurn);
            _CurTurn.OnStartTurn();
        }
        public bool IsPlayerTurn()
        {
            return _CurTurn.GetType() == typeof(PlayerTurn);
        }
    }
    //
    public interface ITurn
    {
        void OnStartTurn();
        void OnUpdateTurn();
        void OnEndTurn();
    }
    public abstract class CTurn : ITurn
    {
        protected BattleTurnsModule _battleTurnsModule;
        public CTurn(BattleTurnsModule btm) {
            _battleTurnsModule = btm;
        }
        public virtual void OnStartTurn()
        { Debug.Log(string.Format("this is {0} OnStartTurn", this.GetType().Name)); }
        public virtual void OnUpdateTurn()
        { Debug.Log(string.Format("this is {0} OnUpdateTurn", this.GetType().Name)); }
        public virtual void OnEndTurn()
        { Debug.Log(string.Format("this is {0} OnEndTurn", this.GetType().Name)); }
    }

    public class PlayerTurn : CTurn
    {
        BattlefieldInputModule _battleFieldInputModule;
        public PlayerTurn(BattleTurnsModule btm) : base(btm)
        {
            _battleFieldInputModule = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
        }

        public override void OnStartTurn()
        {
            base.OnStartTurn();
            _battleFieldInputModule.locked = false;
        }

        public override void OnUpdateTurn()
        {
            //base.OnUpdateTurn();
        }

        public override void OnEndTurn()
        {
            _battleFieldInputModule.locked = true;
            //在玩家操作结束后再做判断查看任务是否完成
            //主动check 等待事件通知
            MissionManager.getInstance.CheckMissionState();

            //if (MissionManager.getInstance.GetMissionsState() == MissionManager.Mission_Success)
            //{
            //    ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_WIN);
            //}
            //else if (MissionManager.getInstance.GetMissionsState() == MissionManager.Mission_Failure)
            //{
            //    ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_LOSE);
            //}
            //
            base.OnEndTurn();
        }
    }
    public class RobotTurn : CTurn
    {
        RobotManagerModule _robotManagerModule;
        public RobotTurn(BattleTurnsModule btm) : base(btm)
        {
            _robotManagerModule = ModuleManager.getInstance.GetModule<RobotManagerModule>();
        }

        public override void OnStartTurn()
        {
            base.OnStartTurn();
            _robotManagerModule.StartRobotsTurn();
        }

        public override void OnEndTurn()
        {
            base.OnEndTurn();
        }

        public override void OnUpdateTurn()
        {
            //base.OnUpdateTurn();
            if(_robotManagerModule.robotTurn == false)
            {
                _battleTurnsModule.NextTurn();
            }
        }
    }
}
