using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapData))]
public class MapDataEditor : Editor
{
    MapData mapdata;
    GUIStyle gs;
    //bool addmaptile;
    bool showFoldout = true;

    Rect PreviewMapRect = new Rect();

    static Color[] tileColorDye = {
        Color.black,
        Color.blue,
        Color.yellow,
        Color.yellow,
        Color.yellow,
        Color.yellow,
        Color.green,
        Color.gray,
        Color.gray,
        Color.magenta,
        Color.red,
        Color.clear,
        Color.red,  //新添加
        Color.white,
    };

    private void OnEnable()
    {
        mapdata = (MapData)target;

        //tile = Resources.Load<MapTile>("tile/" + mapdata.tilename);

        //addmaptile = false;

        Debug.Log("enter MapDataEditor onenable" + mapdata.row);
    }

    List<int> _tempList = new List<int>();
    int currentSelectID = 0;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        bool ischange = false;
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
                mapdata.additionaldata = new string[row * column];
                mapdata.row = row;
                mapdata.column = column;

                ischange = true;
                currentSelectID = 0;
            }
            //mapdata.row = row;
            //mapdata.column = column;


            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            showFoldout = EditorGUILayout.Foldout(showFoldout, "地图块阵列");
            if (showFoldout)
            {

                _tempList.Clear();
                for (int i = 0; i < Constant.Tiles.Length; ++i)
                {
                    _tempList.Add(i);
                }
                var tilescountarray = _tempList.ToArray();

                for (int i = 0; i < mapdata.row; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int j = 0; j < mapdata.column; ++j)
                    {
                        //mapdata.data[i * column + j] = EditorGUILayout.IntField(mapdata.data[i * column + j]);

                        var index = mapdata.data[i * column + j];

                        var selectdata = EditorGUILayout.IntPopup(index, Constant.Tiles, tilescountarray);
                        var oridata = mapdata.data[i * column + j];
                        if (selectdata != oridata)
                        {
                            mapdata.data[i * column + j] = selectdata;
                            ischange = true;

                            //currentSelectID = i * column + j;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(40);
                EditorGUILayout.LabelField("当前的块:");
                EditorGUILayout.BeginHorizontal();
                //当前选择的
                EditorGUILayout.LabelField("类型:", GUILayout.Width(64));
                var currentoridata = mapdata.data[currentSelectID];
                var newselectdata = EditorGUILayout.IntPopup(currentoridata, Constant.Tiles, tilescountarray, GUILayout.Width(128));
                //var currentoridata = mapdata.data[currentSelectIndex];
                GUILayout.Space(10);
                EditorGUILayout.LabelField("参数:", GUILayout.Width(64));
                var curtilestr = Constant.Tiles[newselectdata];

                if(mapdata.additionaldata.Length != mapdata.data.Length)
                {
                    mapdata.additionaldata = new string[mapdata.row * mapdata.column];
                }
                var curadddata = mapdata.additionaldata[currentSelectID];
                var newaddselectdata = curadddata;
                switch (curtilestr)
                {
                    case "BombTile":
                        {
                            bool istimebomb = false;
                            int timebombvalue = 0;
                            if (newaddselectdata != "" && newselectdata == currentoridata)
                            {
                                var olddatas = newaddselectdata.Split(';');
                                istimebomb = bool.Parse(olddatas[0]);
                                timebombvalue = int.Parse(olddatas[1]);                                
                            }
                            EditorGUILayout.LabelField("是否为定时", GUILayout.Width(72));
                            istimebomb = EditorGUILayout.Toggle(istimebomb, GUILayout.Width(32));
                            if (istimebomb)
                            {
                                EditorGUILayout.LabelField("回合数", GUILayout.Width(48));
                                timebombvalue = EditorGUILayout.IntField(timebombvalue, GUILayout.Width(32));
                            }
                            string datastr = string.Format("{0};{1}", istimebomb.ToString(), timebombvalue.ToString());
                            newaddselectdata = datastr;
                            break;
                        }
                    default:
                        {
                            newaddselectdata = EditorGUILayout.TextField(newaddselectdata, GUILayout.Width(64));
                        }
                        break;
                }
                

                if (newselectdata != currentoridata)
                {
                    mapdata.data[currentSelectID] = newselectdata;
                    ischange = true;
                }
                if (newaddselectdata != curadddata)
                {
                    mapdata.additionaldata[currentSelectID] = newaddselectdata;
                    ischange = true;
                }


                EditorGUILayout.EndHorizontal();
                //end
                GUILayout.Space(20);
            }

            
        }

        EditorGUILayout.EndVertical(); 

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("地图预览:");


        Rect r = (Rect)EditorGUILayout.BeginVertical();

        GUILayout.Box("", GUILayout.Width(200), GUILayout.Height(200));

        PreviewMapRect.Set(r.x, r.y, 20, 20);
        //EditorGUI.DrawRect(PreviewMapRect, Color.green);

        Vector2 mouseClickPos = Vector2.zero;
        bool clickMouse = false;
        if (Event.current.type == EventType.MouseDown)
        {
            mouseClickPos = Event.current.mousePosition;

            clickMouse = true;
        }

        for (int i = 0; i < mapdata.row; ++i)
        {
            for (int j = 0; j < mapdata.column; ++j)
            {
                var index = mapdata.data[i * mapdata.column + j];

                PreviewMapRect.x = r.x + j * 20;
                PreviewMapRect.y = r.y + i * 20;

                if(clickMouse && PreviewMapRect.Contains(mouseClickPos))
                {
                    if (Event.current.button == 0) index++; else index--;   //鼠标右键反向
                    if (Event.current.button == 2) index = 0;   //鼠标中键使其归0

                    if (index >= tileColorDye.Length)
                    {
                        index = 0;
                    }
                    if(index < 0)
                    {
                        index = tileColorDye.Length - 1;
                    }

                    ischange = true;
                    mapdata.data[i * mapdata.column + j] = index;
                    currentSelectID = i * mapdata.column + j;
                }

                var iconIndex = Constant.Tiles[index];
                if(MapTileIconManager.getInstance.textures.ContainsKey(iconIndex))
                {
                    var tex = MapTileIconManager.getInstance.textures[iconIndex];
                    if(tex != null)
                        EditorGUI.DrawTextureTransparent(PreviewMapRect, tex);

                }
                //var tex = MapTileIconManager.getInstance.textures[iconIndex];
                //EditorGUI.DrawRect(PreviewMapRect, tileColorDye[index]);

                //Debug.Log(PreviewMapRect);


            }
        }
        GUILayout.Space(20);
        
        EditorGUILayout.BeginHorizontal();
        //gs.stretchWidth = false;
        if (GUILayout.Button("重置", GUILayout.Width(150)))
        {
            if(EditorUtility.DisplayDialog("地图编辑", "确定要重置编辑的关卡么？", "确认", "取消"))
            {
                for (int i = 0; i < mapdata.row; ++i)
                {
                    for (int j = 0; j < mapdata.column; ++j)
                    {
                        mapdata.data[i * mapdata.column + j] = 0;
                    }
                }
            }

            ischange = true;
        }
        //Debug.Log(r);
        GUILayout.Space(20);

        if (GUILayout.Button("保存", GUILayout.Width(150)))
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();






        if (ischange)
        {
            EditorUtility.SetDirty(this.target);
        }

        serializedObject.ApplyModifiedProperties();

    }
}
