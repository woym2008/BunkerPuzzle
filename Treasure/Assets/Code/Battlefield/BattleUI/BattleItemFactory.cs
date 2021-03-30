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
        Dictionary<string, int> ItemList = new Dictionary<string, int>();
        //这里要控制一下，比如消除2个格子，只能得到一个物品这种
        /*
            这里的ITEM的生成规则是，可以生成任意个道具，比如0.1、0.5 、1和2等，每当满足1个完整道具的时候，就会给
            游戏UI中实例化一个完整的道具，且此时这里也有保存
        */
        public void CreateBattleItem<T>(float n) where T : BattleItem, new()
        {
            var item_name = typeof(T).Name;
            if (ItemRequests.ContainsKey(item_name))
            {
                ItemRequests[item_name] = ItemRequests[item_name] + n;                
            }
            else
            {
                ItemRequests.Add(item_name, n);                
            }
            //
            while (ItemRequests[item_name] >= 1)
            {
                ModuleManager.getInstance.GetModule<BattleUIModule>().CreateItem<T>();
                ItemRequests[item_name] = ItemRequests[item_name] - 1;
                ItemList[item_name] = ItemList[item_name] + 1;
            }
        }

        public void Reset()
        {          
            ItemRequests.Clear();
            ItemList.Clear();
        }

        public void ClearItemRequestsList(float threshold = 1.0f)
        {
            foreach (var pair in ItemRequests)
            {
                if (pair.Value < threshold)
                {
                    ItemRequests[pair.Key] = 0;
                }
            }
        }

        public void ConsumeItem<T>() where T : BattleItem, new()
        {
            var item_name = typeof(T).Name;
            if (ItemList.ContainsKey(item_name))
            {
                ItemList[item_name] = ItemList[item_name] - 1;
                if (ItemList[item_name] < 0)
                {
                    Debug.Log("ConsumeItem Error");
                }
            }
            else
            {
                Debug.Log("ConsumeItem Error");
            }
        }

        public Dictionary<string, int> GetItemList() { return ItemList; }

        public void RegistItemType<T>() where T : BattleItem, new()
        {
            var item_name = typeof(T).Name;
            if (!ItemList.ContainsKey(item_name))
            {
                ItemList.Add(item_name, 0);
            }
        }
    }
}
