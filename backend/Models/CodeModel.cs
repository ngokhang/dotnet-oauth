using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LearnOAuth.Models
{
  public class CodeModel
  {
    public string authuser { set; get; } = string.Empty;
    public string code { set; get; } = string.Empty;
    public string prompt { set; get; } = string.Empty;
    public string scope { set; get; } = string.Empty;
  }
}