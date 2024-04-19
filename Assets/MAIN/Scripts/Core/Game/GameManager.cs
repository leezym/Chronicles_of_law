using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GAME
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public TMP_Text pointsText;

        public float points { get; private set; } = 0f;
        public Dictionary<string, Sprite> items = new Dictionary<string, Sprite>();

        private void Awake()
        {
            Instance = this;
        }

        public void SetPoints(float points)
        {
            this.points = points;
            pointsText.text = points.ToString();
        }        
    }
}