using MongoDB.Bson;
using MongoDB.Driver;

public class Model_FriendRequest
{
    public ObjectId _id;

    public MongoDBRef Sender;
    public MongoDBRef Receiver;
}