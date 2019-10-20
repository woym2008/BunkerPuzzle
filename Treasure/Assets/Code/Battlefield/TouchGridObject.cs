using UnityEngine;
using System;
using System.Collections;
using Bunker.Module;

namespace Bunker.Game
{
    public class TouchGridObject : MonoBehaviour
    {
        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            var inputmodule = ModuleManager.getInstance.GetModule("BattlefieldInputModule") as BattlefieldInputModule;
            inputmodule.onPressClick += OnClick;
            inputmodule.onReleaseClick += OnRelease;
        }

        private void OnDisable()
        {
            var inputmodule = ModuleManager.getInstance.GetModule("BattlefieldInputModule") as BattlefieldInputModule;
            inputmodule.onPressClick -= OnClick;
            inputmodule.onReleaseClick -= OnRelease;
        }

        public void OnClick(Vector3 clickpos, InputState state, Action<object> onclick)
        {

        }

        public void OnRelease()
        {
            ;
        }
    }
}

