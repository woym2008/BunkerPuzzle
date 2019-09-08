using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapData))]
public class MapDataEditor : Editor
{
    MapData mapdata;

    //bool addmaptile;
    
    private void OnEnable()
    {
        mapdata = (MapData)target;

        //tile = Resources.Load<MapTile>("tile/" + mapdata.tilename);

        //addmaptile = false;

        Debug.Log("enter MapDataEditor onenable" + mapdata.row);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();

        //addmaptile = EditorGUILayout.Toggle("AddMapTile",addmaptile);
        //if(addmaptile)
        {
            int row = mapdata.row;
            int column = mapdata.column;

            row = EditorGUILayout.IntField("Row:", mapdata.row);
            column = EditorGUILayout.IntField("Column:", mapdata.column);
            if (row <= 0)
            {
                row = 1;
            }
            if (column <= 0)
            {
                column = 1;
            }

            if (mapdata.data == null || row != mapdata.row || column != mapdata.column)
            {
                mapdata.data = new int[row * column];
                mapdata.row = row;
                mapdata.column = column;
            }
            //mapdata.row = row;
            //mapdata.column = column;


            EditorGUILayout.Space();
            EditorGUILayout.Space();

            for (int i = 0; i < mapdata.row; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < mapdata.column; ++j)
                {
                    //mapdata.data[i * column + j] = EditorGUILayout.IntField(mapdata.data[i * column + j]);

                    var index = mapdata.data[i * column + j];

                    mapdata.data[i * column + j] = EditorGUILayout.IntPopup(index, Constant.Tiles, Constant.TilesIndex);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        //Debug.Log("enter MapDataEditor OnInspectorGUI");

        //mapdata.datastr = EditorGUILayout.TextArea(mapdata.datastr);


        for (int i=0;i< mapdata.row; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < mapdata.column; ++j)
            {
                //mapdata.data[i * column + j] = EditorGUILayout.IntField(mapdata.data[i* column + j]);
            }
            EditorGUILayout.EndHorizontal();
        }

        //if (addmaptile)
        {


        }

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();

    }
}
