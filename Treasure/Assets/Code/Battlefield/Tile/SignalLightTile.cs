using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    public class SignalLightTile : BaseGrid
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
            Debug.LogFormat("我是{0}块，我被消除了,会获得一个道具", GetGridType());
            //尝试2个获得一个道具,一个只占0.5
            BattleItemFactory.getInstance.CreateBattleItem<BattleItem_Fire>(0.5f);
        }
    }
}
