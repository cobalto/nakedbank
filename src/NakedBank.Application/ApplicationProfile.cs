using AutoMapper;
using NakedBank.Shared.Models.Responses;

namespace NakedBank.Application
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Domain.User, ProfileResponse>()
               .ForMember(app => app.Login, opt => opt.MapFrom(d => d.Login.ToString()))
               .ForMember(app => app.EmailAddress, opt => opt.MapFrom(d => d.EmailAddress.ToString()))
               .ForMember(app => app.PhoneNumber, opt => opt.MapFrom(d => d.PhoneNumber.ToString()));

            CreateMap<ProfileResponse, Domain.User>()
               .ForMember(app => app.Login, opt => opt.MapFrom(i => new Domain.Login(i.Login)))
               .ForMember(app => app.EmailAddress, opt => opt.MapFrom(i => new Domain.Email(i.EmailAddress)))
               .ForMember(app => app.PhoneNumber, opt => opt.MapFrom(i => new Domain.PhoneNumber(i.PhoneNumber)));

            CreateMap<Domain.Account, AccountResponse>();
            CreateMap<Domain.Balance, BalanceResponse>();
            CreateMap<Domain.Transaction, TransactionResponse>();

            CreateMap<BalanceResponse, Domain.Account>();
            CreateMap<BalanceResponse, Domain.Balance>();
            CreateMap<TransactionResponse, Domain.Transaction>();
        }
    }
}
