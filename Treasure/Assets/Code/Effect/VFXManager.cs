using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;
using DG.Tweening;

namespace Bunker.Game
{
    public class VFXManager : ServicesModule<VFXManager>
    {
        const int MAX_VFX = 10;
        const float BLUE_VFX_SPEED = 5;
        Dictionary<string,GameObject> PrefabsList;  

        public void Init()
        {
            PrefabsList = new Dictionary<string, GameObject>();
            var objs = Resources.LoadAll("Prefabs/VFX/");
            foreach (var obj in objs)
            {
                var go = obj as GameObject;
                go.SetActive(false);
                PrefabsList.Add(go.name, go);
            }
        }

        public override void Release()
        {
            base.Release();
            PrefabsList.Clear();
        }

        public void VFX_SPR(string id, Vector3 from, Vector3 to, TweenCallback cb = null)
        {
            var vfx = GameObject.Instantiate<GameObject>(PrefabsList[id]);
            vfx.SetActive(true);
            vfx.transform.position = from;
            var t = Vector3.Distance(from, to) / BLUE_VFX_SPEED;
            if (cb != null)
            {
                vfx.transform.DOMove(to, t).OnComplete(cb);
            }
            else
            {
                vfx.transform.DOMove(to, t).OnComplete(() =>
                {
                    GameObject.Destroy(vfx.gameObject);
                });
            }
        }

        public void VFX_BlueSPR(Vector3 from,Vector3 to, TweenCallback cb = null)
        {
            VFX_SPR("Blue_SPR",from,to,cb);
        }
        public void VFX_RedSPR(Vector3 from, Vector3 to, TweenCallback cb = null)
        {
            VFX_SPR("Red_SPR", from, to, cb);
        }
    }
}
