using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class SteamManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            SteamClient.Init( 2819630 );
        }
        catch (Exception e)
        {
            // Something went wrong - it's one of these:
            //
            //     Steam is closed?
            //     Can't find steam_api dll?
            //     Don't have permission to play app?
            //
            Debug.LogError(e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SteamClient.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        SteamClient.Shutdown();
    }
}
