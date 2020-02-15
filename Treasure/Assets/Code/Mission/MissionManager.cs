using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bunker.Module;


namespace Bunker.Game
{
    public delegate void MissionChangeDelegate(string name, int n,int max);
    public class MissionManager : ServicesModule<MissionManager>
    {
        public const int Mission_Success = 1;
        public const int Mission_Failure = -1;
        public const int Mission_Processing = 0;
        public const int Mission_Unload = -2;

        MissionData missionData = new MissionData();
        MissionData curMissionData = new MissionData();

        public MissionData OriginalMission { get { return missionData; } }
        public MissionData CurrentMission { get { return curMissionData; } }

        public Dictionary<MissionCollectionType,MissionChangeDelegate> MissionChangeDelegateDict = 
            new Dictionary<MissionCollectionType, MissionChangeDelegate>();


        public int curMissionState = Mission_Unload;

        public void LoadMissionData(MissionData md)
        {
            MissionChangeDelegateDict.Clear();
            missionData.Reset(); 
            curMissionData.Reset();
            //复制两份，一份用来保存原始标准，一份用来记录当前情况
            missionData.MaxSteps = md.MaxSteps;
            foreach (var mp in md.Missions)
            {
                missionData.Missions.Add(mp.Clone());
            }
            foreach (var mp in md.ProtectMissions)
            {
                missionData.ProtectMissions.Add(mp.Clone());
            }
            //
            curMissionData.MaxSteps = md.MaxSteps;
            foreach (var mp in md.Missions)
            {
                var tmp_mp = mp.Clone();
                //当前任务的完成度应该是0，而保护任务的完成度应该是满的
                tmp_mp.Value = 0;
                curMissionData.Missions.Add(tmp_mp);
            }
            foreach (var mp in md.ProtectMissions)
            {
                curMissionData.ProtectMissions.Add(mp.Clone());
            }
            //
            curMissionState = Mission_Processing;
        }

        public bool RegisterMissionChangeDelegate(MissionCollectionType mct,MissionChangeDelegate cb)
        {
            if (!MissionChangeDelegateDict.ContainsKey(mct))
            {
                MissionChangeDelegateDict.Add(mct, cb);
                return true;
            }
            return false;
        }

        public void InvokeMissionDelegate(MissionCollectionType mct)
        {
            if (MissionChangeDelegateDict.ContainsKey(mct))
            {
                var idx = curMissionData.Missions.FindIndex(s => { return s.Key == mct; });
                if (idx != -1)
                {
                    MissionChangeDelegateDict[mct]?.Invoke(
                    MissionDataHelper.MCT_2_Names(mct),
                    curMissionData.Missions[idx].Value,
                    OriginalMission.Missions[idx].Value);
                }
                else
                {
                    idx = curMissionData.ProtectMissions.FindIndex(s => { return s.Key == mct; });
                    if (idx != -1)
                    {
                        MissionChangeDelegateDict[mct]?.Invoke(
                            MissionDataHelper.MCT_2_Names(mct),
                            curMissionData.ProtectMissions[idx].Value,
                            OriginalMission.ProtectMissions[idx].Value);
                    }
                }  
            }
        }

        public void Collect(MissionCollectionType mct)
        {
            var idx = curMissionData.Missions.FindIndex(s=> { return s.Key == mct; });
            if (idx != -1)
            {
                curMissionData.Missions[idx].Value += 1;
                if (MissionChangeDelegateDict.ContainsKey(mct))
                {
                    MissionChangeDelegateDict[mct]?.Invoke(
                        MissionDataHelper.MCT_2_Names(mct),
                        curMissionData.Missions[idx].Value,
                        OriginalMission.Missions[idx].Value);
                }
            }
            else
            {
                idx = curMissionData.ProtectMissions.FindIndex(s => { return s.Key == mct; });
                if (idx != -1)
                {
                    curMissionData.ProtectMissions[idx].Value -= 1;

                    if (MissionChangeDelegateDict.ContainsKey(mct))
                    {
                        MissionChangeDelegateDict[mct]?.Invoke(
                            MissionDataHelper.MCT_2_Names(mct),
                            curMissionData.ProtectMissions[idx].Value,
                            OriginalMission.ProtectMissions[idx].Value);
                    }
                }
            }
            
        }

        public void ConsumeStep()
        {
            curMissionData.MaxSteps -= 1;
        }

        public void RegainStep()
        {
            curMissionData.MaxSteps += 1;
        }

        public int GetMissionsState()
        {
            var ret = Mission_Processing;

            foreach (var md in curMissionData.ProtectMissions)
            {
                if(md.Value == 0)
                {
                    return Mission_Failure;
                }
            }

            for (var i = 0; i < curMissionData.Missions.Count; ++i)
            {
                var mp = curMissionData.Missions[i];
                if (mp.Value == OriginalMission.Missions[i].Value)
                {
                    ret = Mission_Success;
                    break;
                }
            }

            if (curMissionData.MaxSteps == 0)
            {
                return Mission_Failure;
            }

            return ret;
        }
    }
}
