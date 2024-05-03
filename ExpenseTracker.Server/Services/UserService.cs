using AutoMapper;
using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Models;
using ExpenseTracker.Server.Responses;
using ExpenseTracker.Server.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ExpenseTracker.Server.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(
            UserManager<User> userManager,
            IMapper mapper,
            IValidator<CreateUserDto> createUserValidator,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _createUserValidator = createUserValidator;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<CommonResponse> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = new List<UserResponse>();
            foreach (var user in users)
            {
                result.Add(_mapper.Map<UserResponse>(user));
            }

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Search successful!",
                ReceivedData = result
            };
        }

        public async Task<CommonResponse> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Search failed!",
                    Errors = new List<string>()
                    {
                        "User was not found!"
                    }
                };
            }

            var userResponse = _mapper.Map<UserResponse>(user);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Search successful!",
                ReceivedData = userResponse
            };
        }

        public async Task<CommonResponse> CreateAsync(CreateUserDto dto)
        {
            var preValidation = _createUserValidator.Validate(dto);

            if (!preValidation.IsValid)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "User registration failed!",
                    Errors = preValidation.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var user = _mapper.Map<User>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "User registration failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            string url = $"{_configuration["AppUrl"]}/api/User/ConfirmEmail?" +
                $"id={user.Id}" +
                $"&confirmationToken={validEmailToken}";

            await _emailService.SendConfirmationEmailAsync(user.Email!, url);

            var userResponse = _mapper.Map<UserResponse>(user);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "User registration successful!",
                ReceivedData = userResponse
            };
        }

        public async Task<CommonResponse> ConfirmEmailAsync(string id, string confirmationToken)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "E-mail confirmation failed!",
                    Errors = new List<string>()
                    {
                        "User was not found!"
                    }
                };
            }

            var decodedConfirmationToken = WebEncoders.Base64UrlDecode(confirmationToken);
            var normalConfirmationToken = Encoding.UTF8.GetString(decodedConfirmationToken);

            var result = await _userManager.ConfirmEmailAsync(user, normalConfirmationToken);

            if (!result.Succeeded)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "E-mail confirmation failed!",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "E-mail confirmation successful!",
                ReceivedData = user.Email
            };
        }

        public async Task<CommonResponse> UpdateAsync(UpdateUserDto dto, string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "User update failed!",
                    Errors = new List<string>()
                    {
                        "User was not found!"
                    }
                };
            }

            user = _mapper.Map(dto, user);

            await _userManager.UpdateAsync(user);

            var userResponse = _mapper.Map<UserResponse>(user);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "User update successful!",
                ReceivedData = userResponse
            };
        }

        public async Task<CommonResponse> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "User delete failed!",
                    Errors = new List<string>()
                    {
                        "User was not found!"
                    }
                };
            }

            await _userManager.DeleteAsync(user);

            var userResponse = _mapper.Map<UserResponse>(user);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "User successfully deleted!",
                ReceivedData = userResponse
            };
        }
    }
}
