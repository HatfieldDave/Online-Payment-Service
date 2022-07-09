using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    class FinancialService
    {
        private const string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public List<API_User> GetAllOtherUsers()
        {
            RestRequest request = new RestRequest($"{API_BASE_URL}users");  //TODO rename to username if doesn't work
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse<List<API_User>> response = client.Get<List<API_User>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Request rejected by server.");
                return null;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("Problem getting other users: " + response.StatusDescription);
                Console.WriteLine(response.Content);
                return null;
            }
            return response.Data;
        }
        public List<Transfer> GetUsersTranactions()
        {
            RestRequest request = new RestRequest($"{API_BASE_URL}transfers");  //TODO rename to username if doesn't work
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Request rejected by server.");
                return null;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("Problem getting transfer list: " + response.StatusDescription);
                Console.WriteLine(response.Content);
                return null;
            }
            return response.Data;
        }

        public Transfer GetTransferById(int id)
        {
            RestRequest request = new RestRequest($"{API_BASE_URL}transfers/" + id);  //TODO rename to username if doesn't work
            request.AddHeader("Authorization", "Bearer " + token);
            
            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Please recheck transfer number.");
                return null;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("Invalid Transfer request. Denied. ");
                //Console.WriteLine(response.Content);  //TODO change or remove
                return null;
            }
            return response.Data; //TODO replace when works 
        }

        public bool TransferTEBucks(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{API_BASE_URL}transfers");  //TODO rename to username if doesn't work
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddJsonBody(transfer);
            IRestResponse<AccountBalance> response = client.Post<AccountBalance>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Insuffcient funds, please buy more TE Bucks!");
                return false;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("Invalid Transfer request. Denied. ");
                //Console.WriteLine(response.Content);  //TODO change or remove
                return false;
            }
            return true; //TODO replace when works

        }

        public AccountBalance GetBalance()
        {
            RestRequest request = new RestRequest($"{API_BASE_URL}balance");
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse<AccountBalance> response = client.Get<AccountBalance>(request);
           
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not connect to the server; Try again later!");
                return null;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("Problem getting balance: " + response.StatusDescription);
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
