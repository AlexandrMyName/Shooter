using EventBus;
using UnityEngine;

public class GameFinisher : MonoBehaviour
{
    [SerializeField] private PlayerScoreList _playerScoreList;
    
    void Start()
    {
        PlayerEvents.OnGameEnded += EndGameActions;
    }
    
    void OnDestroy()
    {
        PlayerEvents.OnGameEnded -= EndGameActions;
    }

    private void EndGameActions(int score)
    {
        AddGameScoreToList(score);
    }
    
    private void AddGameScoreToList(int score)
    {
        _playerScoreList.AddCurrentScoreToList(score);
    }
}
