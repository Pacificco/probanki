using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Debtors
{
    /// <summary>
    /// Модель списка должников для отображения на страницах сайта
    /// </summary>
    public class VM_Debtors
    {
        public VM_DebtorsFilter Filters { get; set; }
        public List<Debtor> Items { get; set; }
        public VM_PagingInfo PagingInfo { get; set; }

        public VM_Debtors()
        {
            Filters = new VM_DebtorsFilter();
            Items = new List<Debtor>();
            PagingInfo = new VM_PagingInfo();
        }
    }
}