using AutoMapper;
using uTest.BLL.DTO;
using uTest.Entities.General;

namespace uTest.BLL.Infrastructure
{
    public class MapperConfig
    {
        public static MapperConfiguration GetConfigToDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Test, TestDTO>();
                cfg.CreateMap<Question, QuestionDTO>();
                cfg.CreateMap<Answer, AnswerDTO>();
                cfg.CreateMap<SolvedTest, SolvedTestDTO>();
            });
        }

        public static MapperConfiguration GetConfigFromDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TestDTO, Test>();
                cfg.CreateMap<QuestionDTO, Question>();
                cfg.CreateMap<AnswerDTO, Answer>();
                cfg.CreateMap<SolvedTestDTO, SolvedTest>();
            });
        }
    }
}
