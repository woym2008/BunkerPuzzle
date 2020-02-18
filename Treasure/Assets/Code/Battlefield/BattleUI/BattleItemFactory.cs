using System;
using System.Collections.Generic;
using Bunker.Process;
using Bunker.Module;
using UnityEngine;
using UnityEngine.UI;

namespace Bunker.Game
{
    public class BattleItemFactory : ServicesModule<BattleItemFactory>
    {
        Dictionary<string, float> ItemRequests = new Dictionary<string, float>();
        //这里要控制一下，比如消除2个格子，只能得到一个物品这种
        public void CreateBattleItem<T>(float n) where T : BattleItem, new()
        {
            var item_name = typeof(T).Name;
            if (ItemRequests.ContainsKey(item_name))
            {
                ItemRequests[item_name] = ItemRequests[item_name] + n;
                while (ItemRequests[item_name] >= 1)
                {
                    ModuleManager.getInstance.GetModule<BattleUIModule>().CreateItem<T>();
                    ItemRequests[item_name] = ItemRequests[item_name] - 1;
                }
            }
            else
            {
                ItemRequests.Add(item_name, n);
            }
        }

        public void Reset()
        {          
            ItemRequests.Clear();
        }
    }
}
