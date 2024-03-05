using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_Audio : CMD_DatabaseExtension
    {
        private static string PARAM_SONG = "-musica";
        private static string PARAM_CHANNEL = "-canal";
        private static string PARAM_VOLUME = "-vol";

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("reproducir", new Action<string[]>(PlaySong));
            database.AddCommand("detener", new Action<string>(StopSong));
        }

        private static void PlaySong(string[] data)
        {
            string filePath;
            int channel;
            var parameters = ConvertDataToParameters(data);

            //Try to get the name or path to the track
            parameters.TryGetValue(PARAM_SONG, out filePath);
            filePath = FilePaths.GetPathToResource(FilePaths.resources_music, filePath);

            //Try to get the channel for this track
            parameters.TryGetValue(PARAM_CHANNEL, out channel, defaultValue: 0);

            PlayTrack(filePath, channel, parameters);
        }

        private static void PlayTrack(string filePath, int channel, CommandParameters parameters)
        {
            float volumeCap;
            
            //Try to get the max volume of the track
            parameters.TryGetValue(PARAM_VOLUME, out volumeCap, defaultValue: 1f);

            //Run the logic
            AudioClip sound = Resources.Load<AudioClip>(filePath);

            if(sound == null)
            {
                Debug.LogError($"Could not load sound from path '{filePath}.' Please ensure it exists within Resources!");
                return;
            }

            AudioManager.Instance.PlayTrack(sound, channel, volumeCap, filePath);
        }

        public static void StopSong(string data)
        {
            if(data == string.Empty)
                StopTrack("0");
            else
                StopTrack(data);
        }

        public static void StopTrack(string data)
        {
            if(int.TryParse(data, out int channel))
                AudioManager.Instance.StopTrack(channel);
            else
                AudioManager.Instance.StopTrack(data);
        }
    }
}
