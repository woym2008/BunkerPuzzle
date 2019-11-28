using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public struct Row
{
    //public CSVTable _table;
    public string rowText;
    //public List<Col> _cols;
    public Dictionary<string, Col> _cols;

    public Row(string line, CSVTable table)
    {
        rowText = line;

        string[] cols = line.Split(',');
        _cols = new Dictionary<string, Col>();
        for(int i=0; i< cols.Length; ++i)
        {
            _cols.Add(table._titles[i], new Col(cols[i]));
        }
    }

    public string GetCell(string tile)
    {
        return _cols[tile].ToString();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach(var data in _cols)
        {
            sb.Append("-" + data.Value.ToString());
        }

        return sb.ToString().TrimStart('-');
    }

    public string this[string str]
    {
        get
        {
            return _cols[str].ToString();
        }
        set
        {
            this._cols[str].SetText(value);
        }
    }
}


public struct Col
{
    public string _cellText;

    public Col(string cell)
    {
        _cellText = cell;
    }

    public override string ToString()
    {
        return _cellText;
    }

    public void SetText(string text)
    {
        _cellText = text;
    }
}
public class CSVTable
{
    public List<Row> _rows;
    public List<string> _titles;

    public void AddTitle(string titleLine)
    {
        if(_titles == null)
        {
            _titles = new List<string>();
        }
        var titles = titleLine.Split(',');
        foreach(var t in titles)
        {
            _titles.Add(t);
        }

    }

    public void AddRow(string line)
    {
        if(_rows == null)
        {
            _rows = new List<Row> ();
        }
        var r = new Row(line,this);
        _rows.Add(r);
    }

    public Row this[int index]
    {
        get
        {
            return _rows[index];
        }
        set
        {
            this._rows[index] = value;
        }
    }
}
