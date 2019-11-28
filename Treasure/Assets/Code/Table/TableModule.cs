using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;

public class TableModule : ServicesModule<TableModule>
{
    const string TablesPath = "Tables";

    Dictionary<string, CSVTable> _tables;

    public override void Release()
    {
        base.Release();
    }

    public void Start()
    {
        var tables = Resources.LoadAll(TablesPath);
        foreach(var table in tables)
        {
            TextAsset text = table as TextAsset;

            var newtable = new CSVTable();
            var lineArray = text.text.Split("\r"[0]);
            //title
            newtable.AddTitle(lineArray[0]);
            //datas
            for (int i = 1; i < lineArray.Length; ++i)
            {
                newtable.AddRow(lineArray[i]);
            }

            _tables.Add(text.name, newtable);
        }

    }

    public CSVTable GetTable(string name)
    {
        if(!_tables.ContainsKey(name))
        {
            Debug.LogError("Null Table");
            return null;
        }
        return _tables[name];
    }
}
