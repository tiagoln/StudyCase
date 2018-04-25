using System;
using System.Collections.Generic;
using System.Linq;
using Core.DTOs;

namespace Core.Model.Extensions
{
    public static class UserExtensions
    {
        public static UserDto GetDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.UserProfile.FirstName,
                LastName = user.UserProfile.LastName,
                CreatedAt = user.CreatedAt
            };
        }

        public static IEnumerable<UserDto> GetDtoList(this IEnumerable<User> userList)
        {
            return userList.Select(GetDto);
        }

        public static User GetModelEntity(this UserDto dto)
        {
            return new User
            {
                CreatedAt = DateTime.Now,
                UserProfile = new UserProfile
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Orders = new List<Order>()
                }
            };
        }
    }
}