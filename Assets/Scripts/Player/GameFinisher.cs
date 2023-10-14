using Core.ResourceLoader;
using EventBus;
using SavableData;
using UnityEngine;

namespace Player
{
    public class GameFinisher : MonoBehaviour
    {
        private PlayerScoreList _playerScoreList;
        private MetaProgression _metaProgression;
    
        void Start()
        {
            _playerScoreList = ResourceLoadManager.GetConfig<PlayerScoreList>();
            _metaProgression = ResourceLoadManager.GetConfig<MetaProgression>();
            PlayerEvents.OnGameEnded += EndGameActions;
        }
    
        void OnDestroy()
        {
            PlayerEvents.OnGameEnded -= EndGameActions;
        }

        private void EndGameActions(int score, int progressPoints)
        {
            _playerScoreList.AddCurrentScoreToList(score);
            SaveParametres();
            _metaProgression.AppProgressionPoints(progressPoints);
			SaveParametres();
        }
        private void SaveParametres()
        {
            Saver.Save(new PlayerPrefsParserModule());
            PlayerPrefs.Save();
        }
        private void SaveParametres()
        {
            Saver.Save(new PlayerPrefsParserModule());
            PlayerPrefs.Save();
        }
    }
}
