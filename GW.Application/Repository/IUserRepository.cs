using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GW.Application.Sevices;
using GW.Core.Context;
using GW.Core.Models.Dto;
using GW.Core.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace GW.Application.Repository
{
    public interface IUserRepository
    {
        public Result<List<RoleDto>> Login(LoginInfo info);
        public Result<string> Token(LoginInfo info);
        public Result<UserRoleDto> UserRole(Guid userId,string role);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IBaseData _baseData;
        private SupervisorContext _context;
        private readonly IMapper _mapper;

        public UserRepository(SupervisorContext context,IMapper mapper,IBaseData baseData)
        {
            _context = context;
            _mapper = mapper;
            _baseData = baseData;
        }

        #region Login
        public Result<List<RoleDto>> Login(LoginInfo info)
        {
            var user = _context.Users.Where(u=>u.Username==info.UserName ).FirstOrDefault();
            if (user is null) return Result<List<RoleDto>>.Fail(ErrorCode.NOT_FOUND, "کاربری با این مشخصات وجود ندارد!");
            if(!BCrypt.Net.BCrypt.EnhancedVerify(info.Password, user.Password))
            {
                return Result<List<RoleDto>>.Fail(ErrorCode.NOT_FOUND, "کاربری با این مشخصات وجود ندارد!");
            }

            var userRoles = _context.UserRoles.Where(u=>u.FkUserId==user.Id).Select(u=>u.Role).ToList();
            List<RoleDto> roles = new();
            foreach (var role in userRoles)
            {
                roles.Add(_mapper.Map<RoleDto>(role));
            }

            return Result<List<RoleDto>>.Ok(roles);
        }
        #endregion

        #region Token
        public Result<string> Token(LoginInfo info)
        {
            var user = _context.Users.Where(u => u.Username == info.UserName).FirstOrDefault();
            if (user is null) return Result<string>.Fail(ErrorCode.NOT_FOUND, "کاربری با این مشخصات وجود ندارد!");
            if (!BCrypt.Net.BCrypt.EnhancedVerify(info.Password, user.Password))
            {
                return Result<string>.Fail(ErrorCode.NOT_FOUND, "کاربری با این مشخصات وجود ندارد!");
            }

            if (!_context.UserRoles
                .Include(u=>u.Role)
                 .Any(u => u.FkUserId == user.Id && u.Role.Name == info.Role))
                return Result<string>.Fail(ErrorCode.INVALID_ID, "شناسه نامعتبر!");
            var token = _baseData.GenerateToken(user.Id, info.Role);
            return Result<string>.Ok(token);
        }
        #endregion

        #region UserRole
        public Result<UserRoleDto> UserRole(Guid userId, string role)
        {
            var userRole = _context.UserRoles
                .AsNoTracking()
                .Include(u => u.Role)
                .Where(u => u.Role.Name == role && u.FkUserId == userId)
                .FirstOrDefault();
            if (userRole is null) return Result<UserRoleDto>.Fail(ErrorCode.NOT_FOUND, "کاربری با این مشخصات وجود ندارد!");
            var result = _mapper.Map<UserRoleDto>(userRole);
            return Result<UserRoleDto>.Ok(result);
        }
        #endregion
    }
}
