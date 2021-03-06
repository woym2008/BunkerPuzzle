﻿using UnityEngine;
using System.Collections;
using Bunker.Game;
using Bunker.Module;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class LaserEmitter : MonoBehaviour
{
    //基本数据
    const float LazerSpeed = 100f;
    const float Bias = .5f;
    //释放lazer
    const float maxlength = 10;
    //------------------------------------------
    Bunker.Game.Grid _destGrid;
    //------------------------------------------
    BattlefieldModule _bfm;
    RobotManagerModule _rmm;

    public Action<float> OnUpdate;
    public Action OnFinish;

    List<Type> blockTileTypes;
    //------------------------------------------
    public SpriteRenderer _laserSprite;
    //------------------------------------------
    public void Init()
    {
        _bfm = ModuleManager.getInstance.GetModule<BattlefieldModule>();
        _rmm = ModuleManager.getInstance.GetModule<RobotManagerModule>();

        if(_laserSprite == null)
        {
            _laserSprite = this.gameObject.GetComponent<SpriteRenderer>();
        }

       //blockTileTypes = new Type[1];
        //blockTileTypes[0] = typeof(BlockTile);

        _laserSprite.sortingOrder = 100;
    }
    public void AddBlockTileType(Type type)
    {
        if(blockTileTypes == null)
        {
            blockTileTypes = new List<Type>();
        }
        blockTileTypes.Add(type);
    }
    public void Emit(Vector3 strartpos, Bunker.Game.Grid destGrid, Vector2Int dir)
    {
        float dist = maxlength;
        //两个起始点，进行向前扫描
        if (dir.x == 0)
        {
            var gridY = destGrid.RowID;
            var gridX = destGrid.ColID;

            var grid = destGrid;

            _destGrid = null;
            while (grid != null)
            {
                if(grid != null && grid.AttachTile != null)
                {
                    var tile = grid.AttachTile;

                    if(CanBlockLazer(tile))
                    {
                        dist = Mathf.Abs(tile.Node.transform.position.y -
                        strartpos.y) + Bias;
                        _destGrid = grid;

                        break;
                    }

                    gridY += dir.y;

                    grid = _bfm.Field.GetGrid(gridX, gridY);
                }
            }
        }
        else if(dir.y == 0)
        {
            var gridY = destGrid.RowID;
            var gridX = destGrid.ColID;

            var grid = destGrid;

            _destGrid = null;
            while (grid != null)
            {
                if (grid != null && grid.AttachTile != null)
                {
                    var tile = grid.AttachTile;

                    if (CanBlockLazer(tile))
                    {
                        dist = Mathf.Abs(tile.Node.transform.position.x -
                        strartpos.x) + Bias;
                        _destGrid = grid;

                        break;
                    }
                }
                gridX += dir.x;

                grid = _bfm.Field.GetGrid(gridX, gridY);
            }
        }
        var dist_time = dist / LazerSpeed;
        Debug.Log("dist_time: " + dist_time);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(DOTween.To(CB_GetHeightUpdate, CB_Lazer_HeightUpdate, dist, dist_time))
           .AppendInterval(.5f).AppendCallback(CB_Lazer_Shoot_Finish);

        //DOTween.To(CB_GetHeightUpdate, CB_Lazer_HeightUpdate, dist, dist_time);

        //StartCoroutine(StepDist(CB_Lazer_HeightUpdate, 0, dist, dist_time, CB_Lazer_Shoot_Finish));
    }

    IEnumerator StepDist(Action<float> heightupdate, float startheight,float dist, float time, Action onfinish)
    {
        yield return 0;
        float currentdist = startheight;

        float dt_dist = dist / time;

        while (currentdist < dist)
        {
            currentdist += dt_dist;

            heightupdate.Invoke(currentdist);

            Debug.LogWarning(currentdist + "/" + dist);

            yield return 0;
        }

        heightupdate.Invoke(dist);

        onfinish.Invoke();
    }
    private void Update()
    {

    }

    public void Emit(Bunker.Game.Grid startGrid, Bunker.Game.Grid destGrid)
    {
        var startpos = _bfm.Field.GetWorldPos(startGrid.ColID, startGrid.RowID);

        var dir = new Vector2Int(
            destGrid.ColID - startGrid.ColID,
            destGrid.RowID - startGrid.RowID
            );

        Emit(startpos, destGrid, dir);
    }

    public void Emit(Bunker.Game.Grid startGrid, Vector2Int dir)
    {
        var startpos = _bfm.Field.GetWorldPos(startGrid.ColID, startGrid.RowID);

        var destGrid = _bfm.Field.GetGrid(startGrid.ColID + dir.x, startGrid.RowID + dir.y);
        if(destGrid == null)
        {
            return;
        }

        Emit(startpos, destGrid, dir);
    }

    public void Stop()
    {
        _laserSprite.size = new Vector2(1, 0);
    }

    public void SetSize(float x, float y)
    {

    }
    //--------------------------------------------------------------------------
    public float CB_GetHeightUpdate()
    {
        return _laserSprite.size.y; 
    }
    public void CB_Lazer_HeightUpdate(float h)
    {
        Debug.LogWarning("CB_Lazer_HeightUpdate: " + h);

        _laserSprite.size = new Vector2(1, h);

        OnUpdate?.Invoke(h);
    }
    public void CB_Lazer_Shoot_Finish()
    {
        OnFinish?.Invoke();

        if(_destGrid == null)
        {
            return;
        }

        RobotBase bot = null;
        bot = _rmm.GetRobot(_destGrid.ColID, _destGrid.RowID);
        if (bot != null) _rmm.RemoveRobot(bot);
        //if (bot is RobotPorter_Boss_2)
        //    ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_LOSE);
        //bot = _rmm.GetRobot(dest_grid_2.x, dest_grid_2.y);
        //if (bot != null) _rmm.RemoveRobot(bot);
        //if (bot is RobotPorter_Boss_2)

        //            ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_LOSE);
        //lazer_finish_count++;
        ////可以播放激光fallout动画
        //if (lazer_finish_count == 2)
        //{
        //    lazer_finish_count = 0;
        //    //消除格子上的机器人
        //    {
        //        RobotBase bot = null;
        //        bot = _rmm.GetRobot(dest_grid_1.x, dest_grid_1.y);
        //        if (bot != null) _rmm.RemoveRobot(bot);
        //        if (bot is RobotPorter_Boss_2)
        //            ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_LOSE);
        //        bot = _rmm.GetRobot(dest_grid_2.x, dest_grid_2.y);
        //        if (bot != null) _rmm.RemoveRobot(bot);
        //        if (bot is RobotPorter_Boss_2)
        //            ProcessManager.getInstance.Switch<EndMenuProcess>(Bunker.Game.EndMenuProcess.END_GAME_LOSE);
        //    }
        //}
    }
    private bool CanBlockLazer(BaseTile tile)
    {
        if (tile == null) return false;
        //if (!(t is NormalTile) && !(t is PorterStartTile)) return true;        

        foreach (var type in blockTileTypes)
        {
            if (tile.GetType() == type)
            {
                return true;
            }
            if(_rmm?.GetRobot(tile.ParentGrid.ColID, tile.ParentGrid.RowID))
            {
                return true;
            }
        }
        return false;
    }
}
