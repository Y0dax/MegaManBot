using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Services.Common
{
    public class Database
    {
        public IMongoClient _client;
        public IMongoDatabase _database;

        public Database()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("MegamanDB");
        }
        ////private readonly DbContextOptions options;

        //private string connectionString { get; }

        //static Database() { }

        //private Database()
        //{
        //    connectionString = NadekoBot.Credentials.Db.ConnectionString;
        //    var optionsBuilder = new DbContextOptionsBuilder();
        //    optionsBuilder.UseSqlite(NadekoBot.Credentials.Db.ConnectionString);
        //    options = optionsBuilder.Options;


        //switch (NadekoBot.Credentials.Db.Type.ToUpperInvariant())
        //{
        //    case "SQLITE":
        //        dbType = typeof(NadekoSqliteContext);
        //        break;
        //    //case "SQLSERVER":
        //    //    dbType = typeof(NadekoSqlServerContext);
        //    //    break;
        //    default:
        //        break;

        //}
        //    }

    }
}
