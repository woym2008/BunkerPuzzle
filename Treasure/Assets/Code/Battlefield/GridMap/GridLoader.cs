using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using Bunker.Module;

namespace Bunker.Game
{
    public static class GridLoader
    {
        //static string _domain = "Bunker.Game";

        static Transform _rootNode;

        public static bool LoadGrid(string areaName, string levelName, out Grid[] rows, out Grid[] cols, out Grid[,] grids)
        {
            //------------------------

            //------------------------
            if (_rootNode == null)
            {
                _rootNode = new GameObject("TileRoot").transform;
            }
            var map = Resources.Load<MapData>(string.Format("{0}/{1}/{2}", "Map", areaName, levelName));

            if (map == null)
            {
                //这里不应该找不到，如果找不到，应该是通关了！！
                rows = null;
                cols = null;
                grids = null;
                Debug.Log(string.Format("Map {0}/{1} not found", areaName, levelName));
                return false;
            }

            var rmax = map.row;
            var cmax = map.column;

            var offsetr = Constant.MaxRow - rmax;
            var offsetc = Constant.MaxCol - cmax;

            GridField.zeroOffset = new Vector3(
                Constant.TileSize.x * offsetc * 0.5f,
                -Constant.TileSize.y * offsetr * 0.5f,
                0
                );

            //这里需要
            //1 保存各个行和各个列的首节点
            //2 对各个节点的上下左右节点进行赋值，通过缓存上一行的节点和同行的前一个节点来实现
            //如果出现了rows[x] == null的情况，说明这一行都没有数据 cols同理
            rows = new Grid[map.row];
            cols = new Grid[map.column];

            grids = new Grid[map.column, map.row];

            //缓存读入的在同一行上的上一个块
            //cache current
            Grid lastTileInRow = null;
            Grid firstTileInRow = null;
            //缓存读入的上一行上的同一列上的上一个块
            //cache1 cache2 cache3
            //1      2      3
            Grid[] lastRowTileInCol = new Grid[map.column];
            Grid[] firstRowTileInCol = new Grid[map.column];
            for (int i = 0; i < map.row; ++i)
            {
                lastTileInRow = null;//换行了，清空这个
                firstTileInRow = null;
                for (int j = 0; j < map.column; ++j)
                {
                    var tiledata = map.data[i * map.row + j];

                    var resname = Constant.Tiles[tiledata];

                    var grid = new Grid(j, i);
                    grid.AttachTile = null;
                    grid.Up = null;
                    grid.Down = null;
                    grid.Left = null;
                    grid.Right = null;

                    BaseTile tile = null;

                    //是否为空墙
                    if (resname != "Empty")
                    {
                        tile = CreateTile(resname, grid);

                        grid.AttachTile = tile;
                        tile.ParentGrid = grid;
                    }

                    //行列的开头节点
                    if (i == 0 && grid.AttachTile != null)
                    {
                        cols[j] = grid;
                    }
                    if (j == 0 && grid.AttachTile != null)
                    {
                        rows[i] = grid;
                    }

                    var lastTileInCol = lastRowTileInCol[j];
                    //四周节点
                    //上下
                    if(grid.AttachTile != null)
                    {
                        //如果之前cols[j]为空,说明第一行的第j列是空的,那么这一列的第一个节点就要向下顺延
                        if (cols[j] == null)
                        {
                            cols[j] = grid;
                        }

                        if (lastTileInCol == null)
                        {
                            grid.Up = null;
                            grid.Down = null;
                            firstRowTileInCol[j] = grid;
                            if(cols[j] == null)
                                cols[j] = grid;
                        }
                        else
                        {
                            grid.Up = lastTileInCol;
                            lastTileInCol.Down = grid;
                        }
                        lastRowTileInCol[j] = grid;
                        
                    }

                    //左右
                    if(grid.AttachTile != null)
                    {
                        //if (rows[i] == null)
                        //{
                        //    rows[i] = grid;
                        //}

                        if (lastTileInRow == null)
                        {
                            firstTileInRow = grid;
                            grid.Left = null;
                            grid.Right = null;
                            if (rows[i] == null)
                                rows[i] = grid;
                        }
                        else
                        {
                            grid.Left = lastTileInRow;
                            lastTileInRow.Right = grid;
                        }
                        lastTileInRow = grid;

                    }

                    grids[j, i] = grid;
                }

                if(lastTileInRow != null && firstTileInRow != null)
                {
                    lastTileInRow.Right = firstTileInRow;
                    firstTileInRow.Left = lastTileInRow;
                }
            }

            for(int i=0;i< lastRowTileInCol.Length; ++i)
            {
                var first = firstRowTileInCol[i];
                var last = lastRowTileInCol[i];
                if(first != null && last != null)
                {
                    first.Up = last;
                    last.Down = first;
                }
            }

            //create mask
            //var mask = GameObject.Instantiate( Resources.Load("Prefabs/Tiles/TileMask")) as GameObject;
            //if(mask != null)
            //{
            //    var sm = mask.GetComponent<SpriteMask>();
            //    if(sm != null)
            //    {
            //        sm.transform.parent = _rootNode;

            //        sm.gameObject.transform.localScale = new Vector3(
            //        (float)map.column,
            //        (float)map.row + 1, 
            //        0.0f
            //            );

            //        sm.transform.position = GridField.ZeroPos + new Vector3(
            //        Constant.TileSize.x * (map.column * 0.5f) - Constant.TileSize.x * 0.5f,
            //        -Constant.TileSize.y * (map.row * 0.5f) + Constant.TileSize.y * 1.0f,
            //        0
            //            );


            //    }
            //}
            //Create Mission Data
            var md = map.mission;
            MissionManager.getInstance.LoadMissionData(md);
            //Create UI Mission Item
            foreach (var pair in md.Missions)
            {
                var item = ModuleManager.getInstance.GetModule<BattleUIModule>().CreateMissionItem(
                    MissionDataHelper.MCT_2_SpriteNames(pair.Key), 
                    pair.Value);
                MissionManager.getInstance.RegisterMissionItemPos(pair.Key, item.transform as RectTransform);
                MissionManager.getInstance.RegisterMissionChangeDelegate(pair.Key, item.OnChange);
                MissionManager.getInstance.InvokeMissionDelegate(pair.Key);
            }
            foreach (var pair in md.ProtectMissions)
            {
                var item = ModuleManager.getInstance.GetModule<BattleUIModule>().CreateMissionItem(
                    MissionDataHelper.MCT_2_SpriteNames(pair.Key),
                    pair.Value);
                MissionManager.getInstance.RegisterMissionItemPos(pair.Key, item.transform as RectTransform);
                MissionManager.getInstance.RegisterMissionChangeDelegate(pair.Key, item.OnChange);
                MissionManager.getInstance.InvokeMissionDelegate(pair.Key);
            }
            MissionManager.getInstance.RegisterStepChanageDelegate(
                //ModuleManager.getInstance.GetModule<BattleUIModule>().GetBattleUIPanel().SetProgressNum);
                ModuleManager.getInstance.GetModule<BattleUIModule>().GetBattleUIPanel().SetAPNum);
            //UI level 的设置
            ModuleManager.getInstance.GetModule<BattleUIModule>().GetBattleUIPanel().SetLevelText(
                ModuleManager.getInstance.GetModule<BattlefieldModule>().LevelNum
            );
            //这张地图是否有boss
            ModuleManager.getInstance.GetModule<BattleTurnsModule>().InsertTurn(map.boss_type,true);
            //清理一下item工场
            BattleItemFactory.getInstance.Reset();
            //add by wwh 2021-4-19
            //将有效的list传入robotManager
            ModuleManager.getInstance.GetModule<RobotManagerModule>().UpdateEnemyTypeList(map.enemy_types);
            ModuleManager.getInstance.GetModule<RobotManagerModule>().UpdateFriendlyTypeList(map.friendly_types);
            //add by wwh 2021-5-12 读取主角的组件，然后动态生成出主角
            if(map.starredComponents != null)
            {
                ModuleManager.getInstance.GetModule<StarredModule>().CreateStarringRole(map.starredComponents);
            }

            return true;
        }

        public static BaseTile CreateTile(string name, Grid grid)
        {
            var type = Type.GetType(string.Format("{0}{1}", Constant.DOMAIN_PREFIX, name));
            Debug.Log(name);
            var tile = Activator.CreateInstance(type) as BaseTile;

            tile.Create(name, GridField.ZeroPos , grid);
            tile.Node.transform.parent = _rootNode;
            return tile;
        }
    }

}
