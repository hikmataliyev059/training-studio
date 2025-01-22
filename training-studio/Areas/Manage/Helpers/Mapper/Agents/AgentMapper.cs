using AutoMapper;
using training_studio.Areas.Manage.Helpers.DTOs.Agents;
using training_studio.Models;

namespace training_studio.Areas.Manage.Helpers.Mapper.Agents;

public class AgentMapper : Profile
{
    public AgentMapper()
    {
        CreateMap<CreateAgentDto, Agent>().ReverseMap();
        CreateMap<UpdateAgentDto, Agent>().ReverseMap();
    }
}
