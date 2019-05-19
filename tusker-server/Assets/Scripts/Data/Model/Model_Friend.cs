using MongoDB.Bson;
using MongoDB.Driver;
using System;

public class Model_Friend
{
    public ObjectId _id;

    public MongoDBRef Sender;
    public MongoDBRef Receiver;

    public DateTime Since;
}
