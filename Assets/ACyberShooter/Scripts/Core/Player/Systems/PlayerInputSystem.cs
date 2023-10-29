using Abstracts;


namespace Core
{
    
    public sealed class PlayerInputSystem : BaseSystem
    {
        
        private IGameComponents _components;
        private IComponentsStorage _componentsStorage;
        private PlayerInput _input;
        
        
        protected override void Awake(IGameComponents components)
        {
            _components = components;
            _componentsStorage = _components.BaseObject.GetComponent<IComponentsStorage>();
            _input = _componentsStorage.Input.PlayerInput;
        }


        protected override void OnEnable()
        {
            _input.Enable();
        }
        
        
        protected override void OnDisable()
        {
            _input.Disable();
        }
        
        
    }
}