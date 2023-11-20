using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{

    public class SceneObjectManager : MonoBehaviour
    {

        [SerializeField] private List<GameObject> _hidenObjects;
        [SerializeField] private List<GameObject> _shownObjects;


        public void ShowAll()
        {
            _shownObjects.ForEach(shown => shown.SetActive(true));
        }

        public void HideAll()
        {
            _hidenObjects.ForEach(shown => shown.SetActive(false));
        }


        public void SetDefaults()
        {
            Debug.Log("SetDefault");
            ShowAll();
            HideAll();
        }
    }
}