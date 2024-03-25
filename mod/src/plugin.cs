using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using MonoMod.Utils;
using System.IO;
using MiniJSON;

namespace MapMaker
{
    [BepInPlugin("com.MLT.MapLoader", "MapLoader", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static GameObject PlatformAbility;
        public static Transform levelt;
        public static StickyRoundedRectangle platformPrefab;
        public static List<ResizablePlatform> Platforms;
        public static int t;
        public static string mapsFolderPath; // Create blank folder path var

        private void Awake()
        {
            Logger.LogInfo("MapLoader Has been loaded");
            Harmony harmony = new Harmony("com.MLT.MapLoader");

            Logger.LogInfo("Harmony harmony = new Harmony -- Melon, 2024");
            harmony.PatchAll(); // Patch Harmony
            Logger.LogInfo("MapMaker Patch Compleate!");

            SceneManager.sceneLoaded += OnSceneLoaded;

            mapsFolderPath = Path.Combine(Paths.PluginPath, "Maps");

            if (!Directory.Exists(mapsFolderPath))
            {
                Directory.CreateDirectory(mapsFolderPath);
                Logger.LogInfo("Maps folder created.");
            }
        }
        //CALL ONLY ON LEVEL LOAD!
        public static void LoadMapsFromFolder()
        {
            string[] mapFiles = Directory.GetFiles(mapsFolderPath, "*.boplmap");
            foreach (string mapFile in mapFiles)
            {
                try
                {
                    string mapJson = File.ReadAllText(mapFile);
                    Debug.Log($"Loaded map from file: {Path.GetFileName(mapFile)}");
                    SpawnPlatformsFromMap(mapJson);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load map from file: {Path.GetFileName(mapFile)}. Error: {ex.Message}");
                }
            }
        }

        public static void SpawnPlatformsFromMap(string mapJson)
        {
            //get the platform prefab out of the Platform ability gameobject (david) DO NOT REMOVE!
            //chatgpt code to get the Platform ability object
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            Debug.Log("getting platform object");
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "Platform")
                {
                    // Found the object with the desired name and HideAndDontSave flag
                    // You can now store its reference or perform any other actions
                    PlatformAbility = obj;
                    Debug.Log("Found the object: " + obj.name);
                    break;
                }
            }
            var platformTransform = PlatformAbility.GetComponent(typeof(PlatformTransform)) as PlatformTransform;
            platformPrefab = platformTransform.platformPrefab;
            //turn the json into a dicsanary. (david+chatgpt) dont remove it as it works.
            Dictionary<string, object> Dict = MiniJSON.Json.Deserialize(mapJson) as Dictionary<string, object>;
            List<object> platforms = (List<object>)Dict["platforms"];
            Debug.Log("platforms set");
            foreach (Dictionary<String, object> platform in platforms)
            {
                try
                {
                    // Extract platform data (david)
                    Dictionary<string, object> transform = (Dictionary<string, object>)platform["transform"];
                    Debug.Log("transform set");
                    Dictionary<string, object> size = (Dictionary<string, object>)platform["size"];
                    Debug.Log("size set");
                    double x = (double)transform["x"];
                    Debug.Log("x set");
                    double y = (double)transform["y"];
                    Debug.Log("y set");
                    double width = (double)size["width"];
                    Debug.Log("width set");
                    double height = (double)size["height"];
                    Debug.Log("hight set");
                    double radius = (double)platform["radius"];
                    Debug.Log("radius set");

                    // Spawn platform
                    SpawnPlatform((Fix)x, (Fix)y, (Fix)width, (Fix)height, (Fix)radius);
                    Debug.Log("Platform spawned successfully");
                }
                catch (Exception ex)
                {   
                    Debug.LogError($"Failed to spawn platform. Error: {ex.Message}");
                }
            }
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            if (scene.name == "Level1") // TODO: Check level, Replace with mapId from MapMaker Thing
            {
                //find the platforms and remove them (shadow + david)
                levelt = GameObject.Find("Level").transform;
                foreach (Transform tplatform in levelt)
                {
                    Updater.DestroyFix(tplatform.gameObject);
                }
                LoadMapsFromFolder();
            }
        }

        public static void SpawnPlatform(Fix X, Fix Y, Fix Width, Fix Height, Fix Radius)
        {
            // Spawn platform (david - and now melon)
            var StickyRect = FixTransform.InstantiateFixed<StickyRoundedRectangle>(platformPrefab, new Vec2(X, Y));
            StickyRect.rr.Scale = Fix.One;
            var platform = StickyRect.GetComponent<ResizablePlatform>();
            platform.GetComponent<DPhysicsRoundedRect>().ManualInit();
            ResizePlatform(platform, Width, Height, Radius);
            Debug.Log("Spawned platform at position (" + X + ", " + Y + ") with dimensions (" + Width + ", " + Height + ") and radius " + Radius);
        }

        public static void Update()
        {
            //ignore this its broken
            //if (Platforms.Count > 0)
            //{
            //    ResizePlatform(Platforms[0], (Fix)0.1, (Fix)0.1, (Fix)(5 + t * 0.05));
            //    t++;
            //}
        }

        //this can be called anytime the object is active. this means you can have animated levels with shape changing platforms
        public static void ResizePlatform(ResizablePlatform platform, Fix newWidth, Fix newHeight, Fix newRadius)
        {
            platform.ResizePlatform(newHeight, newWidth, newRadius, true);
        }

        // JSON reading code here.
    }
}
