using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace PointCollectionBugTest.Handlers
{
    public class MouseHandler : INotifyPropertyChanged
    {
        private double _mouseX;
        public double MouseX
        {
            get => _mouseX;
            set => Set(ref _mouseX, value);
        }

        private double _mouseY;
        public double MouseY
        {
            get => _mouseY;
            set => Set(ref _mouseY, value);
        }

        private bool _isMousePressed;
        public bool IsMousePressed
        {
            get => _isMousePressed;
            set => Set(ref _isMousePressed, value);
        }

        private Point _mouseDownPoint;
        public Point MouseDownPoint
        {
            get => _mouseDownPoint;
            set => Set(ref _mouseDownPoint, value);
        }

        private Point _mouseUpPoint;
        public Point MouseUpPoint
        {
            get => _mouseUpPoint;
            set => Set(ref _mouseUpPoint, value);
        }

        private Point _mouseCurrentPoint;
        public Point MouseCurrentPoint
        {
            get => _mouseCurrentPoint;
            set => Set(ref _mouseCurrentPoint, value);
        }

        public delegate void MousePosUpdatedHandler(Point point);
        public event MousePosUpdatedHandler MousePosUpdated;

        public delegate void MouseDownHandler(Point point);
        public event MouseDownHandler MouseDown;

        public delegate void MouseUpHandler(Point point);
        public event MouseUpHandler MouseUp;

        public void OnCanvasMouseMove(object args)
        {
            var e = (MouseEventArgs)args;

            MouseCurrentPoint = new Point(Convert.ToInt32(MouseX), Convert.ToInt32(MouseY));

            if (MousePosUpdated != null)
                MousePosUpdated(MouseCurrentPoint);

            e.Handled = true;
        }

        public void OnCanvasMouseDown(object args)
        {
            var e = (MouseButtonEventArgs)args;

            IsMousePressed = true;
            MouseDownPoint = new Point(Convert.ToInt32(MouseX), Convert.ToInt32(MouseY));

            if (MouseDown != null)
                MouseDown(MouseDownPoint);

            e.Handled = true;
        }


        public void OnCanvasMouseUp(object args)
        {
            var e = (MouseButtonEventArgs)args;

            IsMousePressed = false;
            MouseUpPoint = new Point(Convert.ToInt32(MouseX), Convert.ToInt32(MouseY));

            if (MouseUp != null)
                MouseUp(MouseUpPoint);

            e.Handled = true;
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
