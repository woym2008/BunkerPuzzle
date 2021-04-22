using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//地图文件
[CreateAssetMenu()]
public class MapData : ScriptableObject
{
    //地图文件中保存的是数值而不是二维数组
    [SerializeField]
    public int[] data;
    [SerializeField]
    public int row;
    [SerializeField]
    public int column;

    //存一个使用的tile配置表的名字
    //在编辑器和游戏加载地图文件的时候都先加载tile配置表
    [SerializeField]
    public string tilename;
    //这里保存着编辑的missions
    [SerializeField]
    public MissionData mission;
    [SerializeField]
    public string boss_type;
    [SerializeField]
    public string[] enemy_types;
    [SerializeField]
    public string[] friendly_types;

    //提供一个访问接口，可以用二维下标来方便的获取数据
    public int this[int r,int c]
    {
        get
        {
            int index = r*column + c;
            if (index > row*column || index < 0)
            {
                Debug.LogError("over the array size");
            }

            return data[index];
        }
    }

    private void OnEnable()
    {
        Debug.Log("enter MapData");
        Debug.Log("mapdata row: " + row);
        Debug.Log("mapdata column" + column);
    }
}
