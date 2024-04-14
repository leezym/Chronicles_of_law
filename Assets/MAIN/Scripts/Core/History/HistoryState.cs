using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace History
{
    public class HistoryState
    {
        public DialogueData dialogue;
        public List<CharacterData> characters;
        public List<AudioData> audio;
        public List<GraphicData> graphics;

        public static HistoryState Capture() //pdte almacenar FOLDER Y PUNTOS. Pensar si tener todos los items en la carpeta pero desactivados, y activarlos cuando se los muestren. O crearlos siempre despues de leer la data
        {
            HistoryState state = new HistoryState();
            state.dialogue = DialogueData.Capture();
            state.characters = CharacterData.Capture();
            state.audio = AudioData.Capture();
            state.graphics = GraphicData.Capture();

            return state;
        }

        public void Load()
        {
            
        }
    }
}