using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Helpers
{
    public static class UserTariffHelper
    {
        public static int GetEnabledForecastsCountForMonthByTariff(EnumForecastTariff tariff)
        {
            switch (tariff)
            {
                case EnumForecastTariff.Platinum:
                    return 8;
                case EnumForecastTariff.Gold:
                    return 6;
                case EnumForecastTariff.Silver:
                    return 4;
                case EnumForecastTariff.Bronze:
                    return 2;
                default:
                    return 0;
            }
        }
        public static string GetTariffName(EnumForecastTariff tariff)
        {
            switch (tariff)
            {
                case EnumForecastTariff.Platinum:
                    return "Платинум";
                case EnumForecastTariff.Gold:
                    return "Золото";
                case EnumForecastTariff.Silver:
                    return "Серебро";
                case EnumForecastTariff.Bronze:
                    return "Бронза";
                default:
                    return "?";
            }
        }
    }
}