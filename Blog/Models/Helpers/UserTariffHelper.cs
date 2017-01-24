﻿using Bankiru.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Helpers
{
    public static class UserTariffHelper
    {
        public static int GetEnabledForecastsCountForMonthByTariff(VM_ForecastTariff tariff)
        {
            switch (tariff)
            {
                case VM_ForecastTariff.Platinum:
                    return 8;
                case VM_ForecastTariff.Gold:
                    return 6;
                case VM_ForecastTariff.Silver:
                    return 4;
                case VM_ForecastTariff.Bronze:
                    return 2;
                default:
                    return 0;
            }
        }
    }
}