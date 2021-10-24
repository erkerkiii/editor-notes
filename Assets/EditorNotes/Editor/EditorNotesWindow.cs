using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorNotes.Editor
{
    public class EditorNotesWindow : EditorWindow
    {
        private GUIStyle _guiStyle;

        private GameObject[] _objectsWithNotes;
        
        [MenuItem("EditorNotes/Notes In Scene")]
        public static void Display()
        {
            EditorNotesWindow window = GetWindow<EditorNotesWindow>();
            window.titleContent = new GUIContent("Notes In Scene");
            window.Show();
        }

        private void OnGUI()
        {
            if (_guiStyle == null)
            {
                InitializeGUIStyle();
            }
            
            if (_objectsWithNotes == null)
            {
                LoadObjectsWithNotes();
            }
            
            DrawObjectsWithNotes();

            if (GUILayout.Button("Refresh"))
            {
                LoadObjectsWithNotes();
            }
            
        }
        
        private void InitializeGUIStyle()
        {
            if (_guiStyle != null)
            {
                return;
            }

            _guiStyle = new GUIStyle
            {
                fontSize = 14,
                richText = true,
                alignment = TextAnchor.MiddleCenter
            };
        }

        private void DrawObjectsWithNotes()
        {
            EditorGUILayout.BeginVertical("window");
            
            for (int index = 0; index < _objectsWithNotes?.Length; index++)
            {
                GameObject gameObjectsWithNote = _objectsWithNotes[index];

                DrawGameObjectWithNoteItem(gameObjectsWithNote);
            }
            
            EditorGUILayout.EndVertical();
        }

        private void DrawGameObjectWithNoteItem(GameObject gameObjectsWithNote)
        {
            Rect buttonRect = EditorGUILayout.BeginHorizontal("Button");

            if (GUI.Button(buttonRect, GUIContent.none))
            {
                Selection.activeObject = gameObjectsWithNote;
            }
            
            GUILayout.Label($"<color=yellow>{gameObjectsWithNote.name}</color>", _guiStyle);

            EditorGUILayout.EndHorizontal();
        }

        private void LoadObjectsWithNotes() =>
            _objectsWithNotes = FindObjectsOfType<EditorNoteUniqueIdLinker>()
                .Where(u => !string.IsNullOrEmpty(u.Id))
                .Select(u => u.gameObject)
                .ToArray();
    }
}