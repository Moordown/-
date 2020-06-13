using System.Collections.Generic;

public static class Config
{
    public static Dictionary<string, float> mixerValues = new Dictionary<string, float>
    {
        {"currentVolumeForBackgroundEffects", 0},
        {"currentVolumeForMusic", -10}
    };

    public static string MenuSceneName = "MenuScene";
    public static string FightSceneName = "FightScene";
    public static string RailwaySceneName = "RailwayScene";
}
