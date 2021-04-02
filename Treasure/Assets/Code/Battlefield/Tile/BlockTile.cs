using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class BlockTile : BaseTile
    {
        public override void Init()
        {
            base.Init();
        }

        public override bool CanMove()
        {
            return base.CanMove();
        }

        public override void UpdateGrid(Grid grid)
        {
            base.UpdateGrid(grid);
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
            Vector3 dest = Vector3.zero;
            if (MissionManager.getInstance.GetMissionItemPos(MissionCollectionType.Liquid,ref dest))
            {
                VFXManager.getInstance.VFX_RedSPR(GetSelfWorldPos(), dest);
            }
        }
    }
}
