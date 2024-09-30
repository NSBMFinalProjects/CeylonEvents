using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventHandler.Dto;
using Stripe.Checkout;

namespace EventHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheakOutController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private static string s_wasmClientURL = string.Empty;

        public CheakOutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> CheakoutNftOrder([FromBody] CheakOutDto cheakOutDto, [FromServices] IServiceProvider sp)
        {
            var referer = Request.Headers.Referer;
            s_wasmClientURL = referer[0];

            var server = sp.GetRequiredService<IServer>();

            var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

            string? thisApiUrl = null;

            if (serverAddressesFeature is not null)
            {
                thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();
            }


            if (thisApiUrl is not null)
            {
                var sessionId = await CheakOut(cheakOutDto, thisApiUrl);
                var pubKey = _configuration["Stripe:PubKey"];

                var checkoutOrderResponse = new CheckOutOrderResponse()
                {
                    SessionId = sessionId,
                    PubKey = pubKey
                };

                return Ok(checkoutOrderResponse);
            }
            else
            {
                return StatusCode(500);
            }

        }

        [NonAction]
        public async Task<string> CheakOut(CheakOutDto cheakOutDto, string thisApiUrl)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{thisApiUrl}/checkout/success?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                CancelUrl = s_wasmClientURL + "failed",  // Checkout cancelled.
                PaymentMethodTypes = new List<string> // Only card available in test mode?
                {
                    "card"
                },

                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = cheakOutDto.FinalPrice, // Price is in USD cents.
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = cheakOutDto.EventName,
                                Description = cheakOutDto.TicketName
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Id;

        }


    }
}
