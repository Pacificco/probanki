using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Bankiru.Models.Domain.Users;

namespace Bankiru.Models.OutApi
{
    public class UsersAutoControl : IHttpModule
    {
        static Timer timer;
        long _interval = 900000;
        static object _synclock = new object();
        static string _lastError = "";
        static bool _isProcessing = false;
        static bool _alreadyDone = false;
        
        public void Init(HttpApplication app)
        {            
            timer = new Timer(new TimerCallback(UsersControl), null, 0, _interval);
        }

        private void UsersControl(object obj)
        {
            if (_isProcessing) return;
            try
            {
                _isProcessing = true;
                DateTime curDateTime = DateTime.Now;
                if (curDateTime.Hour > 0 & !_alreadyDone)
                {
                    
                }
                else if (curDateTime.Hour > 2)
                {
                    _alreadyDone = false;
                }
            }
            finally
            {
                _isProcessing = false;
            }            
        }


        public void Dispose()
        {
        
        }
    }
}