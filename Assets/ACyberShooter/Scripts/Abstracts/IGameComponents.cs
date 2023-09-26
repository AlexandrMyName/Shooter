using UnityEngine;


namespace Abstracts
{
    
    public interface IGameComponents
    {
        
        Camera MainCamera { get; }
        Transform BaseTransform { get; }
        GameObject BaseObject { get; }
        
    }
}
