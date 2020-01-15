#region Using

using JetBrains.Annotations;
using System;

#endregion

namespace AbsenSupir.WebApp.Configuration
{
    public class Jwt
    {
        public string Site { get; set; }
        public string SigningKey { get; set; }
        public Int32 ExpiryInMinutes { get; set; }
    }
}
