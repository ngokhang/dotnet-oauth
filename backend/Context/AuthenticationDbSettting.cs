using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnOAuth.Context
{
  public class AuthenticationDbSettting
  {
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string UserCollectionName { get; set; } = string.Empty;
  }
}