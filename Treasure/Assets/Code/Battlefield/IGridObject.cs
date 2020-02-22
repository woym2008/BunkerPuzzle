using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public interface IGridObject
    {
        int X
        {
            get;
        }
        int Y
        {
            get;
        }

        bool CanMove();
        /// <summary>
        /// 移动到一个格子
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="movetime">Movetime.</param>
        void MoveTo(int x, int y, float movetime);
        /// <summary>
        /// 复制一个一摸一样的镜像格子移动
        /// </summary>
        /// <param name="startX">Start x.</param>
        /// <param name="startY">Start y.</param>
        /// <param name="endX">End x.</param>
        /// <param name="endY">End y.</param>
        /// <param name="movetime">Movetime.</param>
        void CopyMoveTo(int startX, int startY, int endX, int endY, float movetime);
        /// <summary>
        /// 格子的update
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        void UpdateGrid(int x, int y);
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

