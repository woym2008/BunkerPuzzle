using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bunker.Game
{
    public delegate void MissionChangeDelegate(string name,int n);
    public class BattleMissionNode
    {
        public int Num;
        public string Name;
        public MissionChangeDelegate Change;
        //
        public void OnChange(int n){
            Num = n;
            Change?.Invoke(Name,Num);
        }        
    }
}