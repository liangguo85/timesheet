using System.ComponentModel;
using AutoMapper;

namespace ZNV.Timesheet
{
    public class NoAutoMappperAttribute : System.Attribute
    {
    }
    public static class IgnoreNoMapExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreNoMap<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            foreach (var property in sourceType.GetProperties())
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(sourceType)[property.Name];
                NoAutoMappperAttribute attribute = (NoAutoMappperAttribute)descriptor.Attributes[typeof(NoAutoMappperAttribute)];
                if (attribute != null)
                    expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }
    }

    public static class AutoMappperExtension
    {
        public static void InitializeAutomapper()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Team.Team, Team.Team>()
                      .IgnoreNoMap();
            });
        }
    }

    //public static class AutoMappperExtension<TSource, TDestination>
}
