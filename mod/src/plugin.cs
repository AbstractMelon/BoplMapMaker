using System;
using HarmonyLib;
using System.Reflection;

namespace HarmonyTest
{
    class Original
    {
        public static int RollDice()
        {
            var random = new Random();
            return random.Next(1, 7); // Roll dice from 1 to 6
        }
    }

    class Main
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Random roll: {Original.RollDice()}"); // Prints: "Random roll: <some number between 1 and 6>"

            // Actual patching is just a one-liner!
            Harmony.CreateAndPatchAll(typeof(Main));

            Console.WriteLine($"Random roll: {Original.RollDice()}"); // Will always print "Random roll: 4"
        }

        [HarmonyPatch(typeof(Original), "RollDice")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]                              // There are different patch types. Prefix code runs before original code
        static bool RollRealDice(ref int __result)
        {
            // https://xkcd.com/221/
            __result = 4; // The special __result variable allows you to read or change the return value
            return false; // Returning false in prefix patches skips running the original code
        }
    }
}