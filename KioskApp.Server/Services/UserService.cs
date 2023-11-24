using AutoMapper;
using KioskApp.Model.Dto;
using KioskApp.Model.Entities;
using KioskApp.Server.Core.DbContext;
using KioskApp.Server.Core.Encryption;
using KioskApp.Server.Core.Enum;
using KioskApp.Server.Core.Tokens;
using System.Security.Authentication;

namespace KioskApp.Server.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public UserService(AppDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        public async Task<User> Register(RegisterUserDto dto)
        {
            var userEmail = await context.Users
                                         .Where(u => u.Email == dto.Email)
                                         .FirstOrDefaultAsync();

            if (userEmail != null)
            {
                throw new AuthenticationException("User with email already exists.");
            }

            var userFirstName = await context.Users
                                                 .Where(u => u.Firstname == dto.Firstname)
                                                 .FirstOrDefaultAsync();

            if (userFirstName != null) 
            {
                throw new AuthenticationException("User with first name already exists.");
            }

            var userLastName = await context.Users
                                            .Where(u => u.Lastname == dto.Lastname)
                                            .FirstOrDefaultAsync();

            if (userLastName != null) 
            {
                throw new AuthenticationException("User with last name already exists.");
            }

            PasswordHasher.CreatePassword(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var addUser = mapper.Map<User>(dto);
            addUser.PasswordHash = passwordHash;
            addUser.PasswordSalt = passwordSalt;
            addUser.VerificationToken = RandomTokens.CreateRandomToken();
            addUser.Role = UserRole.User;
            addUser.DateCreated = DateTime.Now;

            context.Users.Add(addUser);
            await context.SaveChangesAsync();

            return addUser;
        }

        public async Task<(User user, UserRole role)> Login(UserLoginDto dto)
        {
            User user =  await context.Users
                                    .Where(u => u.Email == dto.Email)
                                    .FirstOrDefaultAsync();

            if (user == null) 
            {
                throw new AuthenticationException("Invalid Email");
            }

            if (!user.IsValidated)
            {
                throw new AuthenticationException("Waiting for validation");
            }

            if (!user.IsActive)
            {
                throw new AuthenticationException("Account is deactivated");
            }

            if (!PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt)) 
            {
                throw new AuthenticationException("Invalid Password");
            }

            return (user, user.Role);
        }

        public async Task<List<User>> GetAllUsers()
            => await context.Users.ToListAsync();

        public async Task<User> Verify(string token)
        {
            var user = await context.Users
                                    .Where(u => u.VerificationToken == token)
                                    .FirstOrDefaultAsync();

            user.VerifiedAt = DateTime.Now;
            user.IsValidated = true;
            user.IsActive = true;

            context.Users.Update(user);
            await context.SaveChangesAsync();   

            return user;    
        }

        public async Task<User> ForgotPassword(string email)
        {
            var user = await context.Users 
                                    .Where(u => u.Email == email)
                                    .FirstOrDefaultAsync(); 

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.PasswordResetToken = RandomTokens.CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<User> ResetPassword(ResetPasswordDto dto)
        {
            var user = await context.Users
                                    .Where(u => u.PasswordResetToken == dto.Token)
                                    .FirstOrDefaultAsync();

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                throw new InvalidOperationException("Invalid Token.");
            }

            PasswordHasher.CreatePassword(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return user;
        }
    }
}
