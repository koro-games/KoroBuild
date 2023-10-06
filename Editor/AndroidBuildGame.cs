using System;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

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

#if DEBUG_MODE
            PlayerSettings.Android.useCustomKeystore = false;
            return;
#endif

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

        var path = "/Users/user/Downloads/Ko_Studio_keystore.keystore";
        if (File.Exists("Assets/Editor/KeystorePathReplace.txt"))
        {
            Debug.Log("Replace keystore path");
            path = File.ReadAllText("Assets/Editor/KeystorePathReplace.txt");
        }
        PlayerSettings.Android.keystoreName = path;
        PlayerSettings.Android.keystorePass = pass;

        PlayerSettings.Android.keyaliasName = alias;
        PlayerSettings.Android.keyaliasPass = pass;
    }

    public static void GenericSetting()
    {
        SignBuild();
        VersionSet();
        SetAndroidBuildScriptingBackend();
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
    }

    private static void VersionSet()
    {
        var version = 0;
        if (int.TryParse(GetArg("-versionCode"), out version))
        {
            PlayerSettings.Android.bundleVersionCode = version;
        }
    }

    private static void SetAndroidBuildScriptingBackend()
    {
        var version = 0;
        if (int.TryParse(GetArg("-buildMono"), out version))
        {
            if (version == 1)
            {
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
            }
            else
            {
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            }
        }
    }
}

