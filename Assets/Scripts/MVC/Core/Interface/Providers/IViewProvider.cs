using MVC.Core.Interface.View;

namespace MVC.Core.Interface.Providers
{
    public interface IViewProvider
    {
        public void AddView(IView view);
        public T GetView<T>() where T : IView;
        public void LogAllViews();
    }
}