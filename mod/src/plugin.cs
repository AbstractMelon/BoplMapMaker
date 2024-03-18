using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using UnityEngine.SceneManagement;

namespace MapMaker
{
    [BepInPlugin("com.David_Loves_JellyCar_Worlds.MapMaker", "MapMaker", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Transform levelt;
        private void Awake()
        {
            Logger.LogInfo("Plugin MapMaker is loaded!");

            Harmony harmony = new Harmony("com.David_Loves_JellyCar_Worlds.MapMaker");

            Logger.LogInfo("harmany created");
            harmony.PatchAll();
            Logger.LogInfo("MapMaker Patch Compleate!");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            if (scene.name == "Level1")
            {
                //find the platforms and remove them (shadow + david)
                levelt = GameObject.Find("Level").transform;
                foreach (Transform tplatform in levelt)
                {
                    Updater.DestroyFix(tplatform.gameObject);
                }

            }
        }
    }
}