using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNRDnTAILibrary
{
    public static class VNRDnTAIConfiguration
    {
        #region Private Members to get Configuration
        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            return builder.Build();
        }
        #endregion

        #region Public Configuration Fields
        public static string ConnectionString
            => GetConfiguration()["ConnectionStrings:vnrdntaiDb"];

        public static string JwtIssuer
            => GetConfiguration()["Jwt:ValidIssuer"];
        public static string JwtAudience
            => GetConfiguration()["Jwt:ValidAudience"];
        public static string Secret
            => GetConfiguration()["Jwt:Secret"];
        #endregion
    }
}
