using System;
using System.Net.Http.Headers;
using frontend.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace frontend.Clients;

public class CheckoutClient(HttpClient httpClient, ProtectedLocalStorage localStorage)
{
    public async Task SetAuthorizedHeader()
    {
        var token = (await localStorage.GetAsync<string>("authToken")).Value;

        if (token != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<CheckoutResponce> GetEventInfo(CheckoutRequest request)
    {
        await SetAuthorizedHeader();
        return await httpClient.GetFromJsonAsync<CheckoutResponce>($"Checkout") ?? throw new Exception("Could not find the user");
    }
}
