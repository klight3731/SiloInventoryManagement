using System.Configuration;
using UXCypherLib;

namespace SiloInventoryManagement.Model
{
    public static class ConnectionStringHelper
    {
        private static string _environment = ConfigurationManager.AppSettings["Environment"];
        private static string _entitiesKey = string.Format("{0}UnitedSugarsSAPEntities", _environment);
        private static string _dbPasswordKey = string.Format("{0}DB_PASSWORD", _environment);

        public static string GetConnectionString()
        {
            NCypher encryptionLibrary = new NCypher();
            string connectionString = ConfigurationManager.ConnectionStrings[string.Format("{0}DbConnection", _environment)].ConnectionString;
            string password = encryptionLibrary.Decrypt(ConfigurationManager.AppSettings[_dbPasswordKey]);
            connectionString = string.Format("{0};Password={1}", connectionString, password);

            return connectionString;
        }

        public static string Entities
        {
            get
            {
                return _entitiesKey;
            }
        }

        public static string PasswordKey
        {
            get
            {
                return _dbPasswordKey;
            }
        }

        public static string Environment
        {
            get
            {
                return _environment;
            }
        }
    }
}
