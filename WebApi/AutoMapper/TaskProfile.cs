using AutoMapper;
using Domain.Commands;
using Domain.ViewModel;
using Domain.DataModels;

namespace WebApi.AutoMapper
{
    public class TaskProfile:Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskCommand, Task>();
            CreateMap<UpdateTaskCommand, Task>();
            CreateMap<Task, TaskVm>()
                .ForMember(
                         dest => dest.AssignedToAvatar,
                         opt => opt.MapFrom(src => src.Member.Avatar)
                    );


        }
    }
}
