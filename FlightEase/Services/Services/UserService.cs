
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories.Repositories;
using Services.EmailService;
using Services.Helpers;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IUserService
    {
        public CreateUserDTO CreateUser(CreateUserDTO userCreate);
        public UserDTO UpdateUser(UserDTO userUpdate);
        public bool DeleteUser(int idTmp);
        public List<UserDTO> GetAll();
        public UserDTO GetById(int idTmp);

        public Task<ResultModel> Register(RegisterDTO userRegister);
        public Task<ResultModel> AuthenticateAsync(LoginDTO userLogin);

        public Task<ResultModel> ForgotPasswordAsync(string email);
        public Task<ResultModel> ResetPasswordAsync(string token, string newPassword);
        public Task<ResultModel> ProcessUserFromGoogleLogin(string displayName, string email);
        Task UpdateUserRank(int userId);


    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IPasswordRepository _passwordRepository;
        private readonly IEmailService _emailService;
        private readonly IMembershipService _membershipService;
        private readonly IOrderDetailService _orderDetailService;
        public UserService(IUserRepository userRepository, JwtTokenHelper jwtTokenHelper, IMembershipRepository membershipRepository, IPasswordRepository passwordRepository, IEmailService emailService, IMembershipService membershipService, IOrderDetailService orderDetailService)
        {
            _userRepository = userRepository;
            _jwtTokenHelper = jwtTokenHelper;
            _membershipRepository = membershipRepository;
            _passwordRepository = passwordRepository;
            _emailService = emailService;
            _membershipService = membershipService;
            _orderDetailService = orderDetailService;
        }
        public CreateUserDTO CreateUser(CreateUserDTO userCreate)
        {
            // Kiểm tra email đã tồn tại chưa
            var existingUser = _userRepository.FirstOrDefault(u => u.Email == userCreate.Email);
            if (existingUser != null)
            {
                throw new Exception("Email already exists"); // Hoặc trả về một thông báo lỗi phù hợp
            }

            // Gán vai trò cho user mới
            userCreate.Role = UserRole.Manager.ToString();

            // Tạo mới đối tượng User
            var user = new User
            {
                Email = userCreate.Email,
                Password = userCreate.Password,
                Gender = userCreate.Gender,
                Nationality = userCreate.Nationality,
                Address = userCreate.Address,
                Fullname = userCreate.Fullname,
                Dob = userCreate.Dob,
                Role = userCreate.Role,
                Status = userCreate.Status,
            };

            // Thực hiện tạo user mới
            _userRepository.Create(user);
            _userRepository.Save();

            return userCreate;
        }



        public UserDTO UpdateUser(UserDTO userUpdate)
        {
            var user = _userRepository.Get(userUpdate.UserId);
            if (user == null) return null;

            // Cập nhật các trường
            user.Email = userUpdate.Email;
            user.Password = userUpdate.Password;
            user.Gender = userUpdate.Gender;
            user.Nationality = userUpdate.Nationality;
            user.Address = userUpdate.Address;
            user.Fullname = userUpdate.Fullname;
            user.Dob = userUpdate.Dob;
            user.Role = userUpdate.Role;

            // Lấy MembershipId từ Rank
            var membership = _membershipRepository.FirstOrDefault(m => m.Rank == userUpdate.Rank);
            if (membership != null)
            {
                user.MembershipId = membership.MembershipId;
            }

            user.Status = userUpdate.Status;

            // Gọi phương thức update từ BaseRepository
            _userRepository.Update(user);
            _userRepository.Save();

            return userUpdate;
        }


        public bool DeleteUser(int idTmp)
        {
            var user = _userRepository.Get(idTmp);
            if (user == null) return false;

            // Gọi phương thức xóa từ BaseRepository
            _userRepository.Delete(user);
            _userRepository.Save();

            return true;
        }

        public List<UserDTO> GetAll()
        {
            var users = _userRepository.Get().ToList();
            var userDtos = new List<UserDTO>();

            foreach (var user in users)
            {
                UpdateUserRank(user.UserId).Wait();

                var membership = _membershipRepository.Get(user.MembershipId);
                var rank = membership != null ? membership.Rank : null;

                userDtos.Add(new UserDTO
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Password = user.Password,
                    Gender = user.Gender,
                    Nationality = user.Nationality,
                    Address = user.Address,
                    Fullname = user.Fullname,
                    Dob = user.Dob,
                    Role = user.Role,
                    Rank = rank,
                    Status = user.Status
                });
            }

            return userDtos;
        }

        public UserDTO GetById(int idTmp)
        {
            UpdateUserRank(idTmp).Wait();

            var user = _userRepository.Get(idTmp);
            if (user == null) return null;

            var membership = _membershipRepository.Get(user.MembershipId);
            var rank = membership != null ? membership.Rank : null;

            return new UserDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Password = user.Password,
                Gender = user.Gender,
                Nationality = user.Nationality,
                Address = user.Address,
                Fullname = user.Fullname,
                Dob = user.Dob,
                Role = user.Role,
                Rank = rank,
                Status = user.Status
            };
        }

        public async Task UpdateUserRank(int userId)
        {
            var totalSpent = await _orderDetailService.GetTotalSpendingAsync(userId);
            if (totalSpent == null) return;

            var user = _userRepository.Get(userId);
            if (user == null) return;

            if (totalSpent >= 8000000) // Ngưỡng cho Gold
            {
                var goldMembership = _membershipRepository.FirstOrDefault(m => m.Rank == "Gold");
                if (goldMembership != null)
                    user.MembershipId = goldMembership.MembershipId;
            }
            else if (totalSpent >= 5000000) // Ngưỡng cho Silver
            {
                var silverMembership = _membershipRepository.FirstOrDefault(m => m.Rank == "Silver");
                if (silverMembership != null)
                    user.MembershipId = silverMembership.MembershipId;
            }
            else
            {
                var bronzeMembership = _membershipRepository.FirstOrDefault(m => m.Rank == "Bronze");
                if (bronzeMembership != null)
                    user.MembershipId = bronzeMembership.MembershipId;
            }

            _userRepository.Update(user);
            await _userRepository.SaveAsync();
        }



        #region Register
        public async Task<ResultModel> Register(RegisterDTO userRegister)
        {
            var result = new ResultModel();

            try
            {
                // Check if user already exists
                var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == userRegister.Email); ;
                if (existingUser != null)
                {
                    result.IsSuccess = false;
                    result.StatusCode = 409;
                    result.Message = "User already exists.";
                    return result;
                }

                // Register the new user
                var newUser = new User
                {
                    Email = userRegister.Email,
                    Password = userRegister.Password,
                    Role = UserRole.Member.ToString(),
                    MembershipId = 3,

                };

                await _userRepository.CreateAsync(newUser);
                await _userRepository.SaveAsync();

                result.IsSuccess = true;
                result.StatusCode = 201;
                result.Message = "User registered successfully.";
                result.Data = new UserDTO
                {
                    UserId = newUser.UserId,
                    Email = newUser.Email
                };
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion

        #region Login
        public async Task<ResultModel> AuthenticateAsync(LoginDTO userLogin)
        {
            var result = new ResultModel();

            try
            {
                var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == userLogin.Email);

                if (user != null && user.Password == userLogin.Password)
                {
                    // Generate JWT token using the extracted User object
                    var token = _jwtTokenHelper.GenerateJwtToken(user);

                    result.IsSuccess = true;
                    result.StatusCode = 200;
                    result.Message = "Login successfully";
                    result.Data = new
                    {
                        Token = token,
                        /*Email = userLogin.Email,
						Password = userLogin.Password,
						Role = user.Role*/
                    };
                }
                else
                {
                    // Incorrect email or password case
                    result.IsSuccess = false;
                    result.StatusCode = 401;
                    result.Message = "Wrong email or password";
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion


        #region ForgotPassword 
        public async Task<ResultModel> ForgotPasswordAsync(string email)
        {
            var result = new ResultModel();

            if (string.IsNullOrEmpty(email))
            {
                result.IsSuccess = false;
                result.StatusCode = 400;
                result.Message = "Email address is required.";
                return result;
            }

            try
            {
                var emailExist = await _userRepository.FirstOrDefaultAsync(e => e.Email == email);
                if (emailExist == null)
                {
                    result.IsSuccess = true;
                    result.StatusCode = 200;
                    result.Message = "Email is not exist.";
                    return result;
                }

                // Generate token and store it
                var token = await GeneratePasswordResetTokenAsync(email);
                if (token == null)
                {
                    result.IsSuccess = false;
                    result.StatusCode = 500;
                    result.Message = "Error generating reset link. Please try again later.";
                    return result;
                }

                // Construct reset link
                var resetLink = $"http://localhost:3000/reset-password?token={token}";

                // Send email with reset link
                await _emailService.SendEmailAsync(email, "Reset Your Password",
                    $@"
    Dear User,

    We received a request to reset the password associated with your account. Please follow the link below to reset your password:

    {resetLink}

    For your security, this link will expire in 24 hours. If you did not request a password reset, please disregard this email. Your account will remain secure.

    If you have any questions, feel free to contact our support team.

    Sincerely,
    The FlightEase Support Team
");

                result.IsSuccess = true;
                result.StatusCode = 200;
                result.Message = " A reset link has been sent to your email. Please check your email !";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Message = $"An unexpected error occurred: {ex.Message}";
                return result;
            }
        }

        private async Task<string?> GeneratePasswordResetTokenAsync(string email)
        {
            var token = Guid.NewGuid().ToString();
            var isStored = await GenerateAndStoreTokenAsync(email, token);

            return isStored ? token : null;
        }
        public async Task<bool> GenerateAndStoreTokenAsync(string email, string token)
        {
            PasswordResetToken passwordResetToken = null;


            {
                var user = await _userRepository.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return false;
                }

                passwordResetToken = new PasswordResetToken
                {

                    UserId = user.UserId,
                    Token = token,
                    ExpirationDate = DateTime.Now.AddMinutes(10),
                    CreatedDate = DateTime.Now,
                    IsUsed = false,
                };
            }

            if (passwordResetToken != null)
            {
                _passwordRepository.CreateAsync(passwordResetToken);
                await _passwordRepository.SaveAsync();
                return true;
            }

            return false;
        }


        public async Task<ResultModel> ResetPasswordAsync(string token, string newPassword)
        {
            var result = new ResultModel();

            // Validate parameters
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword))
            {
                result.IsSuccess = false;
                result.StatusCode = 400;
                result.Message = "Token and new password are required.";
                return result;
            }

            try
            {
                // Find the token and check expiration
                var tokenRecord = await _passwordRepository
                    .FirstOrDefaultAsync(t => t.Token == token && t.ExpirationDate > DateTime.Now);

                if (tokenRecord == null || tokenRecord.IsUsed)
                {
                    result.IsSuccess = false;
                    result.StatusCode = 400;
                    result.Message = "Invalid or expired token.";
                    return result;
                }

                // Retrieve the user associated with the token
                var user = await _userRepository.GetAsync(tokenRecord.UserId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.StatusCode = 404;
                    result.Message = "User not found.";
                    return result;
                }

                user.Password = newPassword;
                _userRepository.Update(user);

                tokenRecord.IsUsed = true;
                await _userRepository.SaveAsync();

                // Set success result
                result.IsSuccess = true;
                result.StatusCode = 200;
                result.Message = "Password reset successfully.";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Message = $"An unexpected error occurred: {ex.Message}";
            }

            return result;
        }
        #endregion

        public async Task<ResultModel> ProcessUserFromGoogleLogin(string displayName, string email)
        {
            var result = new ResultModel();

            try
            {
                var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == email);
                if (existingUser != null)
                {
                    var token = _jwtTokenHelper.GenerateJwtToken(existingUser);

                    result.IsSuccess = true;
                    result.StatusCode = 200;
                    result.Message = "User already exists, logged in successfully.";
                    result.Data = new
                    {
                        User = new UserDTO
                        {
                            UserId = existingUser.UserId,
                            Email = existingUser.Email,
                            Password = existingUser.Password,
                            Fullname = existingUser.Fullname,
                            Status = existingUser.Status
                        },
                        Token = token
                    };
                    return result;
                }

                var newUser = new User
                {
                    Email = email,
                    Fullname = displayName,
                    Password = "GoogleUser",
                    Role = UserRole.Member.ToString(),
                    Status = "Active",
                    MembershipId = 1
                };

                await _userRepository.CreateAsync(newUser);
                await _userRepository.SaveAsync();

                var newToken = _jwtTokenHelper.GenerateJwtToken(newUser);

                result.IsSuccess = true;
                result.StatusCode = 201;
                result.Message = "Google User processed successfully";
                result.Data = new
                {
                    User = new UserDTO
                    {
                        UserId = newUser.UserId,
                        Email = newUser.Email,
                        Fullname = newUser.Fullname,
                        Status = newUser.Status
                    },
                    Token = newToken
                };
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Message = ex.Message;
            }

            return result;
        }

    }
}