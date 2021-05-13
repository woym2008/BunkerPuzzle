using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using Bunker.Process;

namespace Bunker.Game
{
    //更像是一个组件管理器
    public class StarredTurn : CTurn
    {
        public StarredTurn(BattleTurnsModule btm) : base(btm)
        {
            Components = new List<StarredComponent>();
        }

        public GameObject starredRoleObj;
        public void CreateRole()
        {
            starredRoleObj = GameObject.Instantiate(Resources.Load("Prefabs/StarringRole")) as GameObject;
        }

        public void RemoveRole()
        {

        }
        #region 回合相关
        public override void OnStartTurn()
        {
            base.OnStartTurn();
            foreach (var c in Components)
            {
                c.OnStartTurn();
            }
        }

        public override void OnUpdateTurn()
        {
            base.OnUpdateTurn();
            if(Components.Count == 0)
            {
                TurnsModule_NextTurn();
            }
            else
            {
                foreach (var c in Components)
                {
                    c.OnUpdateTurn();
                }
            }

        }

        public override void OnEndTurn()
        {
            base.OnEndTurn();
            foreach (var c in Components)
            {
                c.OnEndTurn();
            }
        }

        public void TurnsModule_NextTurn()
        {
            _battleTurnsModule.NextTurn();
        }
        #endregion
        //
        #region COMPONENT
        public void AddComponent(StarredComponent c)
        {
            if (GetComponent(c.GetType().Name) == null)
            {
                Components.Add(c);
                c.OnAdd();
            }
            else
            {
                Debug.Log("添加主角组件失败：" + c.GetType().Name);
            }
        }
        public StarredComponent GetComponent(string comp_type)
        {
            foreach (var c in Components)
            {
                if(c.GetType().Name == comp_type)
                {
                    return c;
                }
            }
            return null;
        }

        public T GetComponent<T>() where T : StarredComponent
        {
            var c = GetComponent(typeof(T).Name);
            return c as T;
        }

        public void RemoveComponent(StarredComponent c)
        {
            if (Components.Remove(c))
            {
                c.OnRemove();
            }
        }
        public void ClearComponent()
        {
            Components.Clear();
        }
        List<StarredComponent> Components;
        #endregion

        #region 回调相关
        public void OnFinishMovement()
        {
            TurnsModule_NextTurn();
        }
        #endregion
    }

}
