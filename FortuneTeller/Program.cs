using System;
using System.Diagnostics.Metrics;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Xml;

namespace TwoSentenceFortuneTeller
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to your own fortune teller");

            Console.WriteLine("Enter your full name please: ");

            string name = Console.ReadLine().Trim();

            int nameLength = name.Length;


            Console.WriteLine("In order to find your fortunes for both the Western and Chinese zodiacs,\nEnter your birthday (MM/DD/YYYY)");
            string birthDate = Console.ReadLine();

            string[] birthDateSeparated = birthDate.Split("/");

            int month = 0, day = 0, year = 0;

            bool canParseMonth = birthDateSeparated.Length > 0 ? int.TryParse(birthDateSeparated[0], out month) : false;
            bool canParseDay = birthDateSeparated.Length > 1 ? int.TryParse(birthDateSeparated[1], out day) : false;
            bool canParseYear = birthDateSeparated.Length > 2 ? int.TryParse(birthDateSeparated[2], out year) : false;


            while (birthDateSeparated.Length != 3 ||
                    !canParseMonth ||
                    !canParseDay ||
                    !canParseYear)
            {
                Console.WriteLine("Please enter dates in this format as numbers MM/DD/YYYY");
                birthDate = Console.ReadLine();
                birthDateSeparated = birthDate.Split("/");

                canParseMonth = birthDateSeparated.Length > 0 ? int.TryParse(birthDateSeparated[0], out month) : false;
                canParseDay = birthDateSeparated.Length > 1 ? int.TryParse(birthDateSeparated[1], out day) : false;
                canParseYear = birthDateSeparated.Length > 2 ? int.TryParse(birthDateSeparated[2], out year) : false;
            }

            //int month = int.Parse(birthDateSeparated[0]);
            //int day = int.Parse(birthDateSeparated[1]);
            //int year = int.Parse(birthDateSeparated[2]);



            // logic starting from userInput

            if (nameLength > 10)
            {
                Console.WriteLine($"Change has been coming your way for a while now and today you may see the first explicit features of it and ");

            }
            if (name.Length < 10)
            {
                Console.WriteLine("Life as you have always known it is about to change and");

            }

            string zodiac = Zodiac(day, month);
            string westernHoroscope = await GetWesternHoroscopeAsync(zodiac);
            Console.WriteLine($"as your zodiac is {zodiac}: {westernHoroscope}");
            Console.WriteLine("-----");

            string zodiacAnimal = ChineseZodiac(year);
            string chineseHoroscope = await GetChineseHoroscopeAsync(zodiacAnimal);
            Console.WriteLine($"as a {zodiacAnimal}: {chineseHoroscope}");

            //Console.WriteLine(ChineseZodiac(year));
            Console.WriteLine("-----");


        }

        static string ChineseZodiac(int year)
        {
            string[] chineseZodiac = new string[] {"Dragon", "Snake", "Horse", "Sheep",
                                 "Monkey", "Rooster", "Dog", "Pig",
                                  "Rat", "Ox", "Tiger", "Rabbit", "Dragon"};
            if (year > 1999)
            {
                year = ((year - 2000) % 12);
            }
            else
            {
                year = ((year - 2000) % 12);
                year = 12 + year;
            }
            return chineseZodiac[year];
        }

        public static async Task<string> GetChineseHoroscopeAsync(string zodiacAnimal)
        {
            string url = $"https://www.astrology.com/horoscope/daily-chinese/{zodiacAnimal.ToLower()}.html";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("We're sorry, we cannot find your horoscope for the chinese zodiac... Please try again later....");
                    }
                    else
                    {
                        Console.WriteLine($"HTTP Error: {response.StatusCode.ToString()}");
                    }
                    return null;
                }


                string html = await response.Content.ReadAsStringAsync();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                HtmlNode contentNode = doc.DocumentNode.QuerySelector("#content");

                if (contentNode != null)
                {
                    return contentNode.InnerText;
                }
                return null;
            }
        }
        static string Zodiac(int day, int month)
        {
            if (month == 1)
            {
                if (day <= 19) { return "Capricorn"; }
                if (day > 19) { return "Aquarius"; }
            }
            else if (month == 2)
            {
                if (day <= 18) { return "Aquarius"; }
                if (day > 18) { return "Pisces"; }
            }
            else if (month == 3)
            {
                if (day <= 20) { return "Pisces"; }
                if (day > 20) { return "Aries"; }
            }
            else if (month == 4)
            {
                if (day <= 19) { return "Aries"; }
                if (day > 19) { return "Taurus"; }
            }
            else if (month == 5)
            {
                if (day <= 20) { return "Taurus"; }
                if (day > 20) { return "Gemini"; }
            }
            else if (month == 6)
            {
                if (day <= 21) { return "Gemini"; }
                if (day > 21) { return "Cancer"; }
            }
            else if (month == 7)
            {
                if (day <= 22) { return "Cancer"; }
                if (day > 22) { return "Leo"; }
            }
            else if (month == 8)
            {
                if (day <= 22) { return "Leo"; }
                if (day > 22) { return "Virgo"; }
            }
            else if (month == 9)
            {
                if (day <= 22) { return "Virgo"; }
                if (day > 22) { return "Libra"; }
            }
            else if (month == 10)
            {
                if (day <= 23) { return "Libra"; }
                if (day > 23) { return "Scorpio"; }
            }
            else if (month == 11)
            {
                if (day <= 21) { return "Scorpio"; }
                if (day > 21) { return "Sagittarius"; }
            }
            else if (month == 12)
            {
                if (day <= 21) { return "Sagittarius"; }
                if (day > 21) { return "Capricorn"; }
            }
            return "Ophiuchus";
        }

        public static async Task<string> GetWesternHoroscopeAsync(string zodiac)
        {

            string url = $"https://www.astrology.com/horoscope/daily/{zodiac.ToLower()}.html";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                // Check for successful HTTP response
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("We're sorry we could not find your horoscope for your zodiac at this time... Please try again later");
                    }
                    else
                    {
                        Console.WriteLine($"HTTP Error: {response.StatusCode.ToString()}");
                    }
                    return null;
                }

                string html = await response.Content.ReadAsStringAsync();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                HtmlNode contentNode = doc.DocumentNode.QuerySelector("#content");

                if (contentNode != null)
                {
                    return contentNode.InnerText;
                }
                return null;
            }

        }
    }
}
