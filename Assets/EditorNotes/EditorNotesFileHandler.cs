using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EditorNotes
{
    internal static class EditorNotesFileHandler
    {
        private const string DIRECTORY_PATH = "EditorNotes";

        public static void Write(string text, string objectUniqueId)
        {
            if (string.IsNullOrEmpty(objectUniqueId))
            {
                return;
            }
            
            if (!Directory.Exists(DIRECTORY_PATH))
            {
                try
                {
                    Directory.CreateDirectory(DIRECTORY_PATH);
                }
                catch (Exception e)
                {
                    EditorUtility.DisplayDialog("Editor Notes",
                        $"Couldn't create directory on {DIRECTORY_PATH}. Please check your permissions or create the directory manually.",
                        "Okay");

                    Debug.LogError(e);
                }
            }


            string filePath = GetFilePath(objectUniqueId);
            File.WriteAllText(filePath, text);
        }

        public static Note Read(string objectUniqueId)
        {
            if (string.IsNullOrEmpty(objectUniqueId))
            {
                return null;
            }
            
            string filePath = GetFilePath(objectUniqueId);
            if (!File.Exists(filePath))
            {
                return null;
            }

            string noteContent = File.ReadAllText(filePath);
            return new Note(objectUniqueId, noteContent);
        }

        public static void Delete(string objectUniqueId)
        {
            string filePath = GetFilePath(objectUniqueId);
            if (!File.Exists(filePath))
            {
                return;
            }

            File.Delete(filePath);
        }

        private static string GetFilePath(string objectUniqueId)
        {
            string filePath = $"{DIRECTORY_PATH}/{objectUniqueId}.txt";
            return filePath;
        }
    }
}