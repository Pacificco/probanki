using Bankiru.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Club
{
    public class VM_AddUserToForecast
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>        
        [HiddenInput]
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор прогноза
        /// </summary>        
        [HiddenInput]
        public int ForecastId { get; set; }
        /// <summary>
        /// Значение ставки
        /// </summary>
        [Display(Name = "Ваш прогноз")]
        [Required(ErrorMessage = "Вы не указали значение прогноза!")]
        public string Value { get; set; }
        /// <summary>
        /// Сообщение об успешном принятии значения прогноза
        /// </summary>
        [Display(Name = "Сообщение об успешном принятии значения прогноза")]        
        public string SuccessMessage { get; set; }
    }
}