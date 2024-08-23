using AutoMapper;
using backendProjesi.Models;
using System;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Users, VMUsers>();
        CreateMap<VMUsers, Users>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null && srcMember.ToString() != ""));
        CreateMap<Users, VMUsers2>();
    }
}