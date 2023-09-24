using System.Collections.Generic;
using MVC.Core.Interface.Providers;
using MVC.Core.Interface.View;

namespace MVC.Core.Providers
{
    public class ViewProvider : IViewProvider
    {
        private List<IView> _viewList;

        public ViewProvider()
        {
            _viewList = new List<IView>();
        }
        
        public void AddView(IView view)
        {
            _viewList.Add(view);
        }

        public T GetView<T>() where T : IView
        {
            List<IView> viewList = _viewList.FindAll(element => element.GetType() == typeof(T));
            return viewList[0] is T ? (T) viewList[0] : default;
        }
    }
}