
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AngularAuthenApi.Helper;
using AngularAuthenApi.Models;
using AngularAuthenApi.Models.Dto;
using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace AngularAuthenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly ComplaintContext _authDbcontext;
        private readonly IMapper _mapper;


        private readonly IComplaintRepository _complainRepo;



        public AuthController(ComplaintContext appDbContext, IComplaintRepository complaintRepo, IMapper mapper)
        {
            _authDbcontext = appDbContext;
            _complainRepo = complaintRepo;
            _mapper = mapper;
          

        }


        /***********************Login******************/
        //8-making a login functionality


        //Login Authenticate*
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] loginDto userObj)
        {
            if (userObj == null)
                return BadRequest();

            //return 400 error NotFound
            var user = await _authDbcontext.UsersAuthentication.FirstOrDefaultAsync
                (x => x.UserName == userObj.UserName);


            if (user == null)
                return NotFound(new { Message = "User Not Found!" });

            // Use this if statment after hashed the password(second step).                        
            if (!PasswordHasher.VerifyPassword(userObj.password, user.password))
            {
                return BadRequest(new { Message = "Password is Incorrect!" });
            }

            user.Token = CreateJwt(user);  //  TOKEN created when user Success to login

            var newAccessToken = user.Token;
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpireTime = DateTime.Now.AddDays(5); // Refresh Token Time Limit
            await _authDbcontext.SaveChangesAsync();

            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            }

            );

        }




        //Signup Register*
        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterDto userObj)

        {
            StringBuilder sb = new StringBuilder();

            if (userObj == null)
                return BadRequest();  //return 400 error NotFound

            //Check username 
            if (await CheackUserNameExistAsync(userObj.UserName))
                return BadRequest(new { Message = "Sorry UserName Already Exist!" });

            //check Email 

            if (await CheackEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Sorry Email Already Exist!" });



            //check password strength

            var pass = CheckPasswordStrength(userObj.password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass.ToString() 
                });

            userObj.password = PasswordHasher.HashPassword(userObj.password); // Call HashPassword class in Helper Folder,
                                                                              // and Hashing the Password with send to the Database.(first Step)
            var complaintToUpdate = _mapper.Map<UserAuthentication>(userObj);
            complaintToUpdate.Role = "User";
            complaintToUpdate.Token = "";
            complaintToUpdate.RefreshToken = "";

 

            await _authDbcontext.UsersAuthentication.AddAsync(complaintToUpdate);
            await _authDbcontext.SaveChangesAsync();

            return Ok(new
            {
                Token = complaintToUpdate.Token,
                Message = "User Registered!"
            });
        }



        private Task<bool> CheackUserNameExistAsync(string userName)
            => _authDbcontext.UsersAuthentication.AnyAsync(x => x.UserName == userName);

        private Task<bool> CheackEmailExistAsync(string email)
          => _authDbcontext.UsersAuthentication.AnyAsync(x => x.Email == email);



        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            //check The passsword should be more than 8 char
            if (password.Length < 8)
                sb.Append("Minimum password length should be 8 " + Environment.NewLine);

            //check The passsword should be Alphanumeric
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
                sb.Append("Password should be Alphanumeric " + Environment.NewLine);

            // Password should be contain special chars      
            if (!Regex.IsMatch(password, "[-,~,`,!,@,\t,#,$,%,^,&,*,(,) ,+,=,{,},[,:,\\,/,;,\",',<,>,.,?,=,_]"))
                sb.Append("Password should contain special chars! " + Environment.NewLine);

            return sb.ToString();
        }





        //CREATE JSON WEB TOKEN(JWT) fIRST-STEP, THEN-> STEP TWO CONFIGURATION IN Program.cs -> builder.Services.AddAuthentication(...)
        private string CreateJwt(UserAuthentication user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecret......");
            // var fullName = user.FirstName + " " + user.LastName;
            var identity = new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Role, user.Role),
                 new Claim(ClaimTypes.Name, $"{user.UserName}")

            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(30), //you maybe need to Add config in program.cs ( ClockSkew )
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }



        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);


            var tokenInUser = _authDbcontext.UsersAuthentication
                .Any(a => a.RefreshToken == refreshToken);

            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }




        //get The principal Value like payload value from the token
        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("veryverysecret......");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;


            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid token ");

            return principal;
        }


    }

}





