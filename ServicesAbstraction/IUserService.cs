using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterUserDto dto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    }
}
