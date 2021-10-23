using UnityEditor;
using UnityEngine;

namespace EditorNotes
{
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class EditorNoteUniqueIdLinker : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField]
        private string _id;

        public string Id => _id;

        internal string GenerateId()
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = GUID.Generate().ToString();
            }

            return _id;
        }
#endif
    }
}