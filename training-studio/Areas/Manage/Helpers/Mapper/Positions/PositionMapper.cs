using AutoMapper;
using training_studio.Areas.Manage.Helpers.DTOs.Positions;
using training_studio.Models;

namespace training_studio.Areas.Manage.Helpers.Mapper.Positions;

public class PositionMapper : Profile
{
    public PositionMapper()
    {
        CreateMap<CreatePositionDto, Position>().ReverseMap();
        CreateMap<UpdatePositionDto, Position>().ReverseMap();
    }
}
