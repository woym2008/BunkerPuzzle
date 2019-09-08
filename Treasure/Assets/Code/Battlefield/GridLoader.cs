using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

namespace Bunker.Game
{
    public static class GridLoader
    {
        static string _domain = "Bunker.Game";

        public static void LoadGrid(string name, out IGridObject[,] reslist)
        {
            var map = Resources.Load<MapData>(string.Format("{0}/{1}", "Map", name));

            reslist = new IGridObject[map.column, map.row];

            for (int i = 0; i < map.row; ++i)
            {
                for (int j = 0; j < map.column; ++j)
                {
                    var tiledata = map.data[i * map.row + j];

                    var resname = Constant.Tiles[tiledata];
                    var grid = CreateGrid(resname, j, i);
                    reslist[j,i] = grid;
                }
            }
        }

        public static BaseGrid CreateGrid(string name, int x, int y)
        {
            var type = Type.GetType(string.Format("{0}.{1}", _domain, name));
            var grid = Activator.CreateInstance(type) as BaseGrid;

            grid.CreateGrid(name,GridField.ZeroPos ,x, y);

            return grid;
        }
    }

}
