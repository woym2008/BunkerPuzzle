using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bunker.Module;
using System;

public class TableModule : ServicesModule<TableModule>
{
    const string TablesPath = "Table";

    Dictionary<string, CSVTable> _tables = new Dictionary<string, CSVTable>();

    public override void Release()
    {
        base.Release();
    }

    public void Start()
    {
        var tables = Resources.LoadAll(TablesPath);
        Debug.Log(tables);
        foreach(var table in tables)
        {
            Debug.Log(table);
            TextAsset text = table as TextAsset;

            var newtable = new CSVTable();
            var lineArray = text.text.Split(new string[]{ "\r\n" }, StringSplitOptions.None);
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
