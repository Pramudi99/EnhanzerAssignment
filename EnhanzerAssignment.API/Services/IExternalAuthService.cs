using EnhanzerAssignment.API.DTOs;

namespace EnhanzerAssignment.API.Services
{
    public interface IExternalAuthService
    {
         Task<(bool Success, string Message, List<LocationDto> Locations)>
          LoginAsync(LoginRequest request);
       
    }
}
