using System.IO;
using Game.CampaignMode;
using Steamworks;
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

            // if (GUILayout.Button("Generate JSON File"))
            // {
            //     var jsonData = manager.GenerateJson();
            //     Debug.Log(jsonData);
            // }
            //
            if (GUILayout.Button("Upload JSON File"))
            {
                manager.UpdateCloud();
            }

            if (GUILayout.Button("Download Cloud File"))
            {
                
            }
            
            if (GUILayout.Button("Steam enabled check"))
            {
                Debug.Log("1: " + SteamRemoteStorage.QuotaBytes);
                Debug.Log("2: " + SteamRemoteStorage.QuotaRemainingBytes);
                Debug.Log("3: " + SteamRemoteStorage.QuotaUsedBytes);
            }
        }
    }
}