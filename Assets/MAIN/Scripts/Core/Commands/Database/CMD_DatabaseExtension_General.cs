using System;
using System.Collections;
using System.Collections.Generic;
using COMMANDS;
using DIALOGUE;
using UnityEngine;
using UnityEngine.UI;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_General : CMD_DatabaseExtension
    {

        private static readonly string PARAM_FILEPATH = "archivo";
        private static readonly string PARAM_ENQUEUE = "cola";
        private static readonly string PARAM_ITEM = "-nombre";

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("mostrardialogo", new Func<IEnumerator>(ShowDialogueBox));
            database.AddCommand("ocultardialogo", new Func<IEnumerator>(HideDialogueBox));

            database.AddCommand("mostrarinterfaz", new Func<IEnumerator>(ShowDialogueSystem));
            database.AddCommand("ocultarinterfaz", new Func<IEnumerator>(HideDialogueSystem));
        }
        
        private static IEnumerator ShowDialogueBox()
        {
            yield return DialogueSystem.Instance.dialogueContainer.Show();
        }

        private static IEnumerator HideDialogueBox()
        {
            yield return DialogueSystem.Instance.dialogueContainer.Hide();
        }

        private static IEnumerator ShowDialogueSystem()
        {
            yield return DialogueSystem.Instance.Show();
        }

        private static IEnumerator HideDialogueSystem()
        {
            yield return DialogueSystem.Instance.Hide();
        }
    }
}