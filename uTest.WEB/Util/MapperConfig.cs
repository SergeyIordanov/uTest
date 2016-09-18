using AutoMapper;
using uTest.Auth.BLL.DTO;
using uTest.BLL.DTO;
using uTest.WEB.ViewModels;

namespace uTest.WEB.Util
{
    public class MapperConfig
    {
        public static MapperConfiguration GetConfigFromViewModelToDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TestViewModel, TestDTO>();
                cfg.CreateMap<QuestionViewModel, QuestionDTO>();
                cfg.CreateMap<AnswerViewModel, AnswerDTO>();
                cfg.CreateMap<SolvedTestViewModel, SolvedTestDTO>();
                cfg.CreateMap<StatisticViewModel, StatisticDTO>();
                cfg.CreateMap<UserViewModel, UserDTO>();
            });
        }

        public static MapperConfiguration GetConfigToViewModel()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TestDTO, TestViewModel>();
                cfg.CreateMap<QuestionDTO, QuestionViewModel>();
                cfg.CreateMap<AnswerDTO, AnswerViewModel>();
                cfg.CreateMap<SolvedTestDTO, SolvedTestViewModel>();
                cfg.CreateMap<StatisticDTO, StatisticViewModel>();
                cfg.CreateMap<UserDTO, UserViewModel>();
            });
        }
    }
}