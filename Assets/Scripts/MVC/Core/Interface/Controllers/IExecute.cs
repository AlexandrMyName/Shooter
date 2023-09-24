namespace MVC.Core.Interface.Controllers
{
    internal interface IExecute : IController
    {
        public void Execute(float deltaTime);
    }
}