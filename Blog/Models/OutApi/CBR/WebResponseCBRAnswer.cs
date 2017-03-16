using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bankiru.Models.OutApi.CBR
{
    /// <summary>
    /// Класс ответа на запрос
    /// </summary>
    public class WebResponseCBRAnswer
    {
        private int? _httpCode;
        private string _errorMessage;
        private Stream _stream;

        public int? HttpCode
        { get { return _httpCode; } }

        public string ErrorMessage
        { get { return _errorMessage; } }

        public Stream Stream
        { get { return _stream; } }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public WebResponseCBRAnswer(int? httpCode, string errorMessage, Stream stream)
        {
            _httpCode = httpCode;
            _errorMessage = errorMessage;
            _stream = stream;
        }
    }
}
