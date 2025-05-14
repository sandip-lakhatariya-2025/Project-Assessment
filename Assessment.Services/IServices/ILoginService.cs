using Assessment.Models.ViewModel;

namespace Assessment.Services.IServices;

public interface ILoginService
{
    Task<(bool isSuccess, string message)> CheckLoginCredentials(UserViewModel user);
}
