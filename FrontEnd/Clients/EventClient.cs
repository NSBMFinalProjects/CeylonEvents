using System;
using System.Net.Http.Headers;
using frontend.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;


namespace frontend.Clients;

public class EventClient(HttpClient httpClient, ProtectedLocalStorage localStorage)
{
    public async Task SetAuthorizedHeader()
    {
        var token = (await localStorage.GetAsync<string>("authToken")).Value;

        if (token != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
    public async Task<EventResponce> GetEventInfo(int eventId)
    {
        await SetAuthorizedHeader();
        return await httpClient.GetFromJsonAsync<EventResponce>($"api/Event/{eventId}") ?? throw new Exception("Could not find the user");
    }

    public async Task<List<EventResponce>> GetAllEvents()
    {
        await SetAuthorizedHeader();
        return await httpClient.GetFromJsonAsync<List<EventResponce>>($"api/Event/all") ?? throw new Exception("Could not find the user");
    }
}
