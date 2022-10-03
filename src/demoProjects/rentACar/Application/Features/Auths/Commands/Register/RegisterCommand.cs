using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.Auths.Commands.Register;

public class RegisterCommand : IRequest<RegisteredDto>
{
    public UserForRegisterDto UserForRegisterDto { get; set; }
    public string IpAddress { get; set; }
    
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredDto>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public RegisterCommandHandler(AuthBusinessRules authBusinessRules, IUserRepository userRepository, IAuthService authService)
        {
            _authBusinessRules = authBusinessRules;
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<RegisteredDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.EmailCanNotBeDuplicatedWhenRegistered(request.UserForRegisterDto.Email);
            byte[] passwordHash, passwordSalt;

            HashingHelper.CreatePasswordHash(request.UserForRegisterDto.Password,out passwordHash, out passwordSalt);

            User newUser = new User
            {
                
                FirstName = request.UserForRegisterDto.FirstName,
                LastName = request.UserForRegisterDto.LastName,
                Email = request.UserForRegisterDto.Email,
                PasswordSalt = passwordHash,
                PasswordHash = passwordSalt,
                Status = true
            };
            
            User createdUSer = await _userRepository.AddAsync(newUser);

            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUSer);
            RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(createdUSer, request.IpAddress);
            RefreshToken addRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredDto registeredDto = new RegisteredDto
            {
                AccessToken = createdAccessToken,
                RefreshToken = addRefreshToken
            };
            return registeredDto;
        }
    }
}
