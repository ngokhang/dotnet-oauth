using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnOAuth.Context;
using LearnOAuth.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LearnOAuth.Services
{
  public class UserServices
  {
    private readonly IMongoCollection<User> _userCollection;
    public UserServices(IOptions<AuthenticationDbSettting> authenticationDbSetting)
    {
      var mongoClient = new MongoClient(authenticationDbSetting.Value.ConnectionString);
      var mongoDatabase = mongoClient.GetDatabase(authenticationDbSetting.Value.DatabaseName);
      _userCollection = mongoDatabase.GetCollection<User>(authenticationDbSetting.Value.UserCollectionName);
    }

    public async Task CreateNewUserAsync(User userData) => await _userCollection.InsertOneAsync(userData);

    public async Task<User> GetUserByGoogleId(string googleId)
    {
      var result = await _userCollection.Find(user => user.googleId == googleId).FirstOrDefaultAsync();
      if (result != null)
      {
        return result;
      }
      return null;
    }

    public async Task<User> GetUserByUsername(string _username)
    {
      var result = await _userCollection.Find(user => user.username == _username).FirstOrDefaultAsync();
      if (result != null)
      {
        return result;
      }
      return null;
    }
  }
}