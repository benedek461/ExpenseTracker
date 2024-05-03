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
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IValidator<CreateTransactionDto> _createTransactionValidator;
        private readonly IValidator<UpdateTransactionDto> _updateTransactionValidator;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IMapper mapper,
            ICategoryRepository categoryRepository,
            ICurrencyRepository currencyRepository,
            IValidator<CreateTransactionDto> createTransactionValidator,
            IValidator<UpdateTransactionDto> updateTransactionValidator,
            UserManager<User> userManager)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _currencyRepository = currencyRepository;
            _createTransactionValidator = createTransactionValidator;
            _updateTransactionValidator = updateTransactionValidator;
            _userManager = userManager;
        }

        public async Task<CommonResponse> GetAllAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();

            var transactionResponses = new List<TransactionResponse>();

            foreach (var transaction in transactions)
            {
                transactionResponses.Add(_mapper.Map<TransactionResponse>(transaction));
            }

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Search successful!",
                ReceivedData = transactionResponses
            };
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Search failed!",
                    Errors = new List<string>()
                    {
                        "Transaction was not found!"
                    }
                };
            }

            var transactionResponse = _mapper.Map<TransactionResponse>(transaction);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Search successful!",
                ReceivedData = transactionResponse
            };
        }

        public async Task<CommonResponse> CreateAsync(CreateTransactionDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Transaction creation failed!",
                    Errors = new List<string>()
                    {
                        "User was not found!"
                    }
                };
            }

            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);

            if (category == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Transaction creation failed!",
                    Errors = new List<string>()
                    {
                        "Category was not found!"
                    }
                };
            }

            var currency = await _currencyRepository.GetByIdAsync(dto.CurrencyId);

            if (currency == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Transaction creation failed!",
                    Errors = new List<string>()
                    {
                        "Currency was not found!"
                    }
                };
            }

            var preValidation = _createTransactionValidator.Validate(dto);

            if (!preValidation.IsValid)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Transaction creation failed!",
                    Errors = preValidation.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var dtoToTransaction = _mapper.Map<Transaction>(dto);

            await _transactionRepository.CreateAsync(dtoToTransaction);

            var transactionResponse = _mapper.Map<TransactionResponse>(dtoToTransaction);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Transaction creation successful!",
                ReceivedData = transactionResponse
            };
        }

        public async Task<CommonResponse> UpdateAsync(int id,UpdateTransactionDto dto)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Transaction update failed!",
                    Errors = new List<string>()
                    {
                        "Transaction was not found!"
                    }
                };
            }

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Transaction update failed!",
                    Errors = new List<string>()
                    {
                        "Category was not found!"
                    }
                };
            }

            var preValidation = _updateTransactionValidator.Validate(dto);

            if (!preValidation.IsValid)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Transaction update failed!",
                    Errors = preValidation.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            _mapper.Map(dto, transaction);

            await _transactionRepository.UpdateAsync();

            var transactionResponse = _mapper.Map<TransactionResponse>(transaction);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Transaction update successful!",
                ReceivedData = transactionResponse
            };
        }

        public async Task<CommonResponse> DeleteAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Transaction deletion failed!",
                    Errors = new List<string>()
                    {
                        "Transaction was not found!"
                    }
                };
            }

            await _transactionRepository.DeleteAsync(id);

            var transactionResponse = _mapper.Map<TransactionResponse>(transaction);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Transaction deletion successful!",
                ReceivedData = transactionResponse
            };
        }
    }
}
