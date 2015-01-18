using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatWallClient.NET.Models
{
    public class Msg
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int messageid { get; set; }
        public string fakeid { get; set; }
        public string nick_name { get; set; }
        public string content { get; set; }
        public long date_time { get; set; }

    }
}