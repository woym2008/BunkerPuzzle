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
        "Empty"
    };
    public static int[] TilesIndex =
    {
        0,
        1
    };

    public static Vector2 TileSize =
        //new Vector2(0.16f,0.16f);
        new Vector2(1f, 1f);

    public static int debug_start_level = 1;
    //
    public static bool save_items = true;
    //
    public const string DOMAIN_PREFIX = "Bunker.Game.";

    public static int MaxRow = 10;
    public static int MaxCol = 10;
}
