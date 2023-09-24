namespace MVC.Core.Interface.Controllers
{
    public interface IControllers
    {
        public MVC.Core.Controllers Add(IController controller);

        public void PreInitialization();
        public void Initialization();
        public void Execute(float deltaTime);
        public void FixedExecute(float fixedDeltaTime);
        public void Cleanup();

    }
}