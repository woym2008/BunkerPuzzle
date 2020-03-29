using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using Bunker.Process;

namespace Bunker.Game
{
    public class BattleTurnsModule : LogicModule
    {
        Queue<CTurn> _TurnQuene;
        CTurn _CurTurn = null;

        public BattleTurnsModule() : base(typeof(BattleTurnsModule).ToString())
        {

        }
        public BattleTurnsModule(string name) : base(name)
        {

        }
        public override void OnStart()
        {
            base.OnStart();
            _CurTurn = null;
            _TurnQuene = new Queue<CTurn>();
            _TurnQuene.Enqueue(new PlayerTurn(this));
            _TurnQuene.Enqueue(new RobotTurn(this));
        }

        public void InsertTurn(string turn_type_name)
        {
            Type type = Type.GetType(string.Format("Bunker.Game.{0}", turn_type_name));
            if (type != null) { 
                var t = Activator.CreateInstance(type,this);
                _TurnQuene.Enqueue(t as CTurn);
            }
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
            _CurTurn.OnStartTurn();
        }
        public bool IsPlayerTurn()
        {
            return _CurTurn.GetType() == typeof(PlayerTurn);
        }
    }
    //
    public abstract class CTurn
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
            if (MissionManager.getInstance.GetMissionsState() == MissionManager.Mission_Success)
            {
                ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_WIN);
            }
            else if (MissionManager.getInstance.GetMissionsState() == MissionManager.Mission_Failure)
            {
                ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_LOSE);
            }
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
            base.OnUpdateTurn();
            if(_robotManagerModule.robotTurn == false)
            {
                _battleTurnsModule.NextTurn();
            }
        }
    }
}
