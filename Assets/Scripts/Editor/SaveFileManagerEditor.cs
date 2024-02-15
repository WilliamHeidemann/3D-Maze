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
                manager.LoadSaveFile();
            }
            
            if (GUILayout.Button("Quota check"))
            {
                Debug.Log("Quota bytes: " + SteamRemoteStorage.QuotaBytes);
                Debug.Log("Quota remaining bytes: " + SteamRemoteStorage.QuotaRemainingBytes);
                Debug.Log("Quota used bytes: " + SteamRemoteStorage.QuotaUsedBytes);
            }
        }
    }
}