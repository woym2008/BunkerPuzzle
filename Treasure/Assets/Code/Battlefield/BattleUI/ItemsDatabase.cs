using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bunker.Game
{
    [System.Serializable]
    public class ShopItemDesc
    {
        public string item_type;
        public Sprite item_icon;
        public int cost;
        public int level; 
    }

    [CreateAssetMenu()]
    public class ItemsDatabase : ScriptableObject
    {
        [SerializeField]
        ShopItemDesc[] ShopItems;
        //
        public void GetItems(ref List<ShopItemDesc> items,int count)
        {
            items.Clear();
            for (; count > 0; count--)
            {
                items.Add(ShopItems[Random.Range(0, ShopItems.Length)]);
            }
        }
    }
}
