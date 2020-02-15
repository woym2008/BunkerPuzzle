using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class BlockTile : BaseGrid
    {
        public override void Init()
        {
            base.Init();
        }

        public override bool CanMove()
        {
            return base.CanMove();
        }

        public override void UpdateGrid(int x, int y)
        {
            base.UpdateGrid(x, y);
        }

        public override bool CanElimination()
        {
            return true;
        }

        public override string GetGridType()
        {
            return this.GetType().ToString();
        }

        public override void OnEliminationed()
        {
            Debug.LogFormat("我是{0}块，我被消除了,任务系统会完成一些东西", GetGridType());
            MissionManager.getInstance.Collect(MissionCollectionType.Liquid);
        }
    }
}
