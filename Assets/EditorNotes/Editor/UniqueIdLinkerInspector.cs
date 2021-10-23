using UnityEditor;
using UnityEngine;

namespace EditorNotes.Editor
{
    [CustomEditor(typeof(EditorNoteUniqueIdLinker))]
    public class UniqueIdLinkerInspector : UnityEditor.Editor
    {
        private GUIStyle _guiStyle;
        
        private string _id;
        
        public override void OnInspectorGUI()
        {
            _guiStyle ??= new GUIStyle
            {
                fontSize = 14,
                richText = true
            };
            
            if (string.IsNullOrEmpty(_id))
            {
                SerializedObject serializedObject = new SerializedObject(target);
                _id = serializedObject.FindProperty("_id").stringValue;
            }

            EditorGUILayout.LabelField($"<color=yellow>{_id}</color>", _guiStyle);
        }
    }
}