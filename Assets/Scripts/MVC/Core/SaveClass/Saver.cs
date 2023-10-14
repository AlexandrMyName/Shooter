using Configs;

internal static class Saver

{
    private static ConfigLoader s_configLoader;


    public static void Init(ConfigLoader configLoader)
    {
        s_configLoader = configLoader;
    }
    public static void Save(ISave concreteSaver)
    {
        concreteSaver.TrySave(s_configLoader);
    }
    public static void Load(ISave concreteSaver)
    {
        concreteSaver.TryLoad(s_configLoader);
    }


}
