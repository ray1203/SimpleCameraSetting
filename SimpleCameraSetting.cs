using System.Reflection;
using Verse;
using HarmonyLib;
namespace SimpleCameraSetting
{
    [StaticConstructorOnStartup]
    public static class SimpleCameraSetting
    {
        static SimpleCameraSetting()
        {
            Harmony harmony = new Harmony("ray1203.SimpleCameraSetting");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
