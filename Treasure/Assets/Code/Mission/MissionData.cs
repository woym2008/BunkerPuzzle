using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionCollectionType
{
    Liquid,
    Electric,
    Wedge,
    Ghost,
}


[System.Serializable]
public class MissionPair
{
    public MissionPair()
    {
    }

    public MissionPair(MissionCollectionType mct, int n)
    {
        Key = mct;
        Value = n;
    }

    public MissionCollectionType Key;
    public int Value; 

    public MissionPair Clone()
    {
        return new MissionPair(Key, Value);
    }

}

[System.Serializable]
public class MissionData
{
    public List<MissionPair> Missions;          //需要完成的任务
    public List<MissionPair> ProtectMissions;   //保护某些cube不被摧毁  
    public int MaxSteps = 10;                        //最多移动步数
    //
    public void Reset()
    {
        Missions = new List<MissionPair>();
        ProtectMissions = new List<MissionPair>();
        MaxSteps = 10;
    }

}

public static class MissionDataHelper
{
    //Type到names的转换器
    static readonly string[] MissionCollectionTypeNames =
        new string[] { "Liquid", "Electric", "Wedge", "Ghost" };
    public static string MCT_2_Names(MissionCollectionType mct)
    {
        return MissionCollectionTypeNames[(int)mct];
    }
    //
    static readonly string[] MissionCollectionTypeSpriteNames =
        new string[] { "IconSet_14", "IconSet_13", "IconSet_11", "IconSet_1" };
    public static string MCT_2_SpriteNames(MissionCollectionType mct)
    {
        return MissionCollectionTypeSpriteNames[(int)mct];
    }
}