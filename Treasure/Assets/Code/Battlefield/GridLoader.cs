﻿using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

namespace Bunker.Game
{
    public static class GridLoader
    {
        static string _domain = "Bunker.Game";

        static Transform _rootNode;

        public static void LoadGrid(string name, out IGridObject[,] reslist)
        {
            if(_rootNode == null)
            {
                _rootNode = new GameObject("TileRoot").transform;
            }
            var map = Resources.Load<MapData>(string.Format("{0}/{1}", "Map", name));

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
                    (float)map.column * 1.5f,
                    (float)map.row * 1.5f, 
                    0.0f
                        );

                    sm.transform.position = GridField.ZeroPos + new Vector3(
                    Constant.TileSize.x * ((float)map.column * 0.5f),
                    -Constant.TileSize.y * ((float)map.row * 0.5f),
                    0
                        );


                }
            }
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
