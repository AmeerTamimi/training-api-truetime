using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.IO;
using Training_API.Data;
using Training_API.DTOS;
using Training_API. Models;

namespace Training_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly MyAppContext _db;

        public AddressController(MyAppContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<IActionResult> getAddresses()
        {
            return Ok(await _db.Addresses.ToArrayAsync());
        }

        [HttpGet("{AddressID}")]
        public async Task<IActionResult> getAddresses(int AddressID)
        {
            var address = await _db.Addresses.SingleOrDefaultAsync(x => x.AddressId == AddressID);
            if (address == null)
            {
                return NotFound();
            }
            AddressDTO addressDTO = new AddressDTO()
            {
                UserId = address.UserId,
                FullName = address.FullName,
                PhoneNumber = address.PhoneNumber,
                Street = address.Street,
                City = address.City,
                PostalCode = address.PostalCode,
                Country = address.Country,
                AddressId = address.AddressId
            };
            return Ok(addressDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddNewAddress(AddressDTO newAddress)
        {
            if (newAddress == null)
                return BadRequest();

            Address address = new Address()
            {
                UserId = newAddress.UserId,
                FullName = newAddress.FullName,
                PhoneNumber = newAddress.PhoneNumber,
                Street = newAddress.Street,
                City = newAddress.City,
                PostalCode = newAddress.PostalCode,
                Country = newAddress.Country,
                AddressId = newAddress.AddressId
            };

            await _db.Addresses.AddAsync(address);
            await _db.SaveChangesAsync();
            return Ok(newAddress);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAddress(int AddressID , [FromBody]  AddressDTO UpdatedAddressDTO)
        {
            var AddressToUpdate = await _db.Addresses.SingleOrDefaultAsync(x =>x.AddressId == AddressID);
            if(AddressToUpdate == null)
            {
                return BadRequest();
            }
            AddressToUpdate.UserId = UpdatedAddressDTO.UserId;
            AddressToUpdate.FullName = UpdatedAddressDTO.FullName;
            AddressToUpdate.PhoneNumber = UpdatedAddressDTO.PhoneNumber;
            AddressToUpdate.Street = UpdatedAddressDTO.Street;
            AddressToUpdate.City = UpdatedAddressDTO.City;
            AddressToUpdate.PostalCode = UpdatedAddressDTO.PostalCode;
            AddressToUpdate.Country = UpdatedAddressDTO.Country;
            await _db.SaveChangesAsync();
            return Ok(UpdatedAddressDTO);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAddressById(int AddressID)
        {
            var addressToDelete = await _db.Addresses.SingleOrDefaultAsync(x => x.AddressId == AddressID);
            
            if (addressToDelete == null)
            {
                return NotFound();
            }
            var user = await _db.Users.SingleOrDefaultAsync(x => x.UserId == addressToDelete.UserId);
            if(user.AddressList != null && user.AddressList.Any())
            {
                user.AddressList.Remove(addressToDelete);
            }
            _db.Addresses.Remove(addressToDelete); 
            await _db.SaveChangesAsync();
            return Ok($"{AddressID} has been Deleted");
        }
    }
}
