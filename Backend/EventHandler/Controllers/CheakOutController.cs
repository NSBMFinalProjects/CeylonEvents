using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventHandler.Dto;
using Stripe.Checkout;
using EventHandler.Models.Entities;
using Microsoft.EntityFrameworkCore;
using EventHandler.Data;
using System.Security.Claims;


namespace EventHandler.Controllers
{
    [Route("Checkout")]
    [ApiController]
    public class CheakOutController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EventDbContext _context;

        private static string s_wasmClientURL = string.Empty;

        public CheakOutController(IConfiguration configuration,EventDbContext eventDbContext)
        {
            _configuration = configuration;
            _context = eventDbContext;
        }

        [HttpPost]
        public async Task<ActionResult> CheakoutOrder([FromBody] CheckoutDto cheakoutDto, [FromServices] IServiceProvider sp)
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
                var sessionId = await Cheakout(cheakoutDto, thisApiUrl);
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
        public async Task<string> Cheakout(CheckoutDto cheakOutDto, string thisApiUrl)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            HttpContext.Session.SetString("UserID", cheakOutDto.userID);
            HttpContext.Session.SetInt32("TicketId", cheakOutDto.TiketId);
            HttpContext.Session.SetInt32("Quantity", cheakOutDto.Quantity);
            HttpContext.Session.SetInt32("EventId", cheakOutDto.EventId);

            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{thisApiUrl}/Checkout/success?sessionId={{CHECKOUT_SESSION_ID}}", // Customer paid.
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
                            UnitAmount = cheakOutDto.TicketPrice, // Price is in USD cents.
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = cheakOutDto.EventName,
                                Description = cheakOutDto.description,
                            },
                        },
                        Quantity = cheakOutDto.Quantity,
                    },
                },
                Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Id;

        }

        [HttpGet("success")]
        public async Task<IActionResult> CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            if (session == null || session.PaymentStatus != "paid")
            {
                return BadRequest("Invalid session ID or payment not completed.");
            }

            var userID = HttpContext.Session.GetString("UserID");
            var ticketId = HttpContext.Session.GetInt32("TicketId");
            var quantity = HttpContext.Session.GetInt32("Quantity") ?? 0;
            var eventID = HttpContext.Session.GetInt32("EventId");

            
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                return BadRequest("Ticket not found.");
            }

            if (ticket.Quantity < quantity)
            {
                return BadRequest("Not enough tickets available.");
            }

            ticket.Quantity -= quantity; 

            
            var purchase = new Purchase
            {
                UserId = userID,
                TicketId = ticket.Id,
                Quantity = quantity,
                PurchaseDate = DateTime.UtcNow,
                Ticket = ticket,
                EventId = eventID
                
            };

           
            _context.Tickets.Update(ticket);
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            // Optional: Log or send a confirmation email here

            return Redirect(s_wasmClientURL.TrimEnd('/') + "/success"); 
        }






    }
}
