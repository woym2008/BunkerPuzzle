using UnityEngine;
using System.Collections;
using Bunker.Module;
using System;
using Bunker.Process;
using DG.Tweening;

namespace Bunker.Game
{
    /*
     * 
     *  这个是Monobehavior派生的，是unity组件反馈给代码中的桥梁
     * 
     * 
     * */
    public class StarringRole : MonoBehaviour
    {
        public void SetSpriteSortingOrder(int order)
        {
            var sr = GetComponentInChildren<SpriteRenderer>();
            sr.sortingOrder = order;
        }
    }
}
