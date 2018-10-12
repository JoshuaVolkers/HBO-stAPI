using PoohAPI.Infrastructure.MapAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PoohAPI.Infrastructure.MapAPI.APIClients
{
    public class AzureMapsAPIClient : IMapAPIClient
    {
        private const string baseURL = "https://atlas.microsoft.com";
        private const string addressPath = "/search/address/json?";
        private string apiKey;

        public AzureMapsAPIClient()
        {
            this.apiKey = "";
        }

        private MapCoordinates RunAddressRequest(string query)
        {
            string requestUrl = baseURL + addressPath + "subscription-key=" + this.apiKey + "&api-version=1.0&query=\"" + query + "\"";

            HttpClient httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(baseURL + addressPath);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
            MapCoordinates mapCoordinates = new MapCoordinates();

            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;

                AzureMapsAddressResult result = JsonConvert.DeserializeObject<AzureMapsAddressResult>(jsonData);
                //List<AddressResult> result = JsonConvert.DeserializeObject<List<AddressResult>>(jsonData);

                if (result.results.Count > 0)
                {
                    mapCoordinates.Latitude = result.results[0].position.lat;
                    mapCoordinates.Longitude = result.results[0].position.lon;
                    mapCoordinates.ResponseSucceeded = true;
                }
                else
                {
                    mapCoordinates.ResponseSucceeded = false;
                }
            }
            else
            {
                mapCoordinates.ResponseSucceeded = false;
            }

            return mapCoordinates;
        }

        public MapCoordinates GetCoordinatesLocation(string cityName, string municipalityName, string countryName)
        {
            return this.RunAddressRequest(cityName + " " + municipalityName + " " + countryName);
        }

        public MapCoordinates GetCoordinatesLocation(string cityName, string municipalityOrCountryName)
        {
            return this.RunAddressRequest(cityName + " " + municipalityOrCountryName);
        }

        public MapCoordinates GetCoordinatesLocation(string cityName)
        {
            return this.RunAddressRequest(cityName);
        }
    }
}
