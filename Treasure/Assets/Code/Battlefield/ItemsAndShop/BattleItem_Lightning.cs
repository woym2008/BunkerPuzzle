#region 描述
#endregion
using System;
using System.Collections.Generic;
using Bunker.Process;
using Bunker.Module;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Bunker.Game
{
    public class BattleItem_Lightning : BattleItem, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public override void OnInit(){
            //base.OnInit();
            BattleItemFactory.getInstance.RegistItemType<BattleItem_Lightning>();

        }
        public override void OnClick(){
            //base.OnClick();       
        }
        public override void OnUse(){
            base.OnUse();
        }
        public override void Remove()
        {
            BattleItemFactory.getInstance.ConsumeItem<BattleItem_Lightning>();
            base.Remove();
        }
        //-------------------------------------------------------
        Vector3 offset = Vector3.zero;
        RectTransform rt;

        Transform saved_parent;
        Vector3 saved_pos;
        Camera uiCamera = null;


        void Update()
        {

        }

        void Start()
        {
            rt = GetComponent<RectTransform>();
            saved_pos = rt.position;
            saved_parent = rt.parent;
            if (GameObject.Find("UICamera") != null)
            {
                uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector3 globalMousePos;
            rt.SetParent(GameObject.Find("BattleUIPanel").transform);
            //将屏幕坐标转换成世界坐标
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, 
                eventData.position, uiCamera, out globalMousePos))
            {
                //计算UI和指针之间的位置偏移量
                rt.position = globalMousePos;
                //offset = rt.position - globalMousePos;
            }
            //
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {  

            var bfm = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var bcm = ModuleManager.getInstance.GetModule<BattleControllerModule>();
            var curpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit =
                Physics2D.Raycast(
                    curpos,
                    Vector2.zero,
                    100.0f,
                    LayerMask.GetMask("EliminationTile","Tile")
                    );

            if (hit.collider != null)
            {
                var tile = bfm.Field.FindTileObject(hit.collider.gameObject);
                if (tile != null
                && tile.CanElimination()
                && bcm.UseController<GridFieldController_EliminationTile>(new BaseTile[1] { tile }))
                {
                    Remove();
                    return;
                }
            }
            rt.position = saved_pos;
            rt.SetParent(saved_parent);
            GetComponent<Image>().overrideSprite = null;
        }

        private void SetDraggedPosition(PointerEventData eventData)
        {
            Vector3 globalMousePos;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, 
                eventData.position, uiCamera, out globalMousePos))
            {
                rt.position = offset + globalMousePos;
            }
        }

    }

}