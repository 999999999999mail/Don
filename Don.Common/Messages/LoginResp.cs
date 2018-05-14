using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    public class LoginResp : ResponseBase
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// TokenType
        /// </summary>
        public string TokenType { get; set; }
    }
}
