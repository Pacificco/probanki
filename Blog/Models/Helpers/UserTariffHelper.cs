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
                    return "4 площадки";
                case EnumForecastTariff.Gold:
                    return "3 площадки";
                case EnumForecastTariff.Silver:
                    return "2 площадки";
                case EnumForecastTariff.Bronze:
                    return "1 площадка";
                default:
                    return "?";
            }
        }
        public static string GetTariffPeriodName(EnumClubTariffPeriod period)
        {
            switch (period)
            {
                case EnumClubTariffPeriod.Month:
                    return "1 месяц";
                case EnumClubTariffPeriod.Quarter:
                    return "3 месяца";
                case EnumClubTariffPeriod.Half:
                    return "6 месяцев";
                case EnumClubTariffPeriod.Year:
                    return "12 месяцев";
                default:
                    return "?";
            }
        }
        public static double CalcPaymentSum(EnumForecastTariff tariff, EnumClubTariffPeriod period)
        {
            int k = -1;
            switch(period)
            {
                case EnumClubTariffPeriod.Month:
                    k = 1;
                    break;
                    case EnumClubTariffPeriod.Quarter:
                    k = 3;
                    break;
                    case EnumClubTariffPeriod.Half:
                    k = 6;
                    break;
                    case EnumClubTariffPeriod.Year:
                    k = 12;
                    break;
            }

            if(k == -1)
                return 0.0F;

            switch(tariff)
            {
                case EnumForecastTariff.Platinum:
                    return k * 1199.0F;
                case EnumForecastTariff.Gold:
                    return k * 999.0F;
                case EnumForecastTariff.Silver:
                    return k * 699.0F;
                case EnumForecastTariff.Bronze:
                    return k * 499.0F;
                default:
                    return 0.0F;
            }
        }
    }
}