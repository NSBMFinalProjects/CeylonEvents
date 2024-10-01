using AutoMapper;
using EventHandler.Data;
using EventHandler.Dto;
using EventHandler.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly EventDbContext _context;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, EventDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;

        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _mapper.Map<AppUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Buyer");

            var response = new
            {
                FirstName = registerDto.FirstName,  
                LastName = registerDto.LastName,
                NIC = registerDto.NIC,
                ContactNo = registerDto.PhoneNumber,
                Email = registerDto.Email
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Message = "User not found with this email",
                });
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Message = "Invalid Password."
                });
            }
            var token = GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Message = "Login Success.",
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUserDetail()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId is null)
            {
                return BadRequest("user id is null");
            }

            var user = await _userManager.FindByIdAsync(currentUserId!);

            if (user is null)
            {
                return NotFound(new AuthResponseDto
                {
                    Message = "User not found"
                });
            }

            var eventId = await _context.Purchases

                .Where(p => p.UserId == currentUserId && p.EventId != null)
                .Select(p => p.EventId).FirstOrDefaultAsync();

            //user tikets gathe nathm
            if (eventId == null)
            {
                return Ok(new
                {
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        NIC = user.NIC,
                        PhoneNumber = user.PhoneNumber
                    },
                    Events = new List<EventDto>(), 
                    Message = "No tickets purchased yet."
                });
            }

            //user tiket gathnam
            var events = await _context.Events
                .Include(e => e.category)
                .Where(e => e.Id == eventId)
                .ToListAsync();

            var eventDtos = events.Select(eventEntity => new EventDto
            {

                EventId = eventEntity.Id,
                EventName = eventEntity.EventName,
                EventDate = eventEntity.StartDate.ToString("yyyy-MM-dd"),
                EventTime = eventEntity.StartDate.ToShortTimeString(),
                EventLocation = eventEntity.Location,
                EventTicketPrice = eventEntity.tickets?.FirstOrDefault()?.Price.ToString("c") ?? "no tickets",
                EventDescription = eventEntity.Description,
                EventCategory=eventEntity.category.Name,
                EventImage = eventEntity.Image != null
                            ? $"{Request.Scheme}://{Request.Host}/Uploads/{eventEntity.Image}"
                            : null,

            }).ToList();

            return Ok(new
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    NIC = user.NIC,
                    PhoneNumber = user.PhoneNumber
                },
                Events = eventDtos
            });
        }

        private string GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII
            .GetBytes(_configuration.GetSection("JWTSetting").GetSection("securityKey").Value!);

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims =
            [
                new (JwtRegisteredClaimNames.Email,user.Email??""),
                new (JwtRegisteredClaimNames.Name,user.FirstName??""),
                new (JwtRegisteredClaimNames.NameId,user.Id ??""),
                new (JwtRegisteredClaimNames.Aud,
                _configuration.GetSection("JWTSetting").GetSection("validAudience").Value!),
                new (JwtRegisteredClaimNames.Iss,_configuration.GetSection("JWTSetting").GetSection("validIssuer").Value!)
            ];


            foreach (var role in roles)

            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}