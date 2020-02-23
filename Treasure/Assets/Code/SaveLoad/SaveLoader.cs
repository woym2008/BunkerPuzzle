using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Bunker.Module;

namespace Bunker.Game
{
    public class SaveLoader : ServicesModule<SaveLoader>
    {
        public static void SaveGameProgress()
        {
            var cur_area = ModuleManager.getInstance.GetModule<BattlefieldModule>().AreaNum;
            var cur_level = ModuleManager.getInstance.GetModule<BattlefieldModule>().LevelNum;
            //1 获得当前关卡数，当前区域数，然后和其最新值去对比，如果小于，不变；如果大于，更新！
            //  区域数
            var areas_str = PlayerPrefs.GetString("area", "1");
            //默认为有序排列
            string[] areas_array = areas_str.Split(',');
            if (cur_area > int.Parse(areas_array.Last()))
            {
                areas_str = string.Format("{0},{1}", areas_str,cur_area);
                PlayerPrefs.SetString("area", areas_str);
                PlayerPrefs.SetInt(string.Format("area_{0}", cur_area), cur_level);
            }
            else
            {
                var idx = Array.FindIndex(areas_array,a => int.Parse(a) == cur_area);
                var max_level = PlayerPrefs.GetInt(string.Format("area_{0}", areas_array[idx]), 1);
                if (cur_level > max_level)
                {
                    PlayerPrefs.SetInt(string.Format("area_{0}", areas_array[idx]), cur_level);
                }
            }
            //2 保存得当前道具数，这个道具只是缓存用的（当然没想好就是玩家如果主动选择关卡，如何去做）
            //  所以也可以先保存起来这些数据
            if (Constant.save_items)
            {
                var items = BattleItemFactory.getInstance.GetItemList();
                string item_types = "";
                foreach (var item in items)
                {
                    PlayerPrefs.SetInt(item.Key, item.Value);
                    item_types = string.Format("{0},{1}", item_types, item.Key);
                }
                PlayerPrefs.SetString("item_types", item_types);
            }

        }

        public static void LoadGameProgress()
        {
            if (Constant.save_items)
            {
                string item_types = PlayerPrefs.GetString("item_types");
                string[] item_type_list = item_types.Split(',');
                foreach (var type_name in item_type_list)
                {
                    var n = PlayerPrefs.GetInt(type_name);

                    if (type_name == typeof(BattleItem_Fire).Name)
                    {
                        BattleItemFactory.getInstance.CreateBattleItem<BattleItem_Fire>(n);
                    }
                    if (type_name == typeof(BattleItem_Lightning).Name)
                    {
                        BattleItemFactory.getInstance.CreateBattleItem<BattleItem_Lightning>(n);
                    }
                    if (type_name == typeof(BattleItem_Recovery).Name)
                    {
                        BattleItemFactory.getInstance.CreateBattleItem<BattleItem_Recovery>(n);
                    }
                }
            }
        }

        public static void ClearGameProgress()
        {

        }
    }
}
