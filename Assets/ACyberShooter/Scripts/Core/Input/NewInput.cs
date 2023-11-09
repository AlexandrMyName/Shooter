using Abstracts;


namespace Core
{
    
    public sealed class NewInput : IInput
    {

        private static NewInput InputInstance = null;

        public PlayerInput PlayerInput { get; }


        private NewInput()
        {
            PlayerInput = new PlayerInput();
        }
        

        public static NewInput Instance
        {
            get
            {
                if (InputInstance == null)
                {
                    InputInstance = new NewInput();
                }
                return InputInstance;
            }
        }
        
        
    }
}