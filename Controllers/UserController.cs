using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Training_API.Data;
using Training_API.DTOS;
using Training_API.Models;

namespace Training_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyAppContext _db;

        public UserController(MyAppContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers() // AS a DTO !!!!!!!!!!!!!!!!!!
        {

            List<User> users = await _db.Users.Include(x =>x.AddressList)
                .ToListAsync();

            List<UserDTO> dtoUsers = new List<UserDTO>();

            foreach(var u in users)
            {
                List<AddressDTO> addressList = new List<AddressDTO>();
                if (u.AddressList != null)
                {
                    foreach (var address in u.AddressList)
                    {
                        AddressDTO addressDTO = new AddressDTO()
                        {
                            FullName = address.FullName,
                            PhoneNumber = address.PhoneNumber,
                            Street = address.Street,
                            City = address.City,
                            PostalCode = address.PostalCode,
                            Country = address.Country,
                            AddressId = address.AddressId
                        };
                        addressList.Add(addressDTO);
                    }
                }
                dtoUsers.Add(new UserDTO()
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    PasswordHash = u.PasswordHash,
                    AddressList = addressList
                });
            }
            return Ok(dtoUsers);

        }

        [HttpGet("{UserID}")]
        public async Task<IActionResult> GetUsers(int UserID)
        {
            var user = await _db.Users.Include(x => x.AddressList)
                                        .FirstOrDefaultAsync(x => x.UserId == UserID);
            if (user == null)
            {
                return NotFound();
            }

            UserDTO userDTO = new()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                AddressList = new List<AddressDTO>()
                
            };
            if (user.AddressList != null)
            {
                foreach (var address in user.AddressList)
                {
                    AddressDTO addressDTO = new()
                    {
                        FullName = address.FullName,
                        PhoneNumber = address.PhoneNumber,
                        Street = address.Street,
                        City = address.City,
                        PostalCode = address.PostalCode,
                        Country = address.Country,
                        AddressId = address.AddressId
                        
                    };
                    userDTO.AddressList.Add(addressDTO);
                }
            }
            return Ok(userDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewUser(UserDTO newUser)
        {
            if (newUser == null)
            {
                return BadRequest();
            }
            User user = new User()
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                PasswordHash = newUser.PasswordHash,
                AddressList = new List<Address>()
            };
            if (newUser.AddressList != null) 
            { 
                foreach (var o in newUser.AddressList)
                {
                    Address address = new Address()
                    {
                        FullName = o.FullName
                    };
                    user.AddressList.Add(address);  
                }
            }

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return Ok(newUser.UserName);
        }


        [HttpPut("{UserID}")]
        public async Task<IActionResult> UpdateUser(int UserID , [FromBody]UserDTO updatedUser)
        {
            var userToUpdate = await _db.Users.SingleOrDefaultAsync(x => x.UserId == UserID);
            if(userToUpdate == null)
            {
                return BadRequest();
            }
            userToUpdate.UserName = updatedUser.UserName;
            userToUpdate.Email = updatedUser.Email;
            userToUpdate.IsAdmin = updatedUser.IsAdmin;
            userToUpdate.CreatedAt = updatedUser.CreatedAt;
            userToUpdate.Phone = updatedUser.Phone;
            userToUpdate.PasswordHash = updatedUser.PasswordHash;

            List<Address> addresses = new List<Address>();
            if (updatedUser.AddressList != null)
            {
                foreach (var dtoAddress in updatedUser.AddressList)
                {
                    Address address = new Address()
                    {
                        UserId = dtoAddress.UserId,
                        FullName = dtoAddress.FullName,
                        PhoneNumber = dtoAddress.PhoneNumber,
                        Street = dtoAddress.Street,
                        City = dtoAddress.City,
                        PostalCode = dtoAddress.PostalCode,
                        Country = dtoAddress.Country
                    };
                    addresses.Add(address);
                }
                userToUpdate.AddressList = addresses;
            }
            await _db.SaveChangesAsync();
            return Ok(updatedUser);
        }


        [HttpDelete("{UserID}")]
        public async Task<IActionResult> DeleteUser(int UserID)
        {
            var userToDelete = await _db.Users.Include(x => x.AddressList)
                .SingleOrDefaultAsync(x => x.UserId == UserID);

            if (userToDelete == null)
            {
                return NotFound();
            }
            userToDelete.UserName = "Delted-User";
            await _db.SaveChangesAsync();
            return Ok($"User {UserID} has been Deleted !");

        }
    }
}
