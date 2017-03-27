using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Club
{
    public class VM_ForecastCloseInfo
    {
        [HiddenInput]
        [Display(Name = "Идентификатор")]
        public int ForecastId { get; set; }
        [Display(Name = "Фактическое значение")]
        public double FactValue { get; set; }
        [Display(Name = "Дата следующего прогноза")]
        public DateTime NextForecastDate { get; set; }
    }
}