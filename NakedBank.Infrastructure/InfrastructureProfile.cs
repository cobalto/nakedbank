using AutoMapper;
using NakedBank.Infrastructure.Entities;

namespace NakedBank.Application
{
    public class InfrastructureProfile : Profile
    {
        public InfrastructureProfile()
        {
            CreateMap<Domain.User, User>()
               .ForMember(app => app.Login, opt => opt.MapFrom(d => d.Login.ToString()))
               .ForMember(app => app.Password, opt => opt.MapFrom(d => d.Password.Hash.ToString()))
               .ForMember(app => app.EmailAddress, opt => opt.MapFrom(d => d.EmailAddress.ToString()))
               .ForMember(app => app.PhoneNumber, opt => opt.MapFrom(d => d.PhoneNumber.ToString()))
               .ForMember(app => app.LastAccessAt, opt => opt.MapFrom(d => d.LastAccessAt));

            CreateMap<User, Domain.User>()
               .ForMember(app => app.Login, opt => opt.MapFrom(i => new Domain.Login(i.Login)))
               .ForMember(app => app.Password, opt => opt.MapFrom(i => new Domain.Password(new Domain.Hash(i.Password))))
               .ForMember(app => app.EmailAddress, opt => opt.MapFrom(i => new Domain.Email(i.EmailAddress)))
               .ForMember(app => app.PhoneNumber, opt => opt.MapFrom(i => new Domain.PhoneNumber(i.PhoneNumber)))
               .ForMember(app => app.LastAccessAt, opt => opt.MapFrom(i => i.LastAccessAt));

            CreateMap<Domain.Token, Token>();
            CreateMap<Token, Domain.Token>();

            CreateMap<Domain.Account, Account>();
            CreateMap<Domain.Balance, Balance>();
            CreateMap<Domain.Transaction, Transaction>();

            CreateMap<Account, Domain.Account>();
            CreateMap<Balance, Domain.Balance>();
            CreateMap<Transaction, Domain.Transaction>();
        }
    }
}
