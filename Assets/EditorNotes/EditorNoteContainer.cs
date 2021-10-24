using System.Collections.Generic;
using UnityEngine;

namespace EditorNotes
{
    public static class EditorNoteContainer
    {
        private static readonly Dictionary<string, Note> NoteRegistry = new Dictionary<string, Note>();

        public static Note GetNote(string uniqueId)
        {
            if (NoteRegistry.ContainsKey(uniqueId))
            {
                return NoteRegistry[uniqueId];
            }

            Note readNote = EditorNotesFileHandler.Read(uniqueId);
            if (readNote != null)
            {
                AddToRegistry(readNote);
            }

            return readNote;
        }

        private static Note CreateNewNote(GameObject gameObject)
        {
            string id = gameObject.AddComponent<EditorNoteUniqueIdLinker>().GenerateId();
            Note note = new Note(id);

            note.Save();
            return note;
        }

        private static void AddToRegistry(Note note)
        {
            if (NoteRegistry.ContainsKey(note.TargetObjectUniqueId) || string.IsNullOrEmpty(note.TargetObjectUniqueId))
            {
                return;
            }

            NoteRegistry.Add(note.TargetObjectUniqueId, note);
        }

        private static void RemoveFromRegistry(string uniqueId)
        {
            if (!NoteRegistry.ContainsKey(uniqueId))
            {
                return;
            }

            NoteRegistry.Remove(uniqueId);
        }

        public static Note AddNote(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out EditorNoteUniqueIdLinker uniqueIdLinker))
            {
                if (NoteRegistry.ContainsKey(uniqueIdLinker.Id))
                {
                    return null;
                }
            }
            
            Note note = CreateNewNote(gameObject);
            AddToRegistry(note);

            return note;
        }

        public static void DeleteNote(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out EditorNoteUniqueIdLinker uniqueIdLinker))
            {
                return;
            }
            
            string objectUniqueId = uniqueIdLinker.Id;
            EditorNotesFileHandler.Delete(objectUniqueId);
            RemoveFromRegistry(objectUniqueId);
            Object.DestroyImmediate(uniqueIdLinker);
        }
    }
}