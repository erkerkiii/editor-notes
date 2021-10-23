using System.IO;
using UnityEngine;

namespace EditorNotes
{
    internal static class EditorNotesFileHandler
    {
        private const string DIRECTORY_PATH = "EditorNotes";
        
        public static void Write(string text, string objectUniqueId)
        {
            if (!Directory.Exists(DIRECTORY_PATH))
            {
                Directory.CreateDirectory(DIRECTORY_PATH);
            }
            
            string filePath = GetFilePath(objectUniqueId);
            File.WriteAllText(filePath, text);
        }

        public static Note Read(string objectUniqueId)
        {
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