using AutoMapper;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;
using Microsoft.AspNetCore.Identity;

namespace FloralFusion.Application.Services
{
    public class AbstractClass
    {
        public readonly IUniteOfWork uniteOfWork;
        public readonly IMapper mapper;
        public readonly SmtpService smtpService;

   
        public AbstractClass(IUniteOfWork uniteOfWork, IMapper mapper, SmtpService smtpService)
        {
            this.uniteOfWork = uniteOfWork;
            this.mapper = mapper;
            this.smtpService = smtpService;
        }
    }
}
