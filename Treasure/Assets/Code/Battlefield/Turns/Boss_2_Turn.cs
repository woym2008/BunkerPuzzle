using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using Bunker.Process;
using DG.Tweening;


namespace Bunker.Game
{
    /*
        此为BOSS_2 Turn
        BOSS_2 为2阶段的BOSS,在战斗此阶段内,每3回合进行对场景中释放激光射线
        Round 1:定位到场景中某一处
        Round 2:开始蓄力
        Round 3:发射激光
        Boss有生命值,在主角捡起武器后,自动释放导弹,必命中boss,boss进行受击反馈和减少生命值
        //----
        场景中共有4个Turn,分别为 "玩家,主角,小鬼,BOSS"
        主角在场景中寻觅导弹[R],欲对BOSS进行射击
        #导弹[R],在场景中只存在1个,随机生成,如被主角拾起后,随机生成下一个进行补充
        小鬼在场景中寻觅主角,小鬼触碰到主角,主角死亡,任务失败
        #小鬼在场景中最多同时存在两个,如果小鬼死亡,在BOSS回合开始时进行补充
        BOSS发射的激光无法摧毁Cube和[R],但是,可以杀死主角和小鬼.主角死亡,任务失败
        #BOSS出场的时候会先说话的

    */
    public class Boss_2_Turn : CTurn
    {
        //state
        const int ENTRANCE = 0; 
        const int STAGE_1 = 1;  //入场唠嗑
        const int STAGE_2 = 2;  //战斗阶段
        const int STAGE_3 = 3;  //结束阶段

        int state = ENTRANCE;
        const float BOSSWARNING_TIME = 2f;
        const float DIALOG_TIME = 1.5f;
        //round
        const int START_ROUND = 0;
        const int LOCATION_ROUND = 1;
        const int CHARGE_ROUND = 2;
        const int LAZER_ROUND = 3;
        int curRound = START_ROUND;
        //ghost
        const string ROBOT_TYPE_NAME = "RobotKiller_Boss_2";
        const int MAX_GHOST_COUNT = 5;
        //self body
        public Boss_2_Body boss_body;
        //property
        const int MAX_HP = 5;
        int hp = MAX_HP;
        string[] dialogues = {
            "尝尝我炙热的激光炮吧!",
            "卫兵,抓住他!",
            "让你侥幸逃脱了\n下次没这么走运",
        };
        int remainTalk = 0;
        int bossTalk_num = 2;
        int text_idx = 0;
        bool bossTalking = false;
        //lazer相关
        int lazer_finish_count = 0;
        BaseTile dest_tile;
        Vector2Int dest_grid_1 = Vector2Int.zero;
        Vector2Int dest_grid_2 = Vector2Int.zero;
        //
        BattlefieldModule _bfm;
        BattleUIModule _bui;
        RobotManagerModule _rmm;
        BattlefieldInputModule _bim;

