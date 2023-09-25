using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.DTOs
{
  public class APIResponseDTO<T>
  {
    public int? ErrorCode { get; set; } = 200;
    public string Message { get; set; }
  }
}