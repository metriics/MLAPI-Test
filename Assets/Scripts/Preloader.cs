using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preloader : MonoBehaviour
{
    [SerializeField] private ConnManager cm;

    private void Awake()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--server")
            {
                // launch as server
                cm.Server();
            }
            else if (args[i] == "--passphrase")
            {
                try
                {
                    cm.SetPassphrase(args[i + 1]);
                } catch (IndexOutOfRangeException)
                {
                    
                }
            }
        }
    }
}
