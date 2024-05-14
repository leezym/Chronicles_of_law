using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace GAME
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public TMP_Text pointsText;

        public float points { get; private set; } = 0f;
        [SerializedDictionary("Name", "Sprite")]
        public SerializedDictionary<string, Sprite> items = new SerializedDictionary<string, Sprite>();

        private void Awake()
        {
            Instance = this;
        }

        public float GetPoints(){ return points; }

        public void SetPoints(float points)
        {
            this.points = points;
            pointsText.text = points.ToString();
        }        
    }
}