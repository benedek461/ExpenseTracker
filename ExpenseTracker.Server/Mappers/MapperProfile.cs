using AutoMapper;
using ExpenseTracker.Server.Dtos;
using ExpenseTracker.Server.Models;
using ExpenseTracker.Server.Responses;

namespace ExpenseTracker.Server.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserResponse>();

            CreateMap<CreateUserDto, User>()
                .AfterMap((src, dest) => dest.CreatedAt = DateTime.Now)
                .ForSourceMember(source => source.ConfirmPassword, opt => opt.DoNotValidate());

            CreateMap<UpdateUserDto, User>()
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName != null ? src.UserName : src.UserName))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName != null ? src.FirstName : src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName != null ? src.LastName : src.LastName));

            CreateMap<CreateAccountDto, Account>();

            CreateMap<Account, AccountResponse>();

            CreateMap<UpdateAccountDto, Account>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Currency, CurrencyResponse>();

            CreateMap<Category, CategoryResponse>();

            CreateMap<Transaction, TransactionResponse>();

            CreateMap<CreateTransactionDto, Transaction>()
                .AfterMap((src, dest) => dest.CreatedAt = DateTime.Now);

            CreateMap<UpdateTransactionDto, Transaction>();
        }
    }
}