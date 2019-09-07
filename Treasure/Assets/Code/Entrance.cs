using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        ModuleManager.getInstance.CreateModule<BattlefieldModule>();
    }
}
