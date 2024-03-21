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

namespace MapMaker
{
    [BepInPlugin("com.David_Loves_JellyCar_Worlds.MapMaker", "MapMaker", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static GameObject PlatformAbility;
        public static Transform levelt;
        public static StickyRoundedRectangle platformPrefab;
        public static List<ResizablePlatform> Platforms;
        public static int t;
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
        //chatgpt code
        public static Dictionary<string, object> Json_decode(string jsonString)
        {
            // Deserialize the JSON string into a JObject
            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(jsonString);

            // Create a dictionary to hold the data
            Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

            // Iterate through each property in the JObject
            foreach (var property in jsonObject.Properties())
            {
                // If the property is an array, convert it to a list of dictionaries
                if (property.Value.Type == JTokenType.Array)
                {
                    var list = new List<Dictionary<string, object>>();
                    foreach (var item in property.Value)
                    {
                        var itemDict = item.ToObject<Dictionary<string, object>>();
                        list.Add(itemDict);
                    }
                    dataDictionary.Add(property.Name, list);
                }
                // Otherwise, add the property directly to the dictionary
                else
                {
                    dataDictionary.Add(property.Name, property.Value.ToObject<object>());
                }
            }
            return dataDictionary;
        }
    }
}