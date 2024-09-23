using System;
using frontend.Models;

namespace frontend.Clients
{
    public class UserClient(HttpClient httpClient)
    {
        public async Task RegisterUserAsync(User user) => await httpClient.PostAsJsonAsync("api/user/register", user);

        public async Task<LoginResponce> LoginUserAsync(LoginRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/User/login", request); // Corrected endpoint if needed

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponce>();

            return loginResponse ?? throw new InvalidOperationException("Login response is null.");
        }

    }

}

