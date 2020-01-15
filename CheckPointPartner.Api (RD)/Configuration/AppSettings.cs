#region Using

using JetBrains.Annotations;

#endregion

namespace AbsenSupir.WebApp.Configuration
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public Jwt Jwt { get; set; }
        public Email Email { get; set; }
        public DatabaseServer Database { get; set; } = DatabaseServer.LocalDb;

    }
}
