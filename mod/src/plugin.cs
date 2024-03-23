using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft;
using System;
using MonoMod.Utils;
using System.IO;

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
        private string mapsFolderPath; // Create blank folder path var

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
            else
            {
                LoadMapsFromFolder();
            }
        }

        private void LoadMapsFromFolder()
        {
            string[] mapFiles = Directory.GetFiles(mapsFolderPath, "*.boplmap");
            foreach (string mapFile in mapFiles)
            {
                try
                {
                    string mapJson = File.ReadAllText(mapFile);
                    Logger.LogInfo($"Loaded map from file: {Path.GetFileName(mapFile)}");
                    SpawnPlatformsFromMap(mapJson);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to load map from file: {Path.GetFileName(mapFile)}. Error: {ex.Message}");
                }
            }
        }

        private void SpawnPlatformsFromMap(string mapJson)
        {
            JObject mapData = JObject.Parse(mapJson);
            JArray platforms = (JArray)mapData["platforms"];
            foreach (JToken platform in platforms)
            {
                try
                {
                    // Extract platform data
                    float x = platform["transform"]["x"].Value<float>();
                    float y = platform["transform"]["y"].Value<float>();
                    float width = platform["size"]["width"].Value<float>();
                    float height = platform["size"]["height"].Value<float>();
                    float radius = platform["radius"].Value<float>();

                    // Spawn platform
                    SpawnPlatform(new Fix((int)x), new Fix((int)y), new Fix((int)width), new Fix((int)height), new Fix((int)radius));
                    Logger.LogInfo("Platform spawned successfully");
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to spawn platform. Error: {ex.Message}");
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
