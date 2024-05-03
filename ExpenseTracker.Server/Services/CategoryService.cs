using AutoMapper;
using ExpenseTracker.Server.Repositories.Interfaces;
using ExpenseTracker.Server.Responses;
using ExpenseTracker.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Server.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var categoryResponses = new List<CategoryResponse>();

            foreach (var category in categories)
            {
                categoryResponses.Add(_mapper.Map<CategoryResponse>(category));
            }

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Search successful!",
                ReceivedData = categoryResponses
            };
        }

        public async Task<CommonResponse> GetByIdAsnyc(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return new CommonResponse
                {
                    IsSuccess = false,
                    Message = "Search failed!",
                    Errors = new List<string>()
                    {
                        "Category was not found!"
                    }
                };
            }

            var categoryResponse = _mapper.Map<CategoryResponse>(category);

            return new CommonResponse
            {
                IsSuccess = true,
                Message = "Search successful!",
                ReceivedData = categoryResponse
            };
        }
    }
}
