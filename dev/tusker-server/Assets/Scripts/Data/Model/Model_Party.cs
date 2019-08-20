using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

public class Model_Party
{
    public ObjectId _id;

    public MongoDBRef Creator;
    public List<MongoDBRef> Members = new List<MongoDBRef>();

    public string Token;

    public byte Status;

    public byte GameMode;
    public byte GameType;
    public byte GameWay;

    public DateTime Since;
    public DateTime Until;

    public Party GetParty()
    {
        Party party = new Party
        {
            Token = Token,
            Status = Status,
            GameMode = GameMode,
            GameType = GameType,
            GameWay = GameWay,
            MemberCount = (byte)(Members.Count + 1)
        };
        return party;
    }
}