using Configs;
using SavableData;
using UniRx;
using UnityEngine;

internal sealed class PlayerPrefsParserModule : ISave
{
    private const string TOTAL_SCORE_KEY = "total_score";
    private const string TOTAL_SCORE_COUNT_KEY = "total_score_count";

    public void TrySave(ConfigLoader configLoader)
    {
        PlayerScoreList playerScoreList = GetScriptable<PlayerScoreList>(configLoader);
        SavePlayerScoreList(playerScoreList);
    }
    public void TryLoad(ConfigLoader configLoader)
    {
        PlayerScoreList playerScoreList = GetScriptable<PlayerScoreList>(configLoader);
        LoadPlayerScoreList(playerScoreList);
    }
    private T GetScriptable<T>(ConfigLoader configLoader) where T : ScriptableObject
    {
        return configLoader.LoadedScriptables.Find(scriptable => scriptable.GetType() == typeof(T)) as T;
    }
    private void SavePlayerScoreList(PlayerScoreList playerScoreList)
    {
        int i = 0;
        foreach (int score in playerScoreList.ScoreList)
        {
            string key = TOTAL_SCORE_KEY + i;
            PlayerPrefs.SetInt(key, score);
            i++;
        }
        PlayerPrefs.SetInt(TOTAL_SCORE_COUNT_KEY, i);
    }

    private void LoadPlayerScoreList(PlayerScoreList playerScoreList)
    {
        int count = PlayerPrefs.GetInt(TOTAL_SCORE_COUNT_KEY);
        playerScoreList.ClearScores();
        for (int i = 0; i < count; i++)
        {
            string key = TOTAL_SCORE_KEY + i;
            playerScoreList.AddCurrentScoreToList(PlayerPrefs.GetInt(key));
        }
    }

}
