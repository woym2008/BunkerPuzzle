using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Bunker.Module;


namespace Bunker.Game
{
    public class ShopUIModule : LogicModule
    {
        public ShopUIPanel shopUI;
        ItemsDatabase db;
        public bool displaying = false;

        public ShopUIModule() : base(typeof(ShopUIModule).ToString())
        {

        }
        public ShopUIModule(string name) : base(name)
        {

        }
        //
        public override void Create()
        {
            base.Create();

        }
        public override void OnStart(params object[] data)
        {
            base.OnStart(data);
            //这里其实根据某些规则，去加载不同的db的
            db = Resources.Load<ItemsDatabase>("UI/Items/ItemsDatabase");
        }
        public override void OnStop()
        {
            UIModule.getInstance.Close<ShopUIPanel>();
            base.OnStop();
        }

        List<ShopItemDesc> items = new List<ShopItemDesc>();

        public void DisplayShopUIPanel()
        {
            if(!displaying)
            {
                displaying = true;
                db.GetItems(ref items, 3);
                //此处需要根据某种规则随机处商店数据
                shopUI = UIModule.getInstance.Open<ShopUIPanel>(db.GetHeadIcon(), items.ToArray());
            }
        }
        public void HideShopUIPanel()
        {
            if (displaying)
            {
                displaying = false;
                UIModule.getInstance.Close<ShopUIPanel>();
                shopUI = null;
            }

        }
    }
}
