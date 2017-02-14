using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Club
{
    public class VM_UserForecastState
    {
        public bool AddEnable { get; set; }
        public EnumDisableAddUserToForecast DisableState { get; set; }
    }
}