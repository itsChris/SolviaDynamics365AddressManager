using System.Net.Http.Headers;
using System.Text;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using SolviaDynamics365AddressManager.Models;

namespace SolviaDynamics365AddressManager.Services
{
    public class DynamicsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string ApiBaseUrl;
        private readonly string ApiVersion;
        private readonly string tenantId;
        private readonly string clientId;
        private readonly string clientSecret;

        public DynamicsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;


            // Read values from appsettings.json
            tenantId = _configuration["EntraId:TenantId"];
            clientId = _configuration["EntraId:ClientId"];
            clientSecret = _configuration["EntraId:ClientSecret"];
            ApiBaseUrl = _configuration["DynamicsApi:ApiBaseUrl"];
            ApiVersion = _configuration["DynamicsApi:ApiVersion"];

        }

        private async Task<string> GetAccessToken()
        {
            var authority = $"https://login.microsoftonline.com/{tenantId}";

            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();

            var scopes = new string[] { $"{ApiBaseUrl}.default" };

            try
            {
                AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                return result.AccessToken;
            }
            catch (MsalServiceException ex)
            {
                // Handle authentication errors here
                throw new Exception($"Error acquiring token: {ex.Message}");
            }
        }


        private async Task<HttpClient> AddAuthorizationHeader()
        {
            var token = await GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            return _httpClient;
        }

        // List Accounts
        public async Task<List<Account>> GetAccountsAsync()
        {
            var client = await AddAuthorizationHeader();
            var response = await client.GetAsync($"{ApiBaseUrl}/api/data/v{ApiVersion}/accounts?$select=name,address1_city,accountid");

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var accountsResponse = JsonConvert.DeserializeObject<AccountResponse>(responseString);
                return accountsResponse.Value;
            }

            throw new Exception("Failed to load accounts");
        }


        // List Contacts
        public async Task<List<Contact>> GetContactsAsync()
        {
            var client = await AddAuthorizationHeader();
            var response = await client.GetAsync($"{ApiBaseUrl}/api/data/v{ApiVersion}/contacts?$select=fullname,emailaddress1,contactid");

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var contactsResponse = JsonConvert.DeserializeObject<ContactResponse>(responseString);
                return contactsResponse.Value;
            }

            throw new Exception("Failed to load contacts");
        }

        // Create Account
        public async Task CreateAccountAsync(string name)
        {
            var account = new { name = name };
            var accountJson = JsonConvert.SerializeObject(account);

            var client = await AddAuthorizationHeader();
            var response = await client.PostAsync($"{ApiBaseUrl}/api/data/v{ApiVersion}/accounts",
                new StringContent(accountJson, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to create account");
            }
        }

        // Create Contact
        public async Task CreateContactAsync(string fullName)
        {
            var contact = new { fullname = fullName };
            var contactJson = JsonConvert.SerializeObject(contact);

            var client = await AddAuthorizationHeader();
            var response = await client.PostAsync($"{ApiBaseUrl}/api/data/v{ApiVersion}/contacts",
                new StringContent(contactJson, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to create contact");
            }
        }

        // Update Account
        public async Task UpdateAccountAsync(Guid accountId, string name)
        {
            var updatedAccount = new { name = name };
            var accountJson = JsonConvert.SerializeObject(updatedAccount);

            var client = await AddAuthorizationHeader();
            var response = await client.PatchAsync($"{ApiBaseUrl}/api/data/v{ApiVersion}/accounts({accountId})",
                new StringContent(accountJson, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to update account");
            }
        }

        // Delete Account
        public async Task DeleteAccountAsync(Guid accountId)
        {
            var client = await AddAuthorizationHeader();
            var response = await client.DeleteAsync($"{ApiBaseUrl}/api/data/v{ApiVersion}/accounts({accountId})");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to delete account");
            }
        }
    }
}
