﻿using System.Collections;
using System.Collections.Generic;
using Bunker.Game;
using Bunker.Module;
using Bunker.Process;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        ModuleManager.getInstance.CreateModule<BattlefieldModule>();
        ModuleManager.getInstance.CreateModule<BattlefieldInputModule>();
        ModuleManager.getInstance.CreateModule<BattlefieldCameraModule>();
        ModuleManager.getInstance.CreateModule<TitleModule>();
        ModuleManager.getInstance.CreateModule<BattleUIModule>();
        ModuleManager.getInstance.CreateModule<ShopUIModule>(); //add by wwh
        ModuleManager.getInstance.CreateModule<GuideModule>();

        ModuleManager.getInstance.CreateModule<MaskTileModule>();
        //
        ModuleManager.getInstance.CreateModule<RobotManagerModule>();
        ModuleManager.getInstance.CreateModule<BattleTurnsModule>();

        ModuleManager.getInstance.CreateModule<BattleControllerModule>();
        ModuleManager.getInstance.CreateModule<StarredModule>();//add by wwh 2021-5-13


        TableModule.getInstance.Start();
        VFXManager.getInstance.Init();
        SoundManager.getInstance.Init();
        //
        //SaveLoader.getInstance.ClearGameProgress();
        SaveLoader.getInstance.FirstWritePlayerPrefs();
        //

        ProcessManager.getInstance.Switch<TitleProcess>();
    }
}
