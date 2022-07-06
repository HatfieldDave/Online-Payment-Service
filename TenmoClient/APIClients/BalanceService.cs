using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    class BalanceService
    {
        private const string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public AccountBalance GetBalance(int user_id)
        {
            RestRequest request = new RestRequest($"{API_BASE_URL}balance/{user_id}");
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse<AccountBalance> response = client.Get<AccountBalance>(request);
           
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not connect to the dad-a-base; Try again later!");
                return null;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("Problem getting joke: " + response.StatusDescription);
                Console.WriteLine(response.Content);
                return null;
            }
            return response.Data;
        }
        public bool HasAuthToken
        {
            get
            {
                return token != null; // TODO: Return true if one is present
            }
        }
        private string token;
        public void UpdateToken(string jwt)
        {
            token = jwt;
            // Any request with this client in the future will AUTOMATICALLY
            // contain the Authorization / Bearer token header
            if (jwt == null)
            {
                client.Authenticator = null;
            }
            else
            {
                client.Authenticator = new JwtAuthenticator(jwt);
            }
        }
    }
}
