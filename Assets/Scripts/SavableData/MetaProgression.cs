using UnityEngine;

namespace SavableData
{
    [CreateAssetMenu(fileName = "MetaProgressionData", menuName = "Data/MetaProgressionData", order = 1)]
    public class MetaProgression : ScriptableObject
    {
        [SerializeField] private int _progressionPoints;

        public int ProgressionPoints
        {
            get => _progressionPoints;
        }

        public void AppProgressionPoints(int progressionPoints)
        {
            _progressionPoints += progressionPoints;
        }
    }
}