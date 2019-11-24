using System.ComponentModel;
using AutoMapper;
using ZNV.Timesheet.RoleManagement;

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
                config.CreateMap<EmailTemplate.EmailTemplate, EmailTemplate.EmailTemplate>()
                      .IgnoreNoMap();
                config.CreateMap<ApproveLog.ApproveLog, ApproveLog.ApproveLog>()
                      .IgnoreNoMap();
                config.CreateMap<Holiday.Holiday, Holiday.Holiday>()
                      .IgnoreNoMap();
                config.CreateMap<PermissionModule.PermissionModule, PermissionModule.PermissionModule>()
                      .IgnoreNoMap();
                config.CreateMap<Project.Project, Project.Project>()
                      .IgnoreNoMap();
                config.CreateMap<Role, Role>()
                      .IgnoreNoMap();
                config.CreateMap<RoleModule, RoleModule>()
                      .IgnoreNoMap();
                config.CreateMap<UserRole, UserRole>()
                      .IgnoreNoMap();
                config.CreateMap<Timesheet.Timesheet, Timesheet.Timesheet>()
                      .IgnoreNoMap();
                config.CreateMap<UserSetting.UserSetting, UserSetting.UserSetting>()
                      .IgnoreNoMap();
                config.CreateMap<ConfigurationManagement.Configuration, ConfigurationManagement.Configuration>()
                     .IgnoreNoMap();
            });
        }

        public static void Ignore<TSource, TDestination>()
           where TSource : BaseEntity
           where TDestination : BaseEntity
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<TSource, TDestination>()
                    //Ignoring the Address property of the destination type
                    .ForMember(dest => dest.Id, act => act.Ignore())
                    .ForMember(dest => dest.Creator, act => act.Ignore())
                    .ForMember(dest => dest.CreationTime, act => act.Ignore());
            });
        }
    }
}
