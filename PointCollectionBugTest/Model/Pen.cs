using PointCollectionBugTest.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Point = System.Windows.Point;

namespace PointCollectionBugTest.Model
{
    public class Pen : INotifyPropertyChanged
    {
        private Point _endPoint;
        public Point EndPoint
        {
            get => _endPoint;
            set => Set(ref _endPoint, value);
        }

        private ObservableCollection<Point> _points;
        public ObservableCollection<Point> Points
        {
            get => _points;
            set => Set(ref _points, value);
        }

        private bool _isDrawn;
        public bool IsDrawn
        {
            get => _isDrawn;
            set => Set(ref _isDrawn, value);
        }

        private bool _isDrawing;
        public bool IsDrawing
        {
            get => _isDrawing;
            set => Set(ref _isDrawing, value);
        }

        public Pen()
        {
            Points = new ObservableCollection<Point>();
        }

        public void Create(Point point)
        {
            if (IsDrawn)
                return;

            EndPoint = point;
            IsDrawing = true;
        }

        public void Drawn(Point point)
        {
            IsDrawn = true;
            Detach();
        }

        public void Update(Point point)
        {
            if (IsDrawn || !IsDrawing)
                return;

            EndPoint = point;

            if (HandlersProvider.MouseHandler.IsMousePressed)
            {
                Points.Add(EndPoint);
                OnPropertyChanged("Points");
            }
        }
        public virtual void Attach()
        {
            HandlersProvider.MouseHandler.MouseDown += Create;
            HandlersProvider.MouseHandler.MousePosUpdated += Update;
            HandlersProvider.MouseHandler.MouseUp += Drawn;
        }

        public virtual void Detach()
        {
            HandlersProvider.MouseHandler.MouseDown -= Create;
            HandlersProvider.MouseHandler.MousePosUpdated -= Update;
            HandlersProvider.MouseHandler.MouseUp -= Drawn;
        }

        #region PROPERTYCHANGED 
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
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
