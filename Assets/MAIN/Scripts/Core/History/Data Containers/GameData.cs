using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using GAME;

namespace HISTORY
{
    [System.Serializable]
    public class GameData
    {
        public float points = 0;
        public Dictionary<string, Sprite> items = new Dictionary<string, Sprite>();

        public static GameData Capture()
        {
            GameData data = new GameData();
            var gm = GameManager.Instance;

            data.points = gm.points;
            data.items = gm.items.ToDictionary(entry => entry.Key, entry => entry.Value);

            return data;
        }

        public static void Apply(GameData data)
        {
            var gm = GameManager.Instance;
            var fp = FolderPanel.Instance;

            gm.SetPoints(data.points);
            gm.items = data.items.ToDictionary(entry => entry.Key, entry => entry.Value);

            fp.ResetFolder();

            foreach(var item in gm.items)
                fp.CreateItemPrefab(item.Value, item.Key);
        }
    }
}