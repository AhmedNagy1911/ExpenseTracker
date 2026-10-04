using ExpenseTracker.Application.Contracts.Notifications;
using ExpenseTracker.Application.Contracts.Users;
using ExpenseTracker.Domain.Entities;
using Mapster;

namespace ExpenseTracker.Application.Mapping;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.roles);

        config.NewConfig<CreateUserRequest, ApplicationUser>()
            .Map(dest => dest.EmailConfirmed, src => true);

        config.NewConfig<Notification, NotificationResponse>()
            .Map(dest => dest.CategoryName, src => src.Budget.Category.Name);
    }
}
