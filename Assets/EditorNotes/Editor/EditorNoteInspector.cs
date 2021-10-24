using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EditorNotes.Editor
{
    [CustomEditor(typeof(Transform))]
    public class EditorNoteInspector : UnityEditor.Editor
    {
        private static bool _showNotes;
        
        private bool _isGettingDeleted;
        
        private Note _note;

        private GUIStyle _guiStyle;

        private void OnEnable()
        {
            _showNotes = EditorPrefs.GetBool("showEditorNotes", true);
        }

        public override void OnInspectorGUI()
        {
            InitializeGUIStyle();
            CheckNotes();
            
            base.OnInspectorGUI();

            if (_note != null)
            {
                DrawTextArea();
            }
            else
            {
                if (GUILayout.Button("Create Notes", GUILayout.Height(50f)))
                {
                    CreateNote();
                    _note.content = "Start typing your notes here";
                    SaveNote();
                }
            }
        }

        private void OnSceneGUI()
        {
            if (_note != null)
            {
                Handles.Label(GetTargetGameObject().transform.position, _note?.content);
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
                fontSize = 18,
                richText = true
            };
        }

        private void DrawTextArea()
        {
            _showNotes = EditorGUILayout.BeginFoldoutHeaderGroup(_showNotes, "Notes");
            if (!_showNotes)
            {
                return;
            }
            
            EditorGUILayout.BeginVertical("window");

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(
                $"<color=green><b>Notes for</b></color> <color=yellow><i>{target.name}</i></color>", _guiStyle);

            if (GUILayout.Button("Delete"))
            {
                DeleteNote();
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();

            _note.content =
                EditorGUILayout.TextArea(_note.content, GUILayout.MinHeight(50f));

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            EditorUtility.SetDirty(target);
        }

        private void DeleteNote()
        {
            _isGettingDeleted = true;
            EditorNoteContainer.DeleteNote(GetTargetGameObject());
            
            SaveScene();
        }

        private void CheckNotes()
        {
            _note = EditorNoteContainer.GetNote(GetTargetObjectUniqueId());
        }
        
        private void CreateNote()
        {
            _note = EditorNoteContainer.AddNote(GetTargetGameObject());

            SaveScene();
        }

        [MenuItem("GameObject/Add Note", false, 9)]
        public static void CreateNoteOnSelectedObject(MenuCommand menuCommand)
        {
            GameObject selectedGameObject = menuCommand.context as GameObject;
            if (selectedGameObject == null)
            {
                return;
            }
            
            EditorNoteContainer.AddNote(selectedGameObject);
            SaveScene();
        }

        private static void SaveScene()
        {
            EditorSceneManager.SaveOpenScenes();
        }

        private GameObject GetTargetGameObject()
        {
            return ((Transform)target).gameObject;
        }

        private string GetTargetObjectUniqueId()
        {
            return GetTargetGameObject().TryGetComponent(out EditorNoteUniqueIdLinker uniqueIdLinker)
                ? uniqueIdLinker.Id
                : string.Empty;
        }

        private void SaveNote()
        {
            if (_isGettingDeleted)
            {
                return;
            }
            _note?.Save();
        }

        private void OnDisable()
        {
            SaveNote();
            
            EditorPrefs.SetBool("showEditorNotes", _showNotes);
        }
    }
}