using AutoMapper;
using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Models;
using ExpenseTracker.Server.Repositories.Interfaces;
using ExpenseTracker.Server.Responses;
using ExpenseTracker.Server.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAccountDto> _createAccountValidator;
        private readonly IValidator<UpdateAccountDto> _updateAccountValidator;

        public AccountService(
            IAccountRepository accountRepository,
            IMapper mapper,
            UserManager<User> userManager,
            IValidator<CreateAccountDto> createAccountValidator,
            IValidator<UpdateAccountDto> updateAccountValidator,
            ICurrencyRepository currencyRepository)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _userManager = userManager;
            _createAccountValidator = createAccountValidator;
            _updateAccountValidator = updateAccountValidator;
            _currencyRepository = currencyRepository;
        }

        public async Task<CommonResponse> GetAllAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();

            var accountResponses = new List<AccountResponse>();
            foreach (var account in accounts)
            {
                accountResponses.Add(_mapper.Map<AccountResponse>(account));
            }

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Search successful!",
                ReceivedData = accountResponses
            };
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);

            if (account == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Search failed!",
                    Errors = new List<string>()
                    {
                        "Account was not found!"
                    }
                };
            }

            var accountResponse = _mapper.Map<AccountResponse>(account);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Search successful!",
                ReceivedData = accountResponse
            };
        }

        public async Task<CommonResponse> CreateAsync(CreateAccountDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Account creation failed!",
                    Errors = new List<string>()
                    {
                        "User was not found!"
                    }
                };
            }

            var currency = await _currencyRepository.GetByIdAsync(dto.CurrencyId);

            if (currency == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Account creation failed!",
                    Errors = new List<string>()
                    {
                        "Currency was not found!"
                    }
                };
            }

            var preValidation = _createAccountValidator.Validate(dto);

            if (!preValidation.IsValid)
            {
                return new CommonResponse
                {
                    IsSuccess = true,
                    Message = "Account creation failed!",
                    Errors = preValidation.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var dtoToAccount = _mapper.Map<Account>(dto);

            await _accountRepository.CreateAsync(dtoToAccount);

            var accountResponse = _mapper.Map<AccountResponse>(dtoToAccount);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Account creation successful!",
                ReceivedData = accountResponse
            };
        }

        public async Task<CommonResponse> UpdateAsync(int id, UpdateAccountDto dto)
        {
            var account = await _accountRepository.GetByIdAsync(id);

            if (account == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Account update failed!",
                    Errors = new List<string>()
                    {
                        "Account was not found!"
                    }
                };
            }

            var preValidation = _updateAccountValidator.Validate(dto);

            if (!preValidation.IsValid)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Account update failed!",
                    Errors = preValidation.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            _mapper.Map(dto, account);

            await _accountRepository.UpdateAsync();

            var accountResponse = _mapper.Map<AccountResponse>(account);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Account update successful!",
                ReceivedData = accountResponse
            };
        }

        public async Task<CommonResponse> DeleteAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);

            if (account == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Account deletion failed!",
                    Errors = new List<string>()
                    {
                        "Account was not found!"
                    }
                };
            }

            await _accountRepository.DeleteAsync(id);

            var accountResponse = _mapper.Map<AccountResponse>(account);

            return new CommonResponse
            {
                IsSuccess = false,
                Message = "Account deletion successful!",
                ReceivedData = accountResponse
            };
        }
    }
}
