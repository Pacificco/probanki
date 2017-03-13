using Bankiru.Models.Domain.Club;
using Bankiru.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Users
{
    public class VM_UserProfileInfo
    {
        #region ПОЛЯ и СВОЙСТВА
        public VM_User User { get; set; }
        public VM_UserTariffInfo TariffInfo { get; set; }
        public List<VM_ForecastUser> ForecastsForMonth { get; set; }
        public int ForecastTriesForMonth { get; set; }
        public List<VM_ForecastSubject> ForecastSubjects { get; set; }        
        #endregion

        public VM_UserProfileInfo()
        {
            User = new VM_User();
            TariffInfo = new VM_UserTariffInfo();
            ForecastsForMonth = new List<VM_ForecastUser>();
            ForecastSubjects = new List<VM_ForecastSubject>();
            Clear();
        }
        public void Clear()
        {
            User.Clear();
            TariffInfo.Clear();
            ForecastsForMonth.Clear();
            ForecastTriesForMonth = 0;
            ForecastSubjects.Clear();
        }

        public int GetUsedForecastsCountForMonth()
        {
            if (ForecastsForMonth.Count == 0)
                return 0;

            return (from f in ForecastsForMonth where f.User.Id == User.Id select f.Forecast.Id).Count();
        }
        public int GetEnabledForecastsCountForMonth()
        {
            if (TariffInfo.Tariff == EnumForecastTariff.Undefined)
                return 0;

            if (TariffInfo.TariffEndDate == null || ((DateTime)TariffInfo.TariffEndDate) < DateTime.Now)
                return 0;

            int tCount = UserTariffHelper.GetEnabledForecastsCountForMonthByTariff(TariffInfo.Tariff);
            int fCount = (from f in ForecastsForMonth where f.User.Id == User.Id select f.Forecast.Id).Count();

            return tCount - fCount;

            //if (ForecastsForMonth.Count >= 8)
            //    return tCount;
            //else if (ForecastsForMonth.Count >= 4)
            //    return tCount / 2;
            //else
            //    return 0;
        }
        public List<VM_ForecastUser> GetForecastsBySubject(int subjectId)
        {
            if (ForecastsForMonth.Count == 0)
                return null;
            List<VM_ForecastUser> result = (from f in ForecastsForMonth where f.Forecast.SubjectId == subjectId select f).ToList<VM_ForecastUser>();
            if (result != null && result.Count > 0)
                result.Sort((a,b) => a.Forecast.CreateDate.CompareTo(b.Forecast.CreateDate));
            return result;
        }
    }
}