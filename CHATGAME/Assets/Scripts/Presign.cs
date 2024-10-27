using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Presign
{
    static Presign()
    {
        PlayerSettings.Android.keystorePass = "pok971!@";
        PlayerSettings.Android.keyaliasName = "chatgameKey";
        PlayerSettings.Android.keyaliasPass = "pok971!@";
    }
}
