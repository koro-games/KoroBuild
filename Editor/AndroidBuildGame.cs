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

    public static void SignBuild()
    {
        var alias = GetArg("-alias");
        var pass = GetArg("-pass");
        if (alias != null && pass != null)
        {
            Sign(alias, pass);
        }
        else
        {
            PlayerSettings.Android.useCustomKeystore = false;
        }
    }


    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }

    private static void Sign(string alias, string pass)
    {
        PlayerSettings.Android.useCustomKeystore = true;

        PlayerSettings.Android.keystoreName = "/Users/user/Downloads/Ko_Studio_keystore.keystore";
        PlayerSettings.Android.keystorePass = pass;

        PlayerSettings.Android.keyaliasName = alias;
        PlayerSettings.Android.keyaliasPass = pass;
    }

    public static void GenericSetting()
    {
        SignBuild();
        VersionSet();
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
    }

    private static void VersionSet()
    {
        var version = 0;
        if (int.TryParse(GetArg("-version"), out version))
        {
            PlayerSettings.Android.bundleVersionCode = version;
        }
    }
}

