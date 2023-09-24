namespace MVC.Core.Interface.Controllers
{
    internal interface IFixedExecute : IController
    {
        public void FixedExecute(float fixedDeltaTime);
    }
}