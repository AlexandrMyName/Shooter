namespace Abstracts
{
    
    public interface ISystem
    {
        
        void BaseAwake(IGameComponents baseObjectStack);
        void BaseStart();
        void BaseOnEnable();
        void BaseUpdate();
        void BaseLateUpdate();
        void BaseFixedUpdate();
        void BaseOnDestroy();

        void BaseOnDrawGizmos();


    }
}