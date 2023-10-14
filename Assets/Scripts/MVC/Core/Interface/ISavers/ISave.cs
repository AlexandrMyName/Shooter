using Configs;

internal interface ISave
{
    public void TrySave(ConfigLoader configLoader);
    public void TryLoad(ConfigLoader configLoader);
}
