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
            var levelStr = l._cols["level"]._cellText;
            var areaStr = int.Parse(l._cols["area"]._cellText);
            Debug.Log("levelStr: " + levelStr);
            Debug.Log("areaStr: " + areaStr);
            if(!_levels.ContainsKey(areaStr))
            {
                _levels[areaStr] = new List<string>();
            }
            var ls = _levels[areaStr];
            ls.Add(levelStr);
        }

        LastLevel = 1;
        LastArea = 1;
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
        if(currentlevel == null || curareaLevels.Length == 0)
        {
            Debug.LogError("Error Area, Not Find Level");
            return "";
        }
        if (currentlevel == "")
        {
            return curareaLevels[0];
        }

        for (int i=0; i< curareaLevels.Length;++i)
        {
            if(curareaLevels[i] == currentlevel)
            {
                if(i== curareaLevels.Length-1)
                {
                    var nextarealevel = GetNextLevel(areaID + 1, "");
                    if(nextarealevel == "")
                    {
                        Debug.LogError("null level");
                    }
                    return nextarealevel;
                }
                return curareaLevels[i+1];
            }
        }

        return "";
    }

    public void Save()
    {

    }

    public void Load()
    {

    }
}
