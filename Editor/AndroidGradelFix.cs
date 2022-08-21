using System;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

public class AndroidGradelFix
{
    const string c_gradePath = "/Plugins/Android/baseProjectTemplate.gradle";

    [MenuItem("Tools/KoroBuild/Android/FixGradel")]
    public static void BuildExportProject()
    {

        if (File.Exists(Application.dataPath + c_gradePath))
        {
            var lines = File.ReadLines(Application.dataPath + c_gradePath).ToArray();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("classpath 'com.android.tools.build:gradle:"))
                {
                    Debug.Log("Fix!");
                    lines[i] = "classpath 'com.android.tools.build:gradle:4.1.3'";
                    break;
                }
            }
            File.WriteAllLines(Application.dataPath + c_gradePath, lines);
        }
        else
        {
            throw new System.Exception("No baseProjectTemplate.gradle in project");
        }
    }
}

