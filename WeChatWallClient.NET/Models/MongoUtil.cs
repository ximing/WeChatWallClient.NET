using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatWallClient.NET.Models
{
    public class MongoUtil
    {
        private string connectionString;
        public MongoClient client;
        private MongoDatabase database;
        private MongoServer server;

        public MongoUtil()
        {
            connectionString = "mongodb://localhost";
            client  = new MongoClient(connectionString);
            server = client.GetServer();
            database = server.GetDatabase("testwechat");
        }

        public MongoCollection<Msg> getCollection(string collectionname)
        {
            var collection = database.GetCollection<Msg>(collectionname);
            return collection;
        }
    }
}