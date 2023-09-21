using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnOAuth.Models
{
  public class ResponseOAuthGoogle
  {
    public string access_token { set; get; } = string.Empty;
    public string refresh_token { set; get; } = string.Empty;
    public string scope { set; get; } = string.Empty;
    public string token_type { set; get; } = string.Empty;
    public string id_token { set; get; } = string.Empty;
  }
}