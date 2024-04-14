using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

namespace History
{
    [System.Serializable]
    public class CharacterData
    { 
        public string characterName;
        public string displayName;
        public bool enabled;
        public Color color;
        public int priority;
        public bool isHighlighted;
        public bool isFacingLeft;
        public Vector2 position;
        public CharacterConfigCache characterConfig;

        public string dataJSON;

        [System.Serializable]
        public class CharacterConfigCache
        {
            public string name;

            public Character.CharacterType characterType;

            public Color nameColor;
            public Color dialogueColor;

            public string nameFont;
            public string dialogueFont;

            public CharacterConfigCache(CharacterConfigData reference)
            {
                name = reference.name;
                characterType = reference.characterType;

                nameColor = reference.nameColor;
                dialogueColor = reference.dialogueColor;

                nameFont = FilePaths.resources_font + reference.nameFont.name;
                dialogueFont = FilePaths.resources_font + reference.dialogueFont.name;
            }            
        }

        public static List<CharacterData> Capture()
        {
            List<CharacterData> characters = new List<CharacterData>();

            foreach(var character in CharacterManager.Instance.allCharacters)
            {
                if(!character.Value.isVisible)
                    continue;
                
                CharacterData entry = new CharacterData();
                entry.characterName = character.Value.name;
                entry.enabled = character.Value.isVisible;
                entry.position = character.Value.targetPosition;
                entry.characterConfig = new CharacterConfigCache(character.Value.config);

                 switch(character.Value.config.characterType)
                 {
                    case Character.CharacterType.Sprite:
                    case Character.CharacterType.SpriteSheet:
                        SpriteData sData = new SpriteData();
                        sData.layers = new List<SpriteData.LayerData>();

                        Character_Sprite sc = character.Value as Character_Sprite;
                        foreach(var layer in sc.layers)
                        {
                            var layerData = new SpriteData.LayerData();
                            layerData.color = layer.renderer.color;
                            layerData.spriteName = layer.renderer.sprite.name;
                            sData.layers.Add(layerData);
                        }

                        entry.dataJSON = JsonUtility.ToJson(sData);
                        break;
                 }

                 characters.Add(entry);
            }

            return characters;
        }

        [System.Serializable]
        public class SpriteData
        {
            public List<LayerData> layers;

            [System.Serializable]
            public class LayerData
            {
                public string spriteName;
                public Color color;
            }
        }
    }
}
