using MongoDB.Bson;
using MongoDB.Driver;

public class Model_PartyRequest
{
    public ObjectId _id;

    public MongoDBRef Sender;
    public MongoDBRef Receiver;

    public MongoDBRef Party;
}