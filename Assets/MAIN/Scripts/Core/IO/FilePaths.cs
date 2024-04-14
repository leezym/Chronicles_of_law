using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePaths
{
    private const string HOME_DIRECTORY_SYMBOL = "~/";

    public static readonly string root = $"{Application.dataPath}/gameData/";

    //Resources Font Paths
    public static readonly string resources_font = "Font/";

    //Resources Graphics Paths
    public static readonly string resources_graphics = "Graphics/";
    public static readonly string resources_backgroundImages = $"{resources_graphics}BG Images/";
    public static readonly string resources_backgroundVideos = $"{resources_graphics}BG Videos/";

    //Resources Audio Paths
    public static readonly string resources_audio = "Audio/";
    public static readonly string resources_music = $"{resources_audio}Music/";

    //Resources Dialogue Paths
    public static readonly string resources_dialogueFiles = $"Dialogue Files/";

    //Resources Items Paths
    public static readonly string resources_itemsFiles = $"Items/";

    ///<summary>
    ///Returns the path to the resource using the default path or the root of the resources folder if
    ///</summary/>
    ///<param name="defaultPath"></param>
    ///<param name="resourceName"></param>
    ///<returns></returns>
    public static string GetPathToResource(string defaultPath, string graphicName)
        {
            if(graphicName.StartsWith(HOME_DIRECTORY_SYMBOL))
                return graphicName.Substring(HOME_DIRECTORY_SYMBOL.Length);

            return defaultPath + graphicName;
        }
}
