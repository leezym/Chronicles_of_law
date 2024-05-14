using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using VISUALNOVEL;

public class TestDialogueFiles : MonoBehaviour
{
    public static TestDialogueFiles Instance { get; private set; }
    public TextAsset fileToRead = null;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            DestroyImmediate(gameObject);
    }

    void Start()
    {
        StartConversation();
    }

    void StartConversation()
    {
        string fullPath = AssetDatabase.GetAssetPath(fileToRead);

        int resourcesIndex = fullPath.IndexOf("Resources/");
        string relativePath = fullPath.Substring(resourcesIndex + 10); // "Resources/" tiene tamaño 10
        Debug.Log(relativePath);

        string filePath = Path.ChangeExtension(relativePath, null);

        VNManager.Instance.LoadFile(filePath);
    }
}
