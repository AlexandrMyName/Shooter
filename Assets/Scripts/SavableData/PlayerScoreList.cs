using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace SavableData
{
    [CreateAssetMenu(fileName = "ScoreData", menuName = "Data/ScoreData", order = 0)]
    public class PlayerScoreList : ScriptableObject
    {
        [SerializeField] private List<int> _scoreList;
        [SerializeField] private int _maxListCapacity;
        private bool _isSuitable;

        public List<int> ScoreList => _scoreList;
        public int MaxListCapacity => _maxListCapacity;

        public void AddCurrentScoreToList(int score)
        {
            SuitableCheck(score);
            if (_scoreList.Count >= _maxListCapacity && _isSuitable)
            {
                _scoreList.Remove(_maxListCapacity - 1);
                _scoreList.Add(score);
            }
            else if(_isSuitable)
            {
                _scoreList.Add(score);
            }
            SortList();
        }

        [Button]
        public void ClearScores()
        {
            _scoreList.Clear();
        }

        private void SortList()
        {
            _scoreList.Sort();
            _scoreList.Reverse();
        }

        private void SuitableCheck(int score)
        {
            _isSuitable = true;
            foreach (int scoreFromList in _scoreList)
            {
                if (score == scoreFromList)
                {
                    _isSuitable = false;
                }
            }
        }
    }
}
