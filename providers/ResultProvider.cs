using DealCloudTest.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DealCloudTest.providers
{
  public class ResultProvider
   {
      private HttpClient client = new HttpClient();
      private string apiKey;

      public ResultProvider(string apiKey)
      {
         this.apiKey = apiKey;
      }

      public async Task<float> GetAverageVolume(string symbol, int days)
      {
         HttpResponseMessage response = await client.GetAsync("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=" + symbol + "&apikey="+ this.apiKey);
         string contentString = await response.Content.ReadAsStringAsync();

         return CalculateAverage(contentString, days);
      }

      public async Task<float> GetHighestClosingPrice(string symbol, int months)
      {
         HttpResponseMessage response = await client.GetAsync("https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY&symbol=" + symbol + "&apikey=" + this.apiKey);
         string contentString = await response.Content.ReadAsStringAsync();

         return CalculateHighest(contentString, months);
      }

      private float CalculateAverage(string contentString, int days)
      {
         var parsed = JObject.Parse(contentString);
         var daily = parsed["Time Series (Daily)"];
         List<string> dateKeys = GetKeys(days);
         float total = 0;
         foreach (var key in dateKeys)
         {
            float val;
            if (daily[key]!= null)
            {
               if (float.TryParse(daily[key]["5. volume"].ToString(), out val))
               {
                  total += val;
               }
            }
         }

         return total > 0 ? total / days : 0;
      }

      private float CalculateHighest(string contentString, int months)
      {
         var parsed = JObject.Parse(contentString);
         var monthly = parsed["Monthly Time Series"];
         int counter = 0;
         float highest = 0;
         foreach (var x in (monthly as JObject))
         {
            float current;
            if(float.TryParse(x.Value["4. close"].ToString(), out current))
            {
               if (current > highest)
               {
                  highest = current;
               }
            }

            if (++counter == months)
            {
               break;
            }
         }

         return highest;
      }
      private List<string> GetKeys(int days)
      {


         List<string> dateKeys = new List<string>();
         int today = DateTime.Today.Day;

         for (int i = 0; i < days; i++)
         {
            dateKeys.Add(("2019-11-" + (today - i)));
         }

         return dateKeys;
      }

     // public float Average()
   }
}
