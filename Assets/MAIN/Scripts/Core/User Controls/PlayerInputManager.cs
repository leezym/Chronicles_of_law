using System;
using System.Collections;
using System.Collections.Generic;
using HISTORY;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        private const string CLIC_SOUND_PATH = "Audio/SFX/interface-click";

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
            
            /*if(Input.GetKeyDown(KeyCode.RightArrow))
                GoForward();

            if(Input.GetKeyDown(KeyCode.LeftArrow))
                GoBack();*/
        }

        public void PromptAdvance()
        {
            if(!FolderPanel.Instance.isWaitingOnUserChoice)
                DialogueSystem.Instance.OnUserPrompt_Next();
        }

        /*public void GoBack()
        {
            if(!DialogueSystem.Instance.conversationManager.isWaitingOnAutoTimer)
                HistoryManager.Instance.GoBack();
        }

        public void GoForward()
        {
            if(!DialogueSystem.Instance.conversationManager.isWaitingOnAutoTimer)
                HistoryManager.Instance.GoForward();
        }*/
    }
}

