using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using Bunker.Process;

namespace Bunker.Game.PathFinder
{
    public class AStar
    {
        #region ASTAR NODE
        public struct Node
        {
            public int x;
            public int y;
            public int parent_index;
            public double g;
            public int h;
            public double f;
            public Node(int _x, int _y, int _parent_index, int _g, int _h, int _f)
            {
                x = _x;
                y = _y;
                parent_index = _parent_index;
                g = _g;
                h = _h;
                f = _f;
            }

        }
        #endregion

        Vector2Int[] DIR_4 = new Vector2Int[] {
                    new Vector2Int(-1,0) ,
                    new Vector2Int(1,0) ,
                    new Vector2Int(0,1) ,
                    new Vector2Int(0,-1) };

        int[][] board = null;
        public AStar(int[][] map)
        {
            UpdateMap(map);
        }

        public void UpdateMap(int[][] map)
        {
            board = map;
        }

        public Vector2Int[] FindPath(Vector2Int _start, Vector2Int _destination)
        {
            var columns = board.Length;
            var rows = board[0].Length;

            var start = new Node(_start.x, _start.y, -1, -1, -1, -1);
            var destination = new Node(_destination.x, _destination.y, -1, -1, -1, -1);

            var open = new List<Node>(); //将要访问的node
            var closed = new List<Node>(); // 已经访问的node

            var g = 0; 
            var h = heuristic(start, destination); 
            var f = g + h; 

            open.Add(start);    //将起始点放到open列表中

            while (open.Count > 0)
            {
                //可以保持open列表按f值从低到高排序，在这种情况下，总是使用第一个节点
                //现在的open列表是傻瓜寻找最值~
                var best_cost = open[0].f;
                var best_node = 0;

                for (var i = 1; i < open.Count; i++)
                {
                    if (open[i].f < best_cost)
                    {
                        best_cost = open[i].f;
                        best_node = i;
                    }
                }

                //设置我们当前访问的点
                var current_node = open[best_node];

                //检查是否已经到了我们的目的地
                if (current_node.x == destination.x && current_node.y == destination.y)
                {
                    var path = new LinkedList<Node>(); //用目的地node初始化路径
                    path.AddFirst(destination);

                    while (current_node.parent_index != -1)
                    {
                        current_node = closed[current_node.parent_index];
                        path.AddFirst(current_node);
                    }

                    Vector2Int[] final_path = new Vector2Int[path.Count];
                    int i = 0;
                    foreach (var n in path)
                    {
                        final_path[i] = new Vector2Int(n.x, n.y);
                        i++;
                    }
                    
                    return final_path;
                }

                open.RemoveAt(best_node);

                closed.Add(current_node);


                //检查4方向
                for (int j = 0;j < DIR_4.Length ; ++j)
                {
                    var new_node_x = current_node.x + DIR_4[j].x;
                    var new_node_y = current_node.y + DIR_4[j].y;
                    //不符合条件
                    if(new_node_x < 0 || new_node_x >= columns || new_node_y < 0 || new_node_y >= rows)
                    {
                        continue;
                    }
                    // 如果新的节点可以通过，或者新的节点就是目标点
                    if (board[new_node_x][new_node_y] == 1 
                        || (destination.x == new_node_x && destination.y == new_node_y)) 
                    {
                        //如果这个节点在我们的close列表中，就跳过
                        var found_in_closed = false;
						foreach (var n in closed)
                        {
                            if (n.x == new_node_x && n.y == new_node_y)
                            {
                                found_in_closed = true;
                                break;
                            }
                        }


                        if (found_in_closed)
                            continue;

                        // 如果这个结点在open列表中，使用它
                        var found_in_open = false;
						foreach (var n in open)
                        {
                            if (n.x == new_node_x && n.y == new_node_y)
                            {
                                found_in_open = true;
                                break;
                            }
                        }

                        if (!found_in_open)
                        {
                            var new_node = new Node(new_node_x, new_node_y, closed.Count - 1, -1, -1, -1);

                            new_node.g = current_node.g + 
                                Math.Floor(Math.Sqrt(Math.Pow(new_node.x - current_node.x, 2) + Math.Pow(new_node.y - current_node.y, 2)));
                            new_node.h = this.heuristic(new_node, destination);
                            new_node.f = new_node.g + new_node.h;
                            open.Add(new_node);
                        }
                    }
                }
            }

            return null;
        }

        public int heuristic(Node current_node, Node destination_node)
        {
            var x = current_node.x - destination_node.x;
            var y = current_node.y - destination_node.y;
            return x * x + y * y;
        }
    }    
}
