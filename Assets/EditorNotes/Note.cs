namespace EditorNotes
{
    [System.Serializable]
    public class Note
    {
        public string TargetObjectUniqueId { get; }

        public string content;

        public Note(string targetObjectUniqueId)
        {
            TargetObjectUniqueId = targetObjectUniqueId;
        }

        public Note(string targetObjectUniqueId, string rContent)
        {
            TargetObjectUniqueId = targetObjectUniqueId;
            content = rContent;
        }
        
        public void Save()
        {
            EditorNotesFileHandler.Write(content, TargetObjectUniqueId);
        }
    }
}