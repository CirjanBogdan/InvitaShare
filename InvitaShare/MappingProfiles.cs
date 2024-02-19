using AutoMapper;
using InvitaShare.Models;
using InvitaShare.ViewModels;

namespace InvitaShare
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<WeddingEventDTO, WeddingEvent>();
            CreateMap<BaptismEventDTO, BaptismEvent>();
        }
    }
}
