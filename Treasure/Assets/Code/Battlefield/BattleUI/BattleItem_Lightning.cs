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
        Vector3 offset;
        RectTransform rt;

        Transform saved_parent;
        Vector3 saved_pos;

        float minWidth;             //水平最小拖拽范围
        float maxWidth;            //水平最大拖拽范围
        float minHeight;            //垂直最小拖拽范围  
        float maxHeight;            //垂直最大拖拽范围
        float rangeX;               //拖拽范围
        float rangeY;               //拖拽范围

        void Update()
        {
            DragRangeLimit();
            /*
            if (Input.GetMouseButtonDown(0))
            {
                var end_pt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var _bf = ModuleManager.getInstance.GetModule<BattlefieldModule>();
                var _tile = _bf.Field.GetGrid(end_pt);
                if (_tile != null)
                {
                    Debug.Log(_tile);
                }
            }*/
        }

        void Start()
        {
            rt = GetComponent<RectTransform>();
            saved_pos = rt.position;
            saved_parent = rt.parent;

            minWidth = rt.rect.width / 2;
            maxWidth = Screen.width - (rt.rect.width / 2);
            minHeight = rt.rect.height / 2;
            maxHeight = Screen.height - (rt.rect.height / 2);
        }
        void DragRangeLimit()
        {
            //限制水平/垂直拖拽范围在最小/最大值内
            rangeX = Mathf.Clamp(rt.position.x, minWidth, maxWidth);
            rangeY = Mathf.Clamp(rt.position.y, minHeight, maxHeight);
            //更新位置
            rt.position = new Vector3(rangeX, rangeY, 0);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector3 globalMousePos;
            rt.SetParent(GameObject.Find("BattleUIPanel").transform);
            //将屏幕坐标转换成世界坐标
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out globalMousePos))
            {
                //计算UI和指针之间的位置偏移量
                offset = rt.position - globalMousePos;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {  
            var end_pt = Camera.main.ScreenToWorldPoint(eventData.position);
            var _bf = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var _tile = _bf.Field.GetGrid(end_pt);
            if (_tile!= null 
                && _tile.CanElimination()
                && _bf.UseController<GridFieldController_DestroyTile>(new IGridObject[1] { _tile }))
            {                                     
                 Remove();                
            }
            else
            {
                rt.position = saved_pos;
                rt.SetParent(saved_parent);
            }
        }

        private void SetDraggedPosition(PointerEventData eventData)
        {
            Vector3 globalMousePos;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out globalMousePos))
            {
                rt.position = offset + globalMousePos;
            }
            //
            var end_pt = Camera.main.ScreenToWorldPoint(eventData.position);
            var _bf = ModuleManager.getInstance.GetModule<BattlefieldModule>();
            var _tile = _bf.Field.GetGrid(end_pt);
            if (_tile != null && _tile.CanElimination())
            {
                Debug.Log("item selected a tile:" + _tile.GetGridType());
            }
        }

    }

}