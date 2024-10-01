using System;
using System.Net.Http.Headers;
using frontend.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace frontend.Clients;

public class OrganizerClient(HttpClient httpClient, ProtectedLocalStorage localStorage)
{
    private async Task SetAuthorizedHeader()
    {
        var token = (await localStorage.GetAsync<string>("authToken")).Value;

        if (token != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
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

    public async Task<User> GetOrganizerInfo()
    {
        await SetAuthorizedHeader();
        return await httpClient.GetFromJsonAsync<User>("api/Organiser/Organiser-Details") ?? throw new Exception("Could not find the user");
    }

    public async Task<HttpResponseMessage> AddEvent(MultipartFormDataContent content)
    {
        await SetAuthorizedHeader();
        var response = await httpClient.PostAsync("api/organiser/createEvent", content);
        return response;
    }
}
