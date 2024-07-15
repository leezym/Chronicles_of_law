using System;
using System.Collections;
using System.Collections.Generic;
using HISTORY;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        private const string CLIC_SOUND_PATH = "Audio/SFX/interface-click";
        public GameObject MENU;
        public GameObject LAYERS;

        public Button LOAD;
        public TextAsset fileToRead = null;

        void Update()
        {
            //Clic sound
            if (Keyboard.current.anyKey.wasPressedThisFrame ||
                    Mouse.current.leftButton.wasPressedThisFrame || 
                    Mouse.current.rightButton.wasPressedThisFrame || 
                    Mouse.current.middleButton.wasPressedThisFrame)
                AudioManager.Instance.PlaySoundEffect(CLIC_SOUND_PATH);
            
            // Next
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                PromptAdvance();
        }

        public void PromptAdvance()
        {
            if(!FolderPanel.Instance.isWaitingOnUserChoice)
                DialogueSystem.Instance.OnUserPrompt_Next();
        }
    }
}

