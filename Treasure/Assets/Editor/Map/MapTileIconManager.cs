using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MapTileIconManager
{
    static MapTileIconManager Instance;
    public static MapTileIconManager getInstance {
        get {
            if(Instance == null)
            {
                Instance = new MapTileIconManager();
            }
            return Instance;
        }
    }
    public Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
    const string baseTexPath = "Assets/Art/tile/editorsprite/";
    MapTileIconManager()
    {
        LoadTextures();

    }

    void LoadTextures()
    {
        for(int i=0; i < Constant.Tiles.Length; ++i)
        {
            var name = Constant.Tiles[i];

            string texname = "";
            switch(name)
            {
                case "NormalTile":
                    {
                        texname = "normal_cube.png";
                    }
                    break;
                case "BlockTile":
                    {
                        texname = "liquid_cube.png";
                    }
                    break;
                case "LeftDirectTile":
                    {
                        texname = "left_cube.png";
                    }
                    break;
                case "UpDirectTile":
                    {
                        texname = "up_cube.png";
                    }
                    break;
                case "DownDirectTile":
                    {
                        texname = "down_cube.png";
                    }
                    break;
                case "RightDirectTile":
                    {
                        texname = "right_cube.png";
                    }
                    break;
                case "SignalLightTile":
                    {
                        texname = "signal_light.png";
                    }
                    break;
                case "WedgeLeftTile":
                    {
                        texname = "wedge_cube.png";
                    }
                    break;
                case "WedgeRightTile":
                    {
                        texname = "wedge_cube_r.png";
                    }
                    break;
                case "GemTile":
                    {
                        texname = "gem_cube.png";
                    }
                    break;
                case "RobotStartTile":
                    {
                        texname = "robot_start.png";
                    }
                    break;
                case "PorterStartTile":
                    {
                        texname = "porter_start.png";
                    }
                    break;
                case "DiskTile":
                    {
                        texname = "disk_cube.png";
                    }
                    break;
                case "BombTile":
                    {
                        texname = "bomb_cube.png";
                    }
                    break;
                case "ScreeTile":
                    {
                        texname = "bomb_cube.png";
                    }
                    break;
                case "LockTile":
                    {
                        texname = "bomb_cube.png";
                    }
                    break;
            }
            var texture = AssetDatabase.LoadAssetAtPath<Texture>(baseTexPath + texname);

            textures[name] = texture;
        }
    }
}