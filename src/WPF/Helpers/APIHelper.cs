using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TouchTest.WPF.Models;

namespace TouchTest.WPF.Helpers
{
    public class APIHelper : IAPIHelper
    {
        string _bearerAuth;
        public HttpClient APIClient { get; set; }

        public string BearerAuth
        {
            get => _bearerAuth;
            set
            {
                _bearerAuth = value;
                InitClient();
            }
        }

        public APIHelper()
        {
            InitClient();
        }

        /// <summary>
        /// add the api url and the json heafer for the request
        /// </summary>
        private void InitClient()
        {
            APIClient = new HttpClient
            {
                BaseAddress = new Uri(Properties.Settings.Default.api)
            };
            APIClient.DefaultRequestHeaders.Accept.Clear();
            APIClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(BearerAuth))
            {
                APIClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {BearerAuth}");
            }
        }

        /// <summary>
        /// check if the user exist and its info is correct
        /// </summary>
        /// <param name="user">the user name</param>
        /// <param name="pass">the user password</param>
        /// <returns>the request status</returns>
        public async Task<AuthenticatedUser> AuthAsync(string user, string pass)
        {
            var date = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", user),
                new KeyValuePair<string, string>("password", pass),
            });

            using (HttpResponseMessage response = await APIClient.PostAsync("Token", date))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    BearerAuth = result.AccessToken;
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
