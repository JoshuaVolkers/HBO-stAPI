using PoohAPI.Infrastructure.MapAPI.Models;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace PoohAPI.Infrastructure.MapAPI.APIClients
{
    public class AzureMapsAPIClient : IMapAPIClient
    {
        private const string baseURL = "https://atlas.microsoft.com";
        private const string addressPath = "/search/address/json?";
        private string apiKey;
        private readonly IConfiguration config;

        public AzureMapsAPIClient(IConfiguration config)
        {
            this.config = config;
            this.apiKey = config.GetValue<string>("AzureMapsAPIKey");
        }

        private MapCoordinates RunAddressRequest(string query)
        {
            string requestUrl = baseURL + addressPath + "subscription-key=" + this.apiKey + "&api-version=1.0&query=\"" + query + "\"";
            
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            httpWebRequest.Accept = "application/json";
            
            MapCoordinates mapCoordinates = new MapCoordinates();
            mapCoordinates.ResponseSucceeded = false;
            HttpWebResponse response;
            
            try {
                response = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch(Exception e) {
                return mapCoordinates;
            }
            
            if ((int)response.StatusCode == 200)
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string jsonData = reader.ReadToEnd();

                AzureMapsAddressResult result = JsonConvert.DeserializeObject<AzureMapsAddressResult>(jsonData);

                if (result.results.Count > 0)
                {
                    mapCoordinates.Latitude = result.results[0].position.lat;
                    mapCoordinates.Longitude = result.results[0].position.lon;
                    mapCoordinates.ResponseSucceeded = true;
                }
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
