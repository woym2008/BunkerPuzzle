using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;

public class LevelManager : ServicesModule<LevelManager>
{
    private CSVTable _levelTable;

    //public List<string> _levels;
    public Dictionary<int, List<string>> _levels;

    public int LastLevel
    {
        get;
        set;
    }
    public int LastArea
    {
        get;
        set;
    }

    public int CurArea;
    public int CurLevel;
    //
    public int MaxArea { get; set; }
    //List
    public void LoadLevelFiles()
    {
        //Resources.LoadAll()
        _levelTable = TableModule.getInstance.GetTable("LevelTable");
        if(_levels == null)
        {
            _levels = new Dictionary<int, List<string>>();
        }
        _levels.Clear();

        foreach (var l in _levelTable._rows)
        {
            if( l._cols == null || 
                l._cols.Count < 2 || 
                !l._cols.TryGetValue("level", out Col val))
            {
                break;
            }

            var levelStr = l._cols["level"]._cellText;
            var areaStr = int.Parse(l._cols["area"]._cellText);
            Debug.Log("levelStr: " + levelStr);
            Debug.Log("areaStr: " + areaStr);
            var lvs = levelStr.Split(';');
            if (!_levels.ContainsKey(areaStr))
            {
                _levels[areaStr] = new List<string>();
            }
            var ls = _levels[areaStr];
            foreach(var lv in lvs)
            {
                ls.Add(lv);
            }
            //
            MaxArea = areaStr;
        }

        LastLevel = 1;
        LastArea = 1;

        CurArea = LastArea;
        CurLevel = LastLevel;
    }

    public int GetBossLevel(int areaID)
    {
        foreach (var l in _levelTable._rows)
        {
            if (l._cols == null ||
                l._cols.Count < 2 ||
                !l._cols.TryGetValue("boss", out Col val))
            {
                break;
            }
            var bossStr = l._cols["boss"]._cellText;
            var areaStr = int.Parse(l._cols["area"]._cellText);
            if(areaID == areaStr)
            {
                return int.Parse(bossStr);
            }
        }
        return 0;
    }

    public string[] GetAreaLevels(int areaID)
    {
        if(!_levels.ContainsKey(areaID))
        {
            return null;
        }
        return _levels[areaID].ToArray();
    }

    public string GetNextLevel(int areaID, string currentlevel)
    {
        var curareaLevels = GetAreaLevels(areaID);
        if(curareaLevels == null || curareaLevels.Length == 0)
        {
            Debug.LogWarning("Error Area, Not Find Level,MayBe Game is Totally complate!");
            return "";
        }
        if (currentlevel == "")
        {
            return curareaLevels[0];
        }

        for (int i=0; i< curareaLevels.Length;++i)
        {
            if (curareaLevels[i] == currentlevel)
            {
                if(i== curareaLevels.Length-1)
                {
                    CurArea++;
                    if(CurArea > LastArea)
                    {
                        LastArea = CurArea;
                    }
                    var nextarealevel = GetNextLevel(CurArea, "");
                    if(nextarealevel == "")
                    {
                        return "";
                    }
                    CurLevel = int.Parse(nextarealevel);
                    return nextarealevel;
                }
                CurLevel++;
                return curareaLevels[CurLevel - 1];
            }
        }

        return "";
    }

    public void SetCurrentLevel(int areaID, int levelID)
    {
        CurArea = areaID;
        CurLevel = levelID;      
    }

    public void Save()
    {

    }

    public void Load()
    {

    }
}
