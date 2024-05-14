using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using Unity.VisualScripting;
using UnityEngine;

namespace HISTORY
{
    //[RequireComponent(typeof(HistoryNavigation))] //Pdte falta History Navigation
    public class HistoryManager : MonoBehaviour
    {
        public const int HISTORY_CACHE_LIMIT = 20;
        public static HistoryManager Instance { get; private set; }
        public List<HistoryState> history = new List<HistoryState>();
        //private HistoryNavigation navigation;
        public HistoryState state;

        void Awake()
        {
            Instance = this;
            //navigation = GetComponent<HistoryNavigation>();    
        }

        void Start()
        {
            DialogueSystem.Instance.onClear += LogCurrentState;
        }

        public void LogCurrentState()
        {
            HistoryState state = HistoryState.Capture();
            history.Add(state);

            if(history.Count > HISTORY_CACHE_LIMIT)
                history.RemoveAt(0);
        }

        public void LoadState(HistoryState state)
        {
            state.Load();
        }

        //public void GoForward() => navigation.GoFoward();
        //public void GoBack() => navigation.GoBack();
    }
}