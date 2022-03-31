using PointCollectionBugTest.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointCollectionBugTest.Providers
{
    public static class HandlersProvider
    {
        private static MouseHandler _mouseHandler;
        public static MouseHandler MouseHandler
        {
            get
            {
                if (_mouseHandler == null)
                    _mouseHandler = new MouseHandler();
                return _mouseHandler;
            }
        }
    }
}
