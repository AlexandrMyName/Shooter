using Abstracts;
using UnityEngine;


namespace Core
{
    
    public class ObjectStack : IGameComponents
    {
        
        private Camera _camera;
        private Transform _transform;
        private GameObject _gameObject;
        


        public ObjectStack(Camera cam, GameObject baseObject)
        {
            _camera = cam;
            _gameObject = baseObject;
            _transform = baseObject.transform;
        }
        
        
        public Camera MainCamera => _camera;

        public Transform BaseTransform => _transform;

        public GameObject BaseObject => _gameObject;

    }
}
