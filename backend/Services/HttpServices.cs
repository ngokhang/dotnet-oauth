using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using LearnOAuth.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnOAuth.Services
{
  public class HttpServices
  {

    public HttpServices()
    {
    }

    public async Task<ActionResult<UserProfile>> Get(string token)
    {
      using (var client = new HttpClient())
      {
        client.BaseAddress = new Uri("https://www.googleapis.com");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Authorization", token);

        try
        {
          HttpResponseMessage response = await client.GetAsync("/oauth2/v3/userinfo");

          var responseContent = await response.Content.ReadAsStringAsync();
          var result = JsonConvert.DeserializeObject<UserProfile>(responseContent);

          return result;
        }
        catch (System.Exception)
        {
          throw;
        }
      }
    }

    public async Task<ActionResult<ResponseOAuthGoogle>> Post(string authCode)
    {
      using (var client = new HttpClient())
      {
        client.BaseAddress = new Uri("https://accounts.google.com");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        var requestData = new
        {
          grant_type = "authorization_code",
          code = authCode,
          redirect_uri = "http://localhost:5173",
          client_id = "718783698594-upqe5q5q5dvicood4ve4jdt1qqqivaba.apps.googleusercontent.com",
          client_secret = "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"
        };
        string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
          HttpResponseMessage response = await client.PostAsync("/o/oauth2/token", content);
          if (response.IsSuccessStatusCode)
          {
            // Read and handle the response content (if needed)
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseOAuthGoogle>(responseContent);

            return result;
          }
          else
          {
            return null;
          }
        }
        catch (HttpRequestException ex)
        {
          return null;
        }

      }
    }

  }
}