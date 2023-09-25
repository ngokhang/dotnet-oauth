using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
  public class BaseBusinessException : Exception
  {
    public int? StatusCode { get; set; } = null;
  }
}