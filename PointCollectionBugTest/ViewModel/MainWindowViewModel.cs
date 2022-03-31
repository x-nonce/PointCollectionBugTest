using PointCollectionBugTest.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Pen = PointCollectionBugTest.Model.Pen;
using System.Windows.Shapes;

namespace PointCollectionBugTest.ViewModel
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            HandlersProvider.MouseHandler.MouseDown += CreateTool;
            Tools = new ObservableCollection<Pen>();

            Pen pen = new Pen();
            pen.IsDrawn = true;

            CurrentTool = pen;
        }

        private Pen _currentTool;
        public Pen CurrentTool
        {
            get => _currentTool;
            set => Set(ref _currentTool, value);
        }

        private ObservableCollection<Pen> _tools;
        public ObservableCollection<Pen> Tools
        {
            get => _tools;
            set => Set(ref _tools, value);
        }

        private void CreateTool(Point point)
        {
            if (CurrentTool.IsDrawn)
                SetCurrentTool();

            CurrentTool.Create(point);
            Tools.Add(CurrentTool);
        }

        private void SetCurrentTool()
        {
            CurrentTool?.Detach();
            CurrentTool = new Pen();
            CurrentTool.Attach();
        }
        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        RelayCommand closeAppCommand;
        public RelayCommand CloseAppCommand
        {
            get
            {
                if (closeAppCommand == null)
                    closeAppCommand = new RelayCommand(o => CloseApplication());

                return closeAppCommand;
            }
        }

        #region POSITIONAL PROPERTIES

        private double _areaTop;
        public double AreaTop
        {
            get => _areaTop;
            set => Set(ref _areaTop, value);
        }

        private double _areaLeft;
        public double AreaLeft
        {
            get => _areaLeft;
            set => Set(ref _areaLeft, value);
        }


        private double _areaWidth;
        public double AreaWidth
        {
            get => _areaWidth;
            set => Set(ref _areaWidth, value);
        }

        private double _areaHeight;
        public double AreaHeight
        {
            get => _areaHeight;
            set => Set(ref _areaHeight, value);
        }

        #endregion

        #region MOUSE COMMANDS

        RelayCommand canvasMouseMoveCommand;
        public RelayCommand CanvasMouseMoveCommand
        {
            get
            {
                if (canvasMouseMoveCommand == null)
                    canvasMouseMoveCommand = new RelayCommand(o => HandlersProvider.MouseHandler.OnCanvasMouseMove(o));

                return canvasMouseMoveCommand;
            }
        }

        RelayCommand canvasMouseDownCommand;
        public RelayCommand CanvasMouseDownCommand
        {
            get
            {
                if (canvasMouseDownCommand == null)
                    canvasMouseDownCommand = new RelayCommand(o => HandlersProvider.MouseHandler.OnCanvasMouseDown(o));

                return canvasMouseDownCommand;
            }
        }

        RelayCommand canvasMouseUpCommand;
        public RelayCommand CanvasMouseUpCommand
        {
            get
            {
                if (canvasMouseUpCommand == null)
                    canvasMouseUpCommand = new RelayCommand(o => HandlersProvider.MouseHandler.OnCanvasMouseUp(o));

                return canvasMouseUpCommand;
            }
        }

        #endregion

        #region PROPERTYCHANGED 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }
        #endregion
    }
}
