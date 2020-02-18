using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using Bunker.Module;

namespace Bunker.Game
{
    public static class GridLoader
    {
        static string _domain = "Bunker.Game";

        static Transform _rootNode;

        public static void LoadGrid(string areaName, string levelName, out IGridObject[,] reslist)
        {
            //------------------------

            //------------------------
            if (_rootNode == null)
            {
                _rootNode = new GameObject("TileRoot").transform;
            }
            var map = Resources.Load<MapData>(string.Format("{0}/{1}/{2}", "Map", areaName, levelName));

            reslist = new IGridObject[map.column, map.row];

            for (int i = 0; i < map.row; ++i)
            {
                for (int j = 0; j < map.column; ++j)
                {
                    var tiledata = map.data[i * map.row + j];

                    var resname = Constant.Tiles[tiledata];
                    var grid = CreateGrid(resname, j, i);
                    reslist[i,j] = grid;
                }
            }

            //create mask
            var mask = GameObject.Instantiate( Resources.Load("Prefabs/Tiles/TileMask")) as GameObject;
            if(mask != null)
            {
                var sm = mask.GetComponent<SpriteMask>();
                if(sm != null)
                {
                    sm.transform.parent = _rootNode;

                    sm.gameObject.transform.localScale = new Vector3(
                    (float)map.column,
                    (float)map.row + 1, 
                    0.0f
                        );

                    sm.transform.position = GridField.ZeroPos + new Vector3(
                    Constant.TileSize.x * (map.column * 0.5f) - Constant.TileSize.x * 0.5f,
                    -Constant.TileSize.y * (map.row * 0.5f) + Constant.TileSize.y * 1.0f,
                    0
                        );


                }
            }
            //Create Mission Data
            var md = map.mission;
            MissionManager.getInstance.LoadMissionData(md);
            //Create UI Mission Item
            foreach (var pair in md.Missions)
            {
                var item = ModuleManager.getInstance.GetModule<BattleUIModule>().CreateMissionItem(
                    MissionDataHelper.MCT_2_SpriteNames(pair.Key), 
                    pair.Value);
                MissionManager.getInstance.RegisterMissionChangeDelegate(pair.Key, item.OnChange);
                MissionManager.getInstance.InvokeMissionDelegate(pair.Key);
            }
            foreach (var pair in md.ProtectMissions)
            {
                var item = ModuleManager.getInstance.GetModule<BattleUIModule>().CreateMissionItem(
                    MissionDataHelper.MCT_2_SpriteNames(pair.Key),
                    pair.Value);
                MissionManager.getInstance.RegisterMissionChangeDelegate(pair.Key, item.OnChange);
                MissionManager.getInstance.InvokeMissionDelegate(pair.Key);
            }
            MissionManager.getInstance.RegisterStepChanageDelegate(
                ModuleManager.getInstance.GetModule<BattleUIModule>().GetBattleUIPanel().SetProgressNum);
            //清理一下item工场
            BattleItemFactory.getInstance.Reset();


        }

        public static BaseGrid CreateGrid(string name, int x, int y)
        {
            var type = Type.GetType(string.Format("{0}.{1}", _domain, name));
            var grid = Activator.CreateInstance(type) as BaseGrid;

            grid.CreateGrid(name,GridField.ZeroPos ,x, y);
            grid.Node.transform.parent = _rootNode;
            return grid;
        }
    }

}
