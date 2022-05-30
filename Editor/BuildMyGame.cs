using System;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AndroidBuildGame
{
    [MenuItem("Tools/KoroBuild/Android/BuildExport")]
    public static void BuildExportProject()
    {

        GenericSetting();
        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
        BuildPipeline.BuildPlayer(GetScenes(), $"./Builds/Android-Export-Project", BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("Tools/KoroBuild/Android/BuildAPK")]
    public static void BuildAPK()
    {
        var scenes = EditorBuildSettings.scenes;
        string[] scenesPaths = new string[scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenesPaths[i] = scenes[i].path;
        }
        GenericSetting();
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
        BuildPipeline.BuildPlayer(GetScenes(), $"./Builds/Build.apk", BuildTarget.Android, BuildOptions.None);
    }

    public static string[] GetScenes()
    {
        var scenes = EditorBuildSettings.scenes;
        string[] scenesPaths = new string[scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenesPaths[i] = scenes[i].path;
        }
        return scenesPaths;
    }

    public static void GenericSetting()
    {
        PlayerSettings.Android.useCustomKeystore = false;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;
    }
}

