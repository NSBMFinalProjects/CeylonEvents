using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using frontend.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace frontend.Clients
{
    public class UserClient(HttpClient httpClient, ProtectedLocalStorage localStorage)
    {
        public async Task SetAuthorizedHeader()
        {
            var token = (await localStorage.GetAsync<string>("authToken")).Value;

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<string> GetCurrentUserType()
        {
            var role = (await localStorage.GetAsync<string>("role")).Value;
            return role ?? "Invalid role";
        }

        public async Task<bool> IsUserLogged()
        {
            var token = (await localStorage.GetAsync<string>("authToken")).Value;

            if (token != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task RegisterUserAsync(User user) => await httpClient.PostAsJsonAsync("api/user/register", user);

        public async Task<LoginResponce> LoginUserAsync(LoginRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/User/login", request);

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponce>();

            return loginResponse ?? throw new InvalidOperationException("Login response is null.");
        }

        public async Task<User> GetUserInfo()
        {
            await SetAuthorizedHeader();
            return await httpClient.GetFromJsonAsync<User>("api/user") ?? throw new Exception("Could not find the user");
        }
    }

}

