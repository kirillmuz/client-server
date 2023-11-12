using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI
{
    public class AuthOptions
    {
        /// <summary>
        /// Издатель токена
        /// </summary>
        public const string ISSUER = "WebAPI_App_Server";
        
        /// <summary>
        /// Потребитель токена
        /// </summary>
        public const string AUDIENCE = "WebAPI_App_Client";
        
        /// <summary>
        /// Ключ шифрования
        /// </summary>
        public const string KEY = "A3668733-08D8-4D31-9D30-2515F0C82EBF";
        
        /// <summary>
        /// Время жизни токена
        /// </summary>
        public const int LIFETIME = 1;


        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
