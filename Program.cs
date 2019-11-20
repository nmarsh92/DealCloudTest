using DealCloudTest.providers;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DealCloudTest
{
   class Program
   {
      static void Main(string[] args)
      {
         ResultProvider provider = new ResultProvider("<API_KEY_HERE>");
         var average =  provider.GetAverageVolume("MSFT", 7).GetAwaiter().GetResult();
         var highest = provider.GetHighestClosingPrice("AAPL", 6).GetAwaiter().GetResult();
         Console.WriteLine("MSFT Average(Last 7 Days): " + average);
         Console.WriteLine("AAPL Highest(Last 6 Months): " + highest);
         Console.Read();
      }
   }
}
