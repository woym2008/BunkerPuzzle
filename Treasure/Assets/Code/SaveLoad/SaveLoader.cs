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
        public void SaveGameCurProgress(int cur_area,int cur_level)
        {
            //cur_area = ModuleManager.getInstance.GetModule<BattlefieldModule>().AreaNum;
            //cur_level = ModuleManager.getInstance.GetModule<BattlefieldModule>().LevelNum;
            //1 获得当前关卡数，当前区域数，然后和其最新值去对比，如果小于，不变；如果大于，更新！
            //  区域数
            int max_area = LevelManager.getInstance.MaxArea;
            string sz_cur_area_detail = string.Format("area_{0}_detail", cur_area);
            //"1,1" means 关卡激活到 1,当前关为 1
            var cur_area_detail = PlayerPrefs.GetString(sz_cur_area_detail, "1,1");
            var area_detail_pair = cur_area_detail.Split(',');
            var area_active_level = int.Parse(area_detail_pair[0]);
            //var area_cur_level = int.Parse(area_detail_pair[1]);
            //将区域当前level更新
            if (cur_level > area_active_level)
            {
                area_active_level = cur_level;
            }
            cur_area_detail = string.Format("{0},{1}", area_active_level, cur_level);

            PlayerPrefs.SetString(sz_cur_area_detail, cur_area_detail);
        }
    
        //这里用于选关界面
        public bool LoadGameCurProgress(int cur_area,ref int active_level,ref int cur_level)
        { 
            string sz_cur_area_detail = string.Format("area_{0}_detail", cur_area);
            var cur_area_detail = PlayerPrefs.GetString(sz_cur_area_detail);
            if(cur_area_detail != "")
            {
                var area_detail_pair = cur_area_detail.Split(',');
                active_level = int.Parse(area_detail_pair[0]);
                cur_level = int.Parse(area_detail_pair[1]);
                return true;
            }

            active_level = 0;
            cur_level = 0;

            return false;
        }

        public void SavePlayerCurItems(int cur_area)
        {
            //  保存得当前道具数，这个道具只是缓存用的（当然没想好就是玩家如果主动选择关卡，如何去做）
            //  所以也可以先保存起来这些数据
            if (Constant.save_items)
            {
                string sz_area = string.Format("area_{0}", cur_area);
                string sz_items = string.Format("{0}_items", sz_area);
                //
                var itemlist = BattleItemFactory.getInstance.GetItemList();
                string items = "";
                foreach (var item in itemlist)
                {
                    var item_detail = string.Format("{0},{1}", item.Key, item.Value);
                    items = string.Format("{0}|{1}",items, item_detail);
                }
                PlayerPrefs.SetString(sz_items, items);
            }
        }

        readonly char[] spt = new char[] { '|' };

        public void LoadPlayerCurItems(int cur_area)
        {
            if (Constant.save_items)
            {
                string sz_area = string.Format("area_{0}", cur_area);
                string sz_itemsTypes = string.Format("{0}_item_types", sz_area);
                string sz_items = string.Format("{0}_items", sz_area);
                //
                string items = PlayerPrefs.GetString(sz_items);
                if (items == "") return;
                string[] item_detail_list = items.Split(spt, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item_detail in item_detail_list)
                {
                    var pair = item_detail.Split(',');
                    var type_name = pair[0];
                    var n = int.Parse(pair[1]);

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

        public void ClearGameProgress()
        {
            PlayerPrefs.DeleteAll();
        }

        public void FirstWritePlayerPrefs()
        {
            string sz_cur_area_detail = string.Format("area_{0}_detail", 1);
            var data = PlayerPrefs.GetString(sz_cur_area_detail);
            if (data == "")
            {
                PlayerPrefs.SetString(sz_cur_area_detail, "1,1");
            }
        }
    }
}
