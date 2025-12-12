using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training_API.Data;
using Training_API.DTOS;
using Training_API.Models;

namespace Training_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly MyAppContext _db;

        public PaymentController(MyAppContext context)
        {
            _db = context;
        }
        private bool isValidPayment(PaymentDTO payment)
        {
            return payment != null;
        }

        [HttpGet]
        public async Task<IActionResult> getPayments()
        {
            var payments = await _db.Payments.ToArrayAsync();

            List<PaymentDTO> dTOs = new List<PaymentDTO>();
            foreach (var p in payments) 
            {
                dTOs.Add(new PaymentDTO
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = p.PaymentMethod, 
                    AmountPaid = p.AmountPaid,
                    IsConfirmed = p.IsConfirmed,
                });
            }
            return Ok(dTOs);
        }

        [HttpGet("{PaymentID}")]
        public async Task<IActionResult> AddNewPayment(int PaymentID)
        {
            var payment = await _db.Payments.SingleOrDefaultAsync
                (x => x.PaymentId == PaymentID);
            if(payment == null)
            {
                return NotFound();
            }
            return Ok(new PaymentDTO()
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod,
                AmountPaid = payment.AmountPaid,
                IsConfirmed = payment.IsConfirmed,
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPayment([FromBody] PaymentDTO newPaymentDTO)
        {
            if (isValidPayment(newPaymentDTO))
            {
                Payment payment = new Payment()
                {
                    PaymentId = newPaymentDTO.PaymentId,
                    OrderId = newPaymentDTO.OrderId,
                    PaymentDate = newPaymentDTO.PaymentDate,
                    PaymentMethod = newPaymentDTO.PaymentMethod,
                    AmountPaid = newPaymentDTO.AmountPaid,
                    IsConfirmed = newPaymentDTO.IsConfirmed,
                    Order = null
                };
                await _db.Payments.AddAsync(payment);
                await _db.SaveChangesAsync();
                return Ok(newPaymentDTO);
            }
            return BadRequest();
        }


        [HttpPut]
        public async Task<IActionResult> UpdatePayment(int PaymentId , [FromBody]PaymentDTO updatedPayemntDTO)
        {
            var paymentToUpdate = await _db.Payments.SingleOrDefaultAsync
                (x => x.PaymentId == PaymentId);

            if (isValidPayment(updatedPayemntDTO) && paymentToUpdate != null)
            {
                paymentToUpdate.PaymentDate = updatedPayemntDTO.PaymentDate;
                paymentToUpdate.OrderId = updatedPayemntDTO.OrderId;
                paymentToUpdate.AmountPaid = updatedPayemntDTO.AmountPaid;
                paymentToUpdate.IsConfirmed = updatedPayemntDTO.IsConfirmed;
                paymentToUpdate.PaymentMethod = updatedPayemntDTO.PaymentMethod;
                await _db.SaveChangesAsync();
                return Ok(updatedPayemntDTO);
            }
            return NotFound();
        }


        [HttpDelete]
        public async Task<IActionResult> DeletePaymentById(int PaymentID)
        {
            var PaymentToDelete = await _db.Payments.
                SingleOrDefaultAsync(x => x.PaymentId == PaymentID);

            if(PaymentToDelete != null) 
            { 
                _db.Payments.Remove(PaymentToDelete);
                _db.SaveChanges();
                return Ok($"Payment With ID {PaymentID} Has Been Deleted !");
            }
            return NotFound();


        }

    }
}
