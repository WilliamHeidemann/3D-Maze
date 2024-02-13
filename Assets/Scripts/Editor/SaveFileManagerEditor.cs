using System.IO;
using Game.CampaignMode;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    [CustomEditor(typeof(SaveFileManager))]
    public class SaveFileManagerEditor : UnityEditor.Editor
    {
        private SaveFileManager manager;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            manager = (SaveFileManager)target;

            if (GUILayout.Button("Generate JSON File"))
            {
                var filePath = manager.GenerateJson();
                var jsonData = File.ReadAllText(filePath);
                Debug.Log(jsonData);
            }
            
            if (GUILayout.Button("Upload JSON File"))
            {
                
            }

            if (GUILayout.Button("Download Cloud File"))
            {
                
            }
        }
    }
}