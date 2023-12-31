﻿using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Objects.Reponses;
using Domain.Objects.Requests;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public string LogIn(LogInRequest logInRequest)
        {
            var userId = _userRepository.GetUserIdByNameAndPass(logInRequest.UserName, logInRequest.Password);

            if (!userId.HasValue)
                throw new InvalidCredentialException("Usuário ou senha inválidos");

            return GenerateAuthToken(userId.Value.ToString());
        }

        public async Task SaveUser(CreateUserRequest createUserRequest)
        {
            if (await _userRepository.HasUserWithSameName(createUserRequest.Name))
                throw new InvalidOperationException("Já existe um usuário com esse nome");

            await _userRepository.SaveAsync(_mapper.Map<User>(createUserRequest));
        }

        public async Task UpdateUser(UpdateUserRequest updateUserRequest)
        {
            if (updateUserRequest.Name == null && updateUserRequest.Password == null)
                throw new InvalidOperationException("Nenhuma modificação foi realizada!");

            var user = await _userRepository.GetById(updateUserRequest.UserId) ?? throw new InvalidOperationException("Nenhum usuário encontrado");

            if (updateUserRequest.Name != null)
            {
                if (await _userRepository.HasUserWithSameName(updateUserRequest.Name))
                    throw new InvalidOperationException("Já existe um usuário com esse nome");

                user.Name = updateUserRequest.Name;
            }

            if (updateUserRequest.Password != null)
                user.Password = updateUserRequest.Password;

            user.UpdatedAt = DateTime.Now;

            await _userRepository.UpdateAsync(user);
        }

        public ICollection<UserResponse> GetUsers(int? id, string? name, int currentPage, int takeQuantity)
        {
            var users = _userRepository.GetUsers(id, name, currentPage, takeQuantity);

            if (!users.Any())
                throw new InvalidOperationException("Nenhum usuário encontrado");

            return users;
        }

        public async Task DeleteUser(int id) => await _userRepository.Delete(id);

        private static string GenerateAuthToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("secretPass2k23Backend");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
