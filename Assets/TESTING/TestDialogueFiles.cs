using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TestDialogueFiles : MonoBehaviour
{
    [SerializeField] private TextAsset fileToRead = null;

    void Start()
    {
        StartConversation();
    }

    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset(fileToRead);
        
        DialogueSystem.Instance.Say(lines);
    }

    /*void Update() {
        if(Input.GetKeyDown(KeyCode.DownArrow))
            DialogueSystem.Instance.dialogueContainer.Hide();
            
        if(Input.GetKeyDown(KeyCode.UpArrow))
            DialogueSystem.Instance.dialogueContainer.Show();

    }*/
}
