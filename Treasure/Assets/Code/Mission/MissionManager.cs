using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bunker.Module;


namespace Bunker.Game
{
    public delegate void MissionChangeDelegate(string name, int n,int max);
    public delegate void StepChangeDelegate(float n);

    public class MissionManager : ServicesModule<MissionManager>
    {
        public const int Mission_Success = 1;
        public const int Mission_Failure = -1;
        public const int Mission_Processing = 0;
        public const int Mission_Unload = -2;

        MissionData missionData = new MissionData();
        MissionData curMissionData = new MissionData();
        StepChangeDelegate stepChangeCallback;

        public MissionData OriginalMission { get { return missionData; } }
        public MissionData CurrentMission { get { return curMissionData; } }

        public Dictionary<MissionCollectionType,MissionChangeDelegate> MissionChangeDelegateDict = 
            new Dictionary<MissionCollectionType, MissionChangeDelegate>();

        public Dictionary<MissionCollectionType, RectTransform> MissionItemPosDict =
            new Dictionary<MissionCollectionType, RectTransform>();


        public int curMissionState = Mission_Unload;

        public void LoadMissionData(MissionData md)
        {
            MissionChangeDelegateDict.Clear();
            MissionItemPosDict.Clear();
            missionData.Reset(); 
            curMissionData.Reset();
            stepChangeCallback = null;
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
        /*
            item是missionItem 需要保存其位置
             */
        public bool RegisterMissionChangeDelegate(MissionCollectionType mct,MissionChangeDelegate cb)
        {
            if (!MissionChangeDelegateDict.ContainsKey(mct))
            {
                MissionChangeDelegateDict.Add(mct, cb);
                return true;
            }
            return false;
        }

        public bool RegisterMissionItemPos(MissionCollectionType mct, RectTransform item)
        {
            if (!MissionItemPosDict.ContainsKey(mct))
            { 
                MissionItemPosDict.Add(mct, item);
                return true;
            }
            return false;
        }

        public bool GetMissionItemPos(MissionCollectionType mct,ref Vector3 pos)
        {
            if (MissionItemPosDict.ContainsKey(mct))
            {
                pos = Camera.main.ScreenToWorldPoint(MissionItemPosDict[mct].position);
                pos.z = 0;
                return true;
            }
            return false;
        }

        public void RegisterStepChanageDelegate(StepChangeDelegate cb)
        {
            stepChangeCallback = cb;
            stepChangeCallback.Invoke(curMissionData.MaxSteps);
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

        public void ConsumeStep(int step = 1)
        {
            curMissionData.MaxSteps -= step;
            //stepChangeCallback?.Invoke(curMissionData.MaxSteps / (float)missionData.MaxSteps);
            stepChangeCallback.Invoke(curMissionData.MaxSteps); 
        }

        public void RegainStep(int step = 1)
        {
            curMissionData.MaxSteps += step;
            //stepChangeCallback?.Invoke(curMissionData.MaxSteps / (float)missionData.MaxSteps);
            stepChangeCallback.Invoke(curMissionData.MaxSteps);
        }

        public int GetMissionsState()
        {
            var ret = Mission_Success;

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
                if (mp.Value < OriginalMission.Missions[i].Value)
                {
                    ret = Mission_Processing;
                    break;
                }
            }

            if (ret == Mission_Processing && curMissionData.MaxSteps <= 0)
            {
                return Mission_Failure;
            }

            return ret;
        }
    }
}
