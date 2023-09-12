using Common.DTOs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services.Algorithm
{
    public class CalculateDistancesByGoogleMaps
    {
        public async static Task<string> distByGoogleMaps(StoreDTO store1, StoreDTO store2)
        {
            var apiKey = "AIzaSyAd-0M-alwppdecG0L2WQMS97hXbRNRTjA";
            var origin = store1.StreatName;
            var destination = store2.StreatName;

            using var httpClient = new HttpClient();
            var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={origin}&destination={destination}&mode=walking&key={apiKey}";
            var response = await httpClient.GetAsync(url);
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var json = JObject.Parse(responseContent);
            var distance = json["routes"][0]["legs"][0]["distance"]["text"].ToString();
            return distance;
        }
        //פונקציה שמכניסה את הבדיקה מגוגל מפס לטבלה כדי למנוע גישות
        //API
        //מיותרות בפעמים הבאות
        public static async Task<double> InsertDistToTabale(StoreDTO s1, StoreDTO s2)
        {
            string distWithW;
            string km;
            double distance;
            distWithW = await distByGoogleMaps(s1, s2);
            km = distWithW.Split(" ")[1];
            if (km == "km")
            {
                distance = double.Parse(distWithW.Substring(0, distWithW.IndexOf(' '))) * 1000;
            }
            else
                distance = (double.Parse(distWithW.Substring(0, distWithW.IndexOf(' '))));
            MainAlgorithm.distances[s1.StoreId, s2.StoreId] = distance;
            MainAlgorithm.distances[s2.StoreId, s1.StoreId] = distance;
            return distance;
        }
        public static async Task<double> CalculateDistance(StoreDTO s1, StoreDTO s2)
        {
            if (s1.StoreId == s2.StoreId)
                return 0;
            if (MainAlgorithm.distances[s1.StoreId, s2.StoreId] == 0)
                return await InsertDistToTabale(s1, s2);
            else
                return MainAlgorithm.distances[s1.StoreId, s2.StoreId];
        }
        //פונקציה שממירה את שם החנות לX Y

        public static async Task<(double, double)> GetCoordinatesAsync(string streetName)
        {
            using (var client = new HttpClient())
            {
                var apiKey = "AIzaSyAd-0M-alwppdecG0L2WQMS97hXbRNRTjA"; // Replace with your Google Maps API key
                var encodedStreetName = Uri.EscapeDataString(streetName);
                var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedStreetName}&key={apiKey}";
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(content);
                if (result["status"].ToString() == "OK" && result["results"].HasValues)
                {
                    var location = result["results"][0]["geometry"]["location"];
                    return (location["lat"].Value<double>(), location["lng"].Value<double>());
                }
                else
                {
                    throw new Exception("Failed to get coordinates for street name");
                }
            }
        }


    }
}
