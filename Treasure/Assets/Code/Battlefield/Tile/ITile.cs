using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public interface ITile
    {
        Grid ParentGrid { get; set; }



        bool CanMove();
        /// <summary>
        /// 移动到一个格子
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="movetime">Movetime.</param>
        void MoveTo(int x, int y, float movetime, bool usecopy = false);
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
        void CopyMoveTo(int currentTargetX, int currentTargetY, int startX, int startY, int endX, int endY, float movetime);
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
        /// 自己这个grid能否被消除
        /// </summary>
        bool CanElimination();
        /// <summary>
        /// 能否被别的Grid消除
        /// </summary>
        /// <returns><c>true</c>, if elimination by other was caned, <c>false</c> otherwise.</returns>
        /// <param name="gridtype">传入的消除类型.</param>
        /// <param name="direct">传入的消除位置0,1,2,3分别为上下左右</param>
        bool CanEliminationByOther(string gridtype,int direct);
        /// <summary>
        /// 执行消除
        /// </summary>
        void Elimination();
        /// <summary>
        /// 删除
        /// </summary>
        void Delete();
        /// <summary>
        /// 消除完毕后的调用
        /// </summary>
        void OnEliminationed();

        //void SetPos(int x, int y);
    }
}

