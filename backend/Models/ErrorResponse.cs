using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
  public class ErrorResponse
  {
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; } = 0;
    public List<string> Errors { get; set; }
  }
}