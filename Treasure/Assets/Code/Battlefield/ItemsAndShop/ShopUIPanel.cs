using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Bunker.Process;
using Bunker.Module;



namespace Bunker.Game
{

    public class ShopUIPanel : UIPanel
    {
        public List<Image> Items = new List<Image>();
        public List<Text> Labels = new List<Text>();
        public List<Image> Tags = new List<Image>();
        public List<Toggle> Toggles = new List<Toggle>();
        //
        public Image HeadIcon;
        //
        public Button NextBtn;
        ///--------------------------------------------
        ///
        const int ITEM_COUNT_MAX = 3;
        List<ShopItemDesc> ShopItems = new List<ShopItemDesc>(ITEM_COUNT_MAX);
        //

        public override void OnBegin()
        {
            base.OnBegin();
            //
            Items.Add(_transform.Find("FRAME/ITEMS/SLOT_1/ITEM").GetComponent<Image>());
            Labels.Add(_transform.Find("FRAME/ITEMS/SLOT_1/TEXT").GetComponent<Text>());
            Tags.Add(_transform.Find("FRAME/ITEMS/SLOT_1/SELL").GetComponent<Image>());
            //
            Items.Add(_transform.Find("FRAME/ITEMS/SLOT_2/ITEM").GetComponent<Image>());
            Labels.Add(_transform.Find("FRAME/ITEMS/SLOT_2/TEXT").GetComponent<Text>());
            Tags.Add(_transform.Find("FRAME/ITEMS/SLOT_2/SELL").GetComponent<Image>());
            //
            Items.Add(_transform.Find("FRAME/ITEMS/SLOT_3/ITEM").GetComponent<Image>());
            Labels.Add(_transform.Find("FRAME/ITEMS/SLOT_3/TEXT").GetComponent<Text>());
            Tags.Add(_transform.Find("FRAME/ITEMS/SLOT_3/SELL").GetComponent<Image>());
            //
            HeadIcon = _transform.Find("FRAME/HEAD").GetComponent<Image>();
            NextBtn = _transform.Find("FRAME/NEXT_BTN").GetComponent<Button>();
            NextBtn.onClick.AddListener(OnNext);
            //
            Toggles.Add(_transform.Find("FRAME/ITEMS/SLOT_1").GetComponent<Toggle>());
            Toggles.Add(_transform.Find("FRAME/ITEMS/SLOT_2").GetComponent<Toggle>());
            Toggles.Add(_transform.Find("FRAME/ITEMS/SLOT_3").GetComponent<Toggle>());


        }

        public override void OnClose()
        {
            base.OnClose();
            ShopItems.Clear();
        }

        //打开商店的时候就已经刷好了item，由module传入
        public override void OnOpen(params object[] datas)
        {
            base.OnOpen(datas);
            //刷出来数据
            HeadIcon.sprite = datas[0] as Sprite;
            ShopItemDesc[] items = datas[1] as ShopItemDesc[];
            for (int j = items.Length - 1; j>=0;j-- )
            {
                ShopItems.Add(items[j]);
            }
            //将其刷入到UI上
            int i = 0;
            foreach (var si in ShopItems)
            {
                Items[i].sprite = si.item_icon;
                Labels[i].text = string.Format("${0}",si.cost);
                Tags[i].enabled = false;
                ++i;
            }
        }

        public void OnNext()
        {
            int idx = -1;
            for(int i = 0;i< Toggles.Count; ++i)
            {
                if(Toggles[i].isOn)
                {
                    idx = i;break;
                }
            }
            //
            if(idx != -1)
            {
                string item_type = ShopItems[idx].item_type;
                //TODO
                BattleItemFactory.getInstance.CreateBattleItem(item_type, 1);
            }
            //关闭页面
            ModuleManager.getInstance.GetModule<ShopUIModule>().HideShopUIPanel();
            ModuleManager.getInstance.GetModule<BattleUIModule>().DisplayBattleUIPanel();

        }
    }
}
