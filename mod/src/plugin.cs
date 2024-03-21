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
                // Spawn multiple platforms with different parameters

                // Spawns a platform at the specified position with the given dimensions and corner radius.
                // Parameters:
                //
                //   X: The x-coordinate of the platform's center.
                //   Y: The y-coordinate of the platform's center.
                //   Width: The width of the platform.
                //   Height: The height of the platform.
                //   Radius: The radius of the rounded corners of the platform.
                //
                // SpawnPlatform((Fix)x, (Fix)y, (Fix)Width, (Fix)Height, (Fix)Radius);

                // Randomly generate new platforms between 5 and 25 times

                SpawnPlatform((Fix)8.058, (Fix)(-5.527), (Fix)2.365, (Fix)2.743, (Fix)4.683);
                SpawnPlatform((Fix)(-0.142), (Fix)5.989, (Fix)4.371, (Fix)4.801, (Fix)2.985f);
                SpawnPlatform((Fix)6.952, (Fix)(-7.186), (Fix)4.029, (Fix)4.873, (Fix)5.738);
                SpawnPlatform((Fix)9.051, (Fix)9.615, (Fix)4.784, (Fix)3.239, (Fix)7.612);
                SpawnPlatform((Fix)(-5.889), (Fix)9.606, (Fix)3.005, (Fix)4.621, (Fix)6.127f);
                SpawnPlatform((Fix)13.678, (Fix)(-1.508), (Fix)2.298, (Fix)4.003, (Fix)1.247);
                SpawnPlatform((Fix)(-2.401), (Fix)(-6.349), (Fix)4.283, (Fix)3.792, (Fix)0.862f);
                SpawnPlatform((Fix)10.362, (Fix)(-12.639), (Fix)3.747, (Fix)2.687, (Fix)8.744);
                SpawnPlatform((Fix)(-2.952), (Fix)13.016, (Fix)3.498, (Fix)1.471, (Fix)7.049f);
                SpawnPlatform((Fix)0.367, (Fix)(-5.642), (Fix)4.727, (Fix)1.442, (Fix)9.108);
                SpawnPlatform((Fix)(-14.062), (Fix)8.091, (Fix)1.911, (Fix)2.864, (Fix)2.917f);
                SpawnPlatform((Fix)11.842, (Fix)11.489, (Fix)4.576, (Fix)1.835, (Fix)8.763);
                SpawnPlatform((Fix)(-10.693), (Fix)(-3.311), (Fix)3.712, (Fix)3.068, (Fix)4.933f);
                SpawnPlatform((Fix)8.783, (Fix)(-10.665), (Fix)1.833, (Fix)4.288, (Fix)1.896);
                SpawnPlatform((Fix)3.888, (Fix)(-6.627), (Fix)2.839, (Fix)4.655, (Fix)7.945f);



                Debug.Log("platform(s) have been spawned");
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
