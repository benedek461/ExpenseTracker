using AutoMapper;
using ExpenseTracker.Server.Repositories.Interfaces;
using ExpenseTracker.Server.Responses;
using ExpenseTracker.Server.Services.Interfaces;

namespace ExpenseTracker.Server.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyService(ICurrencyRepository currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> GetAllAsync()
        {
            var currencies = await _currencyRepository.GetAllAsync();

            var currencyResponses = new List<CurrencyResponse>();

            foreach (var currency in currencies)
            {
                currencyResponses.Add(_mapper.Map<CurrencyResponse>(currency));
            }

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Search successful!",
                ReceivedData = currencyResponses
            };
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            var currency = await _currencyRepository.GetByIdAsync(id);

            if (currency == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Search failed!",
                    Errors = new List<string>()
                    {
                        "Currency was not found!"
                    }
                };
            }

            var currencyResponse = _mapper.Map<CurrencyResponse>(currency);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Search successful!",
                ReceivedData = currencyResponse
            };
        }
    }
}
