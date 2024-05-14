using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameSave : MonoBehaviour
{
    public VNGameSave save;
    void Start()
    {
        VNGameSave.activeFile = new VNGameSave();
    }

    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.S))
        {
            VNGameSave.activeFile.Save();
        }
        else if(Input.GetKeyDown(KeyCode.L))
        {            
            try
            {
                save = VNGameSave.Load($"{FilePaths.gameSaves}Caso1.Facil.DerechoFamilia{VNGameSave.FILE_TYPE}", activateOnLoad: true);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Do something because we found an error. {e.ToString()}");
            }
        }*/
    }

    public void Save()
    {
        VNGameSave.activeFile.Save();
    }

    public void Load()
    {
        try
        {
            save = VNGameSave.Load($"{FilePaths.gameSaves}Caso1.Facil.DerechoFamilia{VNGameSave.FILE_TYPE}", activateOnLoad: true);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Do something because we found an error. {e.ToString()}");
        }
    }
}
