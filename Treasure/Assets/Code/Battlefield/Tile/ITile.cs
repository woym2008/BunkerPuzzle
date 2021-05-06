using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public interface ITile
    {
        Grid ParentGrid { get; set; }



        //bool CanMove();
        /// <summary>
        /// 移动到一个格子
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="movetime">Movetime.</param>
        /// <param name="direct">方向，-2 左 -1 右 1 上 2 下.</param>
        void MoveTo(int x, int y, float movetime, int direct, bool usecopy = false);
        /// <summary>
        /// 复制一个一摸一样的镜像格子移动
        /// </summary>
        /// <param name="currentTargetX">当前块要去的位置x.</param>
        /// <param name="currentTargetY">当前块要去的位置y.</param>
        /// <param name="startX">复制块的起点位置Start x.</param>
        /// <param name="startY">复制块的起点位置Start y.</param>
        /// <param name="endX">复制块要去的位置End x.</param>
        /// <param name="endY">复制块要去的位置End y.</param>
        /// <param name="movetime">Movetime.</param>
        /// <param name="direct">方向，-2 左 -1 右 1 上 2 下.</param>
        void CopyMoveTo(int currentTargetX, int currentTargetY, int startX, int startY, int endX, int endY, float movetime, int direct);
        /// <summary>
        /// 格子的update
        /// </summary>
        /// <param name="Grid">The x coordinate And The y coordinate.</param>
        void UpdateGrid(Grid grid);
        /// <summary>
        /// 获取格子类型
        /// </summary>
        /// <returns>The grid type.</returns>
        string GetGridType();

        /// <summary>
        /// 删除
        /// </summary>
        void Delete();

        //void SetPos(int x, int y);
    }
}

