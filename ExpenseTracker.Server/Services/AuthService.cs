using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Models;
using ExpenseTracker.Server.Responses;
using ExpenseTracker.Server.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IValidator<LoginDto> _loginUserValidator;

        public AuthService(
            UserManager<User> userManager,
            ITokenService tokenService,
            IValidator<LoginDto> loginUserValidator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _loginUserValidator = loginUserValidator;
        }

        public async Task<CommonResponse> LoginAsync(LoginDto dto)
        {
            var preValidation = _loginUserValidator.Validate(dto);

            if (!preValidation.IsValid)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Login failed!",
                    Errors = preValidation.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Login failed!",
                    Errors = new List<string>()
                    {
                        "No user found under this username!"
                    }
                };
            }

            var loginSucceeded = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!loginSucceeded)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Login failed!",
                    Errors = new List<string>()
                    {
                        "Invalid password!"
                    }
                };
            }

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!isEmailConfirmed)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Login failed!",
                    Errors = new List<string>()
                    {
                        "Confirm your e-mail address first!"
                    }
                };
            }

            var tokens = await _tokenService.GenerateJwtTokenAsync(user);

            await _userManager.RemoveAuthenticationTokenAsync(user, "ExpenseTracker", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(user, "ExpenseTracker", "RefreshToken", tokens.RefreshToken);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Login successful!",
                ReceivedData = tokens
            };
        }

        public async Task<CommonResponse> RefreshAsync(string refreshToken, string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Token refresh failed!",
                    Errors = new List<string>()
                    {
                        "User was not found!"
                    }
                };
            }

            var IsTokenValid = await _userManager.VerifyUserTokenAsync(user, "ExpenseTracker", "RefreshToken", refreshToken);

            if (!IsTokenValid)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Token refresh failed!",
                    Errors = new List<string>()
                    {
                        "Refresh token was invalid!"
                    }
                };
            }

            var newTokens = await _tokenService.GenerateJwtTokenAsync(user);

            await _userManager.RemoveAuthenticationTokenAsync(user, "ExpenseTracker", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(user, "ExpenseTracker", "RefreshToken", newTokens.RefreshToken);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Token refresh was successful!",
                ReceivedData = newTokens
            };
        }
    }
}
