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

            // Make folder path
            mapsFolderPath = Path.Combine(Paths.PluginPath, "Maps");

            // Make Folder
            if (!Directory.Exists(mapsFolderPath))
            {
                Directory.CreateDirectory(mapsFolderPath);
                Logger.LogInfo("Maps folder created.");
            }
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            if (scene.name == "Level1") // Check level, Replace with mapId from MapMaker Thing
            {
                //find the platforms and remove them (shadow + david)
                levelt = GameObject.Find("Level").transform;
                foreach (Transform tplatform in levelt)
                {
                    Updater.DestroyFix(tplatform.gameObject);
                }
                //spawn test platform (david)
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
                //get the platform prefab out of the Platform ability gameobject (david)
                var platformTransform = PlatformAbility.GetComponent(typeof(PlatformTransform)) as PlatformTransform;
                platformPrefab = platformTransform.platformPrefab as StickyRoundedRectangle;
                Debug.Log(platformPrefab);
                //make it a grass platform (sets it but doesnt change anything)
                // TODO make it change the texter asset in the renderer
                platformPrefab.platformType = PlatformType.grass;
                //spawn test platform (david)
                SpawnPlatform((Fix)0, (Fix)0, (Fix)0.01, (Fix)0.01, (Fix)5);
                Debug.Log("platform(s) have been spawned");
            }
        }

        public static void SpawnPlatform(Fix X, Fix Y, Fix Width, Fix Height, Fix Radius)
        {
            //spawn platform (david)
            var StickyRect = FixTransform.InstantiateFixed<StickyRoundedRectangle>(platformPrefab, new Vec2(X, Y));
            StickyRect.rr.Scale = Fix.One;
            var platform = StickyRect.GetComponent<ResizablePlatform>();
            platform.GetComponent<DPhysicsRoundedRect>().ManualInit();
            ResizePlatform(platform, Width, Height, Radius);
            Debug.Log("spawned platform");
            //Debug.Log(platform);
            //Platforms.Add(platform);
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
