using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnOAuth.Models
{
  public class UserProfile
  {
    public string sub { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string given_name { get; set; } = string.Empty;
    public string family_name { get; set; } = string.Empty;
    public string picture { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public bool email_verified { get; set; }
    public string locale { get; set; } = string.Empty;
  }
}