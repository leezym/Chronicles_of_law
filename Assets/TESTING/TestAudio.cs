using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

namespace TESTING
{
    public class TestAudio : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Run());
        }

        Character CreateCharacter(String name) => CharacterManager.Instance.CreateCharacter(name);

        IEnumerator Run()
        {
            AudioManager.Instance.PlayTrack("Audio/Music/acustico", volumeCap: 0.5f);
            yield return null;
        }
    }
}