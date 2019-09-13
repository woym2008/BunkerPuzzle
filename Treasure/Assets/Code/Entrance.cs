using System.Collections;
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
        


        ProcessManager.getInstance.Switch<TitleProcess>();
    }
}
