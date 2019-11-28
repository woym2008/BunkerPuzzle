using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager
{
    private CSVTable _levelTable;

    //public List<string> _levels;
    public Dictionary<int, List<string>> _levels;
    //List
    void LoadLevelFiles()
    {
        //Resources.LoadAll()
        var levels = TableModule.getInstance.GetTable("LevelTable");
        if(_levels == null)
        {
            _levels = new Dictionary<int, List<string>>();
        }
        _levels.Clear();

        foreach (var l in levels._rows)
        {
            var levelStr = l._cols["level"]._cellText;
            var areaStr = int.Parse(l._cols["area"]._cellText);
            var ls = _levels[areaStr];
            if(ls == null)
            {
                ls = new List<string>();
            }
            ls.Add(levelStr);
        }
    }
}
