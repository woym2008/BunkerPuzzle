using UnityEngine;
using System.Collections;

public static class Constant
{
    public static string[] Tiles =
    {
        "NormalTile",
        "BlockTile",
        "LeftDirectTile",
        "UpDirectTile",
        "DownDirectTile",
        "RightDirectTile",
        "SignalLightTile",
        "WedgeLeftTile",
        "WedgeRightTile",
        "GemTile",
        "RobotStartTile",

        "Empty",
        //新添加的
        "PorterStartTile",
        "DiskTile",
        "BombTile",
        "ScreeTile",
        "LockTile"


    };
    public static int[] TilesIndex =
    {
        0,
        1
    };

    public static Vector2 TileSize =
        //new Vector2(0.16f,0.16f);
        new Vector2(1f, 1f);

    public const int debug_start_level = 1;
    //
    public static bool save_items = true;
    //
    public const string DOMAIN_PREFIX = "Bunker.Game.";

    public static int MaxRow = 10;
    public static int MaxCol = 10;
    //
    public static string CAN_WALK_SUFFIX = "(walkable)";
    //
    public static class COLOR_NAME
    {
        public const string RED = "red";
        public const string YELLOW = "yellow";
        public const string BLUE = "blue";
        public const string GREEN = "green";
        public const string CYAN = "cyan";
        public const string ORANGE = "orange";
        public const string BLACK = "black";
    }
}
//add by wwh
public static class CDebug
{
    public static string GetMethod()
    {
        return new System.Diagnostics.StackFrame(1).GetMethod().ToString();
    }

    public static string GetFileName()
    {
        var fn =  new System.Diagnostics.StackFrame(1,true).GetFileName();
        return System.IO.Path.GetFileNameWithoutExtension(fn);
    }

    public static void Log(string str,string color = Constant.COLOR_NAME.BLACK)
    {
        Debug.Log(string.Format("<color={1}>{0}</color>",str, color));
    }
}