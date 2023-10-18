using Abstracts;


namespace Core.Input
{
    
    public sealed class Input : IInput
    {
        
        public PlayerInput PlayerInput { get; }


        public Input()
        {
            PlayerInput = new PlayerInput();
        }
        
        
    }
}