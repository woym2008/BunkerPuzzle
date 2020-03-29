using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using Bunker.Process;

namespace Bunker.Game
{
    public class BossTurn : CTurn
    {
        BattlefieldModule _bfm;
        public BossTurn(BattleTurnsModule btm) : base(btm)
        {
            _bfm = ModuleManager.getInstance.GetModule<BattlefieldModule>();
        }

        public override void OnStartTurn()
        {
            base.OnStartTurn();
        }

        public override void OnEndTurn()
        {
            base.OnEndTurn();
        }

        public override void OnUpdateTurn()
        {
            base.OnUpdateTurn();
            _battleTurnsModule.NextTurn();

        }
    }
}
