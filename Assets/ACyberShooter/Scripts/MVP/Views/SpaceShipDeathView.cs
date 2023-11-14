using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace MVC
{

    public class SpaceShipDeathView : MonoBehaviour // To Abstracts
    {

        [SerializeField] private Button _reloadButton;
        private Volume _deathVolume;


        public void InitView(int reloadSceneIndex, Volume deathVolume)
        {

            _deathVolume = deathVolume;
            _reloadButton.onClick.AddListener(() =>
            {
                
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(reloadSceneIndex);// Need Async

            });
        }

        private void FixedUpdate()
        {

            if(_deathVolume != null)
            {
               
                if(_deathVolume.weight > 0.07f)
                _deathVolume.weight -= Time.deltaTime * 5000f;
                else
                {
                    _deathVolume.weight = 0.07f;
                }
            }
            
        }
       
        private void OnDestroy()
        {

            _reloadButton.onClick.RemoveAllListeners();
        }
    }
}