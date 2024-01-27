using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace NWI.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {

        public readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IMapper mapper, DataContext context, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(UserLoginDto userRequest)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userRequest.Username);
                if (user is null)
                    throw new Exception($"User with username '{userRequest.Username}' is not found.");
                if (!VerifyPassword(userRequest.Password, user.PasswordHash, user.PasswordSalt))
                    throw new Exception("Wrong password.");

                serviceResponse.Data = CreateToken(user);

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }


            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> Register(UserRequestDto userRequest)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                Role userRole;
                userRequest.Role = userRequest.Role.ToLower();
                if (!Enum.TryParse<Role>(userRequest.Role, out userRole))
                    throw new Exception($"Role should only be Admin or User.");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userRequest.Username);
                if (user is not null){
                    serviceResponse.Data = "username error";
                    throw new Exception("Username is already used.");
                }
                    

                CreatePasswordHash(userRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);
                User newUser = new()
                {
                    Username = userRequest.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = userRole,
                    RefreshToken = ""
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                serviceResponse.Data = "";
                serviceResponse.Message = $"Successfully created user {userRequest.Username}";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }


            return serviceResponse;

        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, nameof(user.Role))
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public async Task<ServiceResponse<string>> GenerateJwtToken(string username, string refreshToken)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if(user is null)
                    throw new Exception("User not be found");
                if(!user.RefreshToken.Equals(refreshToken))
                    throw new Exception("Invalid Refresh Token");
                if(user.TokenExpires < DateTime.Now)
                    throw new Exception("Token expired.");
                
                serviceResponse.Data = CreateToken(user);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public RefreshToken GenerateRefreshToken(string userName)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7)
            };
            UpdateUserRefreshToken(refreshToken, userName);
            return refreshToken;
        }
        
        private void UpdateUserRefreshToken(RefreshToken newRefreshToken, string userName)
        {
            _context.Users
                .Where(u => u.Username == userName)
                .ExecuteUpdate(setters => setters
                .SetProperty(u => u.TokenCreated, newRefreshToken.Created)
                .SetProperty(u => u.RefreshToken, newRefreshToken.Token)
                .SetProperty(u => u.TokenExpires, newRefreshToken.Expires)
                );
            _context.SaveChanges();
        }

    }
}