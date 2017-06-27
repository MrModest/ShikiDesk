using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShikiDesk
{
    public interface IDialogService
    {
        bool? ShowDialog(ViewModel.ViewModel vm);

        void Show(ViewModel.ViewModel vm);
    }

    public class DialogService : IDialogService
    {
        private static DialogService _default;
        private readonly Stack<Window> _parents = new Stack<Window>();

        private readonly IDictionary<Type, Type> _windows = new Dictionary<Type, Type>();

        public DialogService(Window mainWindow)
        {
            _parents.Push(mainWindow);
        }

        public DialogService()
        {
        }

        public static DialogService Default => _default ?? (_default = new DialogService());

        public bool? ShowDialog(ViewModel.ViewModel vm)
        {
            var vmType = vm.GetType();
            if (!_windows.ContainsKey(vmType))
                throw new ArgumentException("no registered window for type " + vmType.Name);

            var viewType = _windows[vmType];

            var view = (Window)Activator.CreateInstance(viewType);
            view.DataContext = vm;
            if (_parents.Count > 0)
                view.Owner = _parents.Peek();

            view.Closed += WindowClosed;
            _parents.Push(view);

            return view.ShowDialog();
        }

        public void Show(ViewModel.ViewModel vm)
        {
            var vmType = vm.GetType();
            if (!_windows.ContainsKey(vmType))
                throw new ArgumentException("no registered window for type " + vmType.Name);

            var viewType = _windows[vmType];

            var view = (Window)Activator.CreateInstance(viewType);
            view.DataContext = vm;
            if (_parents.Count > 0)
                view.Owner = _parents.Peek();

            view.Closed += WindowClosed;
            _parents.Push(view);

            view.Show();
        }

        public void SetMainParent(Window window)
        {
            _parents.Clear();
            _parents.Push(window);
        }

        public void Register<TViewModel, TView>()
            where TViewModel : ViewModel.ViewModel
            where TView : Window
        {
            _windows.Add(typeof(TViewModel), typeof(TView));
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            var view = (Window)sender;
            view.Closed -= WindowClosed;
            _parents.Pop();

            var vm = view.DataContext as IDisposable;
            vm?.Dispose();
        }
    }
}
