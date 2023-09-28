namespace Abstracts
{
    
    public abstract class BaseSystem : ISystem
    {
        
        protected IGameComponents ComponentCollection;

        protected virtual void OnEnable() { }
        protected virtual void Awake(IGameComponents components) { }
        protected virtual void Start() { }
        protected virtual void FixedUpdate() { }
        protected virtual void Update() { }
        protected virtual void LateUpdate() { }
        protected virtual void OnDestroy() { }
        protected virtual void OnDrawGizmos() {}

        #region baseImplement

        public void BaseAwake(IGameComponents components) => Awake(components);
        public void BaseStart() => Start();
        public void BaseOnEnable() => OnEnable();
        public void BaseUpdate() => Update();
        public void BaseFixedUpdate() => FixedUpdate();
        public void BaseLateUpdate() => LateUpdate();
        public void BaseOnDestroy() => OnDestroy();
        public void BaseOnDrawGizmos() => OnDrawGizmos();

        #endregion
    }
}
