using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using Bunker.Process;

namespace Bunker.Game
{
    //这里我们先用bossTurn代替第一个boss，更准确应该为 Boss_1_Turn
    //----------------------
    /*
        BOSS_1 应该有2个阶段
        第一个阶段，每2回合往场景中布置一个机器人(机器人位置不应该在宝石周围8各自内，应该选择空白位置)
        第二个阶段，每回合吸收玩家一个AP点数
        //
        状态变化：
        出场，展示boss warning   # 2s
        说话："Loser,Give in to me!" # 1.5s
              "But firstly,Taste my army!" #1.5s
        玩家回合，机器人回合，boss回合····
        第一个阶段结束
        第二个阶段开始，说话："you never got a chance to defeat me" #1.5s
        //
        阶段切换条件：机器人已经放置了 6 个
        失败条件：水晶被机器人获取
        成功条件：关卡任务完成     
    */

    public class BossTurn : CTurn
    {
        BattlefieldModule _bfm;
        BattleUIModule _bui;
        RobotManagerModule _rmm;
        BattlefieldInputModule _bim;

        //
        const int ENTRANCE = 0;
        const int STAGE_PRE_1 = 1;
        const int STAGE_1 = 2;
        const int STAGE_PRE_2 = 3;
        const int STAGE_2 = 4;
        const int STAGE_OVER = 5;
        const float DIALOG_TIME = 1.5f;
        const float BOSSWARNING_TIME = 2f;
        const int MAX_ROBOT_DEPLOY = 4;
        //
        string[] dialogues = {
            "Loser,Give in to me!",
            "But firstly,Taste my army!",
            "You never got a chance to defeat me!",
            "There is it!",
            "Your AP is mine."
        };
        int state = ENTRANCE;        
        float dialogWaitTimer = DIALOG_TIME;
        int idx = 0;
        int remainTalk = 0;
        int bossTalk_1_num = 2;
        int bossTalk_2_num = 1;
        int bossTalk_1_during = 3;
        int bossTalk_2_during = 4;
        int robotDeployCounter = 0;
        bool bossTalking = false;



        public BossTurn(BattleTurnsModule btm) : base(btm)
        {
            _bfm = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            _bui = ModuleManager.getInstance.GetModule<BattleUIModule>();
            _rmm = ModuleManager.getInstance.GetModule<RobotManagerModule>();
            _bim = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
        }

        public override void OnStartTurn()
        {
            base.OnStartTurn();
            //
            _bim.locked = true;
            //
            switch (state)
            {
                case ENTRANCE:
                    {
                        _bui.ShowBossWarningVFX(BOSSWARNING_TIME, GotoNextState);
                        remainTalk = bossTalk_1_num;
                        break;
                    }
                case STAGE_1:
                    {
                        var g = _bfm.Field.GetRandomGrid("Bunker.Game.NormalTile",true);
                        if (g != null)
                        {
                            var robot = _rmm.CreateRobot<RobotThief>();
                            robot.SetToGird(g);
                            robotDeployCounter++;
                            _bui.ShowBossTalkingVFX(DIALOG_TIME, dialogues[bossTalk_1_during], null);
                        }
                        break;
                    }
                case STAGE_2:
                    {
                        MissionManager.getInstance.ConsumeStep();
                        _bui.ShowBossTalkingVFX(DIALOG_TIME, dialogues[bossTalk_2_during], null);
                        break;
                    }
            }
        }

        public void GotoNextState()
        {
            state += 1;
            Debug.Log("Boss 1 state:" + state);
        }

        public override void OnEndTurn()
        {
            base.OnEndTurn();
            _bim.locked = false;
        }

        public override void OnUpdateTurn()
        {
            base.OnUpdateTurn();
            //
            switch (state)
            {
                case ENTRANCE:
                    break;
                case STAGE_PRE_1:
                    {
                        if(!bossTalking)
                        {
                            _bui.ShowBossTalkingVFX(DIALOG_TIME, dialogues[idx], GotoNextBossText);
                            bossTalking = true;
                        }
                        break;
                    }
                case STAGE_PRE_2:
                    {
                        if (!bossTalking)
                        {
                            _bui.ShowBossTalkingVFX(DIALOG_TIME, dialogues[idx], GotoNextBossText);
                            bossTalking = true;
                        }
                        break;
                    }
                case STAGE_1:
                    {
                        if (robotDeployCounter == MAX_ROBOT_DEPLOY)
                        {
                            remainTalk = bossTalk_2_num;
                            GotoNextState();
                        }
                        _battleTurnsModule.NextTurn();
                        break;
                    }
                default:
                    _battleTurnsModule.NextTurn();
                    break;
            }
        }

        public void GotoNextBossText()
        {
            remainTalk--;
            idx++;
            bossTalking = false;
            if (remainTalk == 0)
            {               
                GotoNextState();
            }
        }
    }

}
