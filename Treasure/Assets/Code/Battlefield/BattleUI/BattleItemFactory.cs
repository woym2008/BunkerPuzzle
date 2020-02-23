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
                    ItemList[item_name] = ItemList[item_name] + 1;
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
            ItemList.Clear();
        }

        public void ConsumeItem<T>() where T : BattleItem, new()
        {
            var item_name = typeof(T).Name;
            if (ItemRequests.ContainsKey(item_name))
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
