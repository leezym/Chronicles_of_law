using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static DIALOGUE.LogicalLines.LogicalLineUtils.Encapsulation;
using GAME;

namespace DIALOGUE.LogicalLines
{
    public class LL_Items : ILogicalLine
    {
        public string keyword => "item";

        public IEnumerator Execute(DIALOGUE_LINE line)
        { 
            string name = line.dialogueData.rawData;

            //Try to get the name or path to the sprite
            string filePath = FilePaths.GetPathToResource(FilePaths.resources_itemsFiles, "Caso2.Intermedio.DerechoFamilia/"+name); // se debe crear un Level Manager para configurar cada caso en su respectiva variable, crear un banco de casos aleatorio

            Sprite sprite = Resources.Load<Sprite>(filePath);

            if(sprite == null)
            {
                Debug.LogError($"Could not load sprite from path '{filePath}.' Please ensure it exists within Resources!");
                yield return null;
            }
            
            ItemsPanel itemsPanel = ItemsPanel.Instance;
            FolderPanel folderPanel = FolderPanel.Instance;
            
            itemsPanel.Show(sprite);

            if (!GameManager.Instance.items.TryGetValue(name, out _))
            {
                folderPanel.CreateItemPrefab(sprite, name);
                folderPanel.AddItemPrefab(sprite, name);
            }
            else
            {
                Debug.LogWarning($"A item called '{name}' already exists. Did not create the item");
            }            

            while(itemsPanel.isWaitingOnUserChoice)
                yield return null;
        }

        public bool Matches(DIALOGUE_LINE line)
        {
            return (line.hasSpeaker && line.speakerData.name.ToLower() == keyword);
        }
    }
}