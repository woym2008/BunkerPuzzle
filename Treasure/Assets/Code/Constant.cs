using UnityEngine;
using System.Collections;

public static class Constant
{
    public static string[] Tiles =
    {
        "NormalTile",
        "BlockTile",
        "LeftDirectTile",
        "UpDirectTile"
    };
    public static int[] TilesIndex =
    {
        0,
        1
    };

    public static Vector2 TileSize =
        new Vector2(0.2f,0.2f);
}