        public Boss_2_Turn(BattleTurnsModule btm) : base(btm)
        {
            _bfm = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            _bui = ModuleManager.getInstance.GetModule<BattleUIModule>();
            _rmm = ModuleManager.getInstance.GetModule<RobotManagerModule>();
            _bim = ModuleManager.getInstance.GetModule<BattlefieldInputModule>();
            //创建自己body
            var boss_object = GameObject.Instantiate(Resources.Load("Prefabs/Boss/boss_2_body")) as GameObject;
            var wp = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 1));
            boss_object.transform.position = wp;
            boss_object.gameObject.SetActive(false);
            boss_body = boss_object.GetComponent<Boss_2_Body>();
        }

        public override void OnStartTurn()
        {
            base.OnStartTurn();       
            //锁住场景操作
            _bim.locked = true;
            //将此关卡载入时的任务系统完成后的回调断开
            MissionManager.getInstance.OnMissionValueHandler = null;
            //
            switch (state)
            {
                case ENTRANCE:  //特效
                    {
                        _bui.ShowBossWarningVFX(BOSSWARNING_TIME, GotoNextState);
                        remainTalk = bossTalk_num;
                        break;
                    }
                case STAGE_1:   //前期唠嗑                       
                        break;
                case STAGE_2:   //战斗阶段，在boss回合开始前，进行对小鬼的补充
                    {
                        int n = _rmm.GetRobotCount(ROBOT_TYPE_NAME);
                        if (n < MAX_GHOST_COUNT)
                        {

                            for ( int i = MAX_GHOST_COUNT - n;i>0;i--)
                            {
                                var t = _bfm.Field.GetRandomTile("Bunker.Game.NormalTile", true);

                                if (t != null)
                                {
                                    var robot = _rmm.CreateRobot(ROBOT_TYPE_NAME);
                                    robot.SetToGird(t.ParentGrid);
                                }
                            }
                        }
                        //
                        if (curRound == LOCATION_ROUND)
                        {
                            //随机找到特殊格子，及其位置,播放移动动画
                            Vector2Int rnd = new Vector2Int(UnityEngine.Random.Range(1,8), 9);
                            dest_tile = _bfm.Field.GetTile(rnd.x, rnd.y);
                            var dest_pos = new Vector3(dest_tile.Node.transform.position.x,
                                boss_body.transform.position.y,
                                boss_body.transform.position.z);
                            boss_body.transform.DOMove(dest_pos, 1.2f).OnComplete(CB_OnFinishMovement);
                        }
                        else if (curRound == CHARGE_ROUND)
                        {
                            //播放枪口处的充能动画 或者是充能特效
                            boss_body.OnCharge();
                            CB_OnFinishMovement();
                        }
                        else if (curRound == LAZER_ROUND)
                        {
                            const float lazer_speed = 30f;
                            const float bias = .5f;
                            //释放lazer
                            float dist_1 = 20;
                            float dist_2 = 20;
                            float dist_1_time = 0;
                            float dist_2_time = 0;
                            //两个起始点，进行向前扫描
                            var t_1 = _bfm.Field.GetTile(dest_tile.ParentGrid.ColID + 1, dest_tile.ParentGrid.RowID);
                            var t_2 = _bfm.Field.GetTile(dest_tile.ParentGrid.ColID - 1, dest_tile.ParentGrid.RowID);
                            if(t_1 != null)
                            {
                                int grid_x = dest_tile.ParentGrid.ColID + 1;
                                int grid_y = 9; //此处应该是mapsize
                                for (int y = grid_y; y>=0;y--)
                                {
                                    if (CanBlockLazer(grid_x, y))
                                    {
                                        var t = _bfm.Field.GetTile(grid_x, y);
                                        dist_1 = Mathf.Abs(t.Node.transform.position.y -
                                            boss_body.Emiter_1.position.y) + bias;
                                        dest_grid_1.Set(grid_x, y);
                                        CDebug.Log(string.Format("创建激光[1]，距离{0}，时间{1} | GRID：{2},{3}", dist_1, dist_1_time, grid_x, y), Constant.COLOR_NAME.ORANGE);
                                        break;
                                    }                                    
                                }
                                //
                                dist_1_time = dist_1 / lazer_speed;

                                Sequence mySequence = DOTween.Sequence();
                                mySequence.Append(DOTween.To(CB_Lazer_1_HeightUpdate, 1.5f, dist_1, dist_1_time))
                                    .AppendInterval(.5f).AppendCallback(CB_Lazer_Shoot_Finish);

                            }
                            if (t_2 != null)
                            {
                                int grid_x = dest_tile.ParentGrid.ColID - 1;
                                int grid_y = 9; //此处应该是mapsize
                                for (int y = grid_y; y >= 0; y--)
                                {
                                    if (CanBlockLazer(grid_x, y))
                                    {
                                        var t = _bfm.Field.GetTile(grid_x, y);
                                        dist_2 = Mathf.Abs(t.Node.transform.position.y -
                                            boss_body.Emiter_2.position.y) + bias;
                                        dest_grid_2.Set(grid_x, y);
                                        CDebug.Log(string.Format("创建激光[1]，距离{0}，时间{1} | GRID：{2},{3}", dist_2, dist_2_time, grid_x, y), Constant.COLOR_NAME.ORANGE);
                                        break;
                                    }
                                }
                                //
                                dist_2_time = dist_2 / lazer_speed;

                                Sequence mySequence = DOTween.Sequence();
                                mySequence.Append(DOTween.To(CB_Lazer_2_HeightUpdate, 1.5f, dist_2, dist_2_time))
                                    .AppendInterval(.5f).AppendCallback(CB_Lazer_Shoot_Finish);
                            }
                            //
                            boss_body.OnShoot();
                        }
                    }
                    break;
                case STAGE_3:
                    break;
            }
        }

        public override void OnUpdateTurn()
        {
            //base.OnUpdateTurn(); //此处基类只是在debug
            switch (state)
            {
                case ENTRANCE:
                    break;
                case STAGE_1:
                    {
                        if (!bossTalking)
                        {
                            _bui.ShowBossTalkingVFX(DIALOG_TIME, dialogues[text_idx], GotoNextBossText);
                            bossTalking = true;
                        }
                    }
                    break;
                case STAGE_2:
                    {
                        if (curRound == START_ROUND)
                        {
                            //显示boss主体
                            boss_body.gameObject.SetActive(true);
                            //直接跳转
                            CB_OnFinishMovement();
                        }                        
                    }
                    break;
                case STAGE_3:
                    {
                        if (!bossTalking)
                        {
                            _bui.ShowBossTalkingVFX(DIALOG_TIME, dialogues[text_idx], null);
                            bossTalking = true;
                            _battleTurnsModule.NextTurn();

                        }
                    }
                    break;
            }
        }

        public override void OnEndTurn()
        {
            base.OnEndTurn();
            _bim.locked = false;
            switch (state)
            {
                case STAGE_3:
                    ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_WIN);
                    break;
            }
        }

        public void GotoNextBossText()
        {
            remainTalk--;
            text_idx++;
            bossTalking = false;
            if (remainTalk == 0)
            {
                GotoNextState();
            }
        }

        public void GotoNextState()
        {
            state += 1;
            Debug.Log("Boss 1 state:" + state);
        }

        public void FinishRound()
        {
            curRound++;
            if (curRound > LAZER_ROUND) curRound = LOCATION_ROUND;
        }

        public void DecreaseHP()
        {
            hp--;
            MissionManager.getInstance.Collect(MissionCollectionType.Boss);
            CDebug.Log(string.Format("class:{0} , hp = {1}", CDebug.GetFileName(), hp),Constant.COLOR_NAME.GREEN);
            if (hp == 0)
            {
                //boss to dead,可以此处释放boss死亡特效
                boss_body.gameObject.SetActive(false);
                GotoNextState();
            }
            else
            {
                var sr = boss_body.GetComponent<SpriteRenderer>();
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(sr.DOColor(Color.red, .08f))
                    .Append(sr.DOColor(Color.white, .08f))
                    .Append(sr.DOColor(Color.red, .08f))
                    .Append(sr.DOColor(Color.white, .08f));
            }
        }
        //
        public void CB_OnFinishMovement()
        {
            FinishRound();
            _battleTurnsModule.NextTurn();
        }

        public void CB_Lazer_1_HeightUpdate(float h)
        {
            boss_body.SetLazer_1_Height(h);
        }
        public void CB_Lazer_2_HeightUpdate(float h)
        {
            boss_body.SetLazer_2_Height(h);
        }
        public void CB_Lazer_Shoot_Finish()
        {
            lazer_finish_count++;
            //可以播放激光fallout动画
            if (lazer_finish_count == 2)
            {
                lazer_finish_count = 0;
                //消除格子上的机器人
                {
                    RobotBase bot = null;
                    bot = _rmm.GetRobot(dest_grid_1.x, dest_grid_1.y);
                    if (bot != null) _rmm.RemoveRobot(bot);
                    if(bot is RobotPorter_Boss_2)
                        ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_LOSE);
                    bot = _rmm.GetRobot(dest_grid_2.x, dest_grid_2.y);
                    if (bot != null) _rmm.RemoveRobot(bot);
                    if (bot is RobotPorter_Boss_2)
                        ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_LOSE);
                }
                //
                boss_body.OnIdle();
                //
                FinishRound();
                _battleTurnsModule.NextTurn();
            }
        }

        public bool CanBlockLazer(int col, int row)
        {
            var t = _bfm.Field.GetTile(col, row);
            if (t == null) return false;
            if (!(t is NormalTile) && !(t is PorterStartTile)) return true;
            if(_rmm.GetRobot(col, row) != null)
            {
                return true;
            }
            return false;
        }
    }
}
