using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.ViewModels
{
    /// <summary>
    /// Постраничная навигация
    /// </summary>
    public class VM_PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public VM_PagingInfo()
        {
            TotalItems = 0;
            ItemsPerPage = 20;
            CurrentPage = 1;
        }
        public void SetData(int curPage, int totalPages)
        {
            TotalItems = totalPages;
            CurrentPage = curPage;
        }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((double)TotalItems / (double)ItemsPerPage); }
        }
        /// <summary>
        /// Возвращает номер строки, с которой будет выбран список записей
        /// </summary>
        /// <returns>Начальная строка</returns>
        public int GetNumberFrom()
        {
            return ((CurrentPage - 1) * ItemsPerPage + 1);
        }
        /// <summary>
        /// Возвращает номер строки, по которую будет выбран список записей
        /// </summary>
        /// <returns>Кончная строка</returns>
        public int GetNumberTo()
        {
            return ((CurrentPage - 1) * ItemsPerPage + ItemsPerPage);
        }
        /// <summary>
        /// Возвращает номер по порядку первой записи на текущей странице
        /// </summary>
        /// <returns>Номер первой записи</returns>
        public int GetFirstNumInCurrentPage()
        {
            return CurrentPage * ItemsPerPage - (ItemsPerPage - 1);
        }
    }
}