using quizapi.Data_Access_Layer.context;
using quizapi.Data_Access_Layer.Entities;
using quizapi.Data_Access_Layer.Repository.Interface;

﻿using Microsoft.EntityFrameworkCore;


namespace quizapi.Data_Access_Layer.Repository.Implementation
{
    public class UserRepo : IUserRep
    {
        private Quizdbcontext dbContext;

        public UserRepo(Quizdbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteAsync(int id)
        {
            var existingUsers = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (existingUsers == null)
            {
                return null;
            }
            dbContext.Users.Remove(existingUsers);
            await dbContext.SaveChangesAsync();
            return existingUsers;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users.Include("UserRole").ToListAsync();

        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await dbContext.Users.Include("UserRole").FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await dbContext.Users.Include("UserRole").FirstOrDefaultAsync(x => x.UserId == id);
        }



        public async Task<User> UpdateAsync(int id, User user)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.FName = user.FName;
            existingUser.LName = user.LName;
            existingUser.Score = user.Score;
            existingUser.TimeTaken = user.TimeTaken;
            existingUser.UserRoleId = user.UserRoleId;

            await dbContext.SaveChangesAsync();
            return existingUser;
        }
    }
}