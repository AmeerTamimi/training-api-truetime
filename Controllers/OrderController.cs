using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training_API.Data;
using Training_API.DTOS;
using Training_API.Models;

namespace Training_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MyAppContext _db;

        public OrderController(MyAppContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _db.Orders
                .Include(x => x.Payments)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .ToListAsync();

            var orderDTOs = new List<OrderDTO>();
            foreach (var order in orders)
            {
                var orderDTO = new OrderDTO
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    orderItems = new List<OrderDetailsDTO>(),
                    Payments = new List<PaymentDTO>()
                };

                foreach (var od in order.OrderDetails ?? Enumerable.Empty<OrderDetails>())
                {
                    orderDTO.orderItems.Add(new OrderDetailsDTO
                    {
                        ProductId = od.ProductId,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice,
                    });
                }

                foreach (var payment in order.Payments ?? Enumerable.Empty<Payment>())
                {
                    orderDTO.Payments.Add(new PaymentDTO
                    {
                        PaymentId = payment.PaymentId,
                        OrderId = payment.OrderId,
                        PaymentDate = payment.PaymentDate,
                        PaymentMethod = payment.PaymentMethod,
                        AmountPaid = payment.AmountPaid,
                        IsConfirmed = payment.IsConfirmed,
                    });
                }

                orderDTOs.Add(orderDTO);
            }

            return Ok(orderDTOs);
        }


        [HttpGet("{OrderID}")]
        public async Task<IActionResult> GetOrders(int OrderID)
        {
            var order = await _db.Orders
               .Include(x => x.Payments)
               .Include(x => x.OrderDetails)
               .ThenInclude(x => x.Product)
               .SingleOrDefaultAsync(x => x.OrderId == OrderID);

            var OrderDTO = new OrderDTO()
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                orderItems = new List<OrderDetailsDTO>(),
                Payments = new List<PaymentDTO>()
            };

            List<OrderDetailsDTO> orderDetailsDTOs = new List<OrderDetailsDTO>();

            foreach (var od in order.OrderDetails)
            {
                OrderDetailsDTO orderDetailsDTO = new OrderDetailsDTO()
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                };
                orderDetailsDTOs.Add(orderDetailsDTO);
            }

            List<PaymentDTO> paymentDTOs = new List<PaymentDTO>();
            foreach (var payment in order.Payments)
            {
                var paymentDTO = new PaymentDTO()
                {
                    PaymentId = payment.PaymentId,
                    OrderId = payment.OrderId,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.PaymentMethod,
                    AmountPaid = payment.AmountPaid,
                    IsConfirmed = payment.IsConfirmed,
                };
                paymentDTOs.Add(paymentDTO);
            }
            OrderDTO.Payments = paymentDTOs;
            OrderDTO.orderItems = orderDetailsDTOs;

            return Ok(OrderDTO);
        }



        [HttpPost]
        public async Task<IActionResult> AddNewOrder([FromBody] OrderDTO newOrder)
        {
            var orderToAdd = new Order()
            {
                OrderId = newOrder.OrderId,
                UserId = newOrder.UserId,
                OrderDate = newOrder.OrderDate,
                TotalAmount = newOrder.TotalAmount,
                OrderDetails = new List<OrderDetails>(),
                Payments = new List<Payment>()
            };

            List<OrderDetails> orderDetails = new List<OrderDetails>();
            if (newOrder.orderItems != null)
            {
                foreach (var odDTO in newOrder.orderItems)
                {
                    OrderDetails od = new OrderDetails()
                    {
                        ProductId = odDTO.ProductId,
                        Quantity = odDTO.Quantity,
                        UnitPrice = odDTO.UnitPrice,
                    };
                    orderDetails.Add(od);
                }
            }

            List<Payment> payments = new List<Payment>();
            if (newOrder.Payments != null)
            {
                foreach (var paymentDTO in newOrder.Payments)
                {
                    Payment payment = new Payment()
                    {
                        PaymentId = paymentDTO.PaymentId,
                        OrderId = paymentDTO.OrderId,
                        PaymentDate = paymentDTO.PaymentDate,
                        PaymentMethod = paymentDTO.PaymentMethod,
                        AmountPaid = paymentDTO.AmountPaid,
                        IsConfirmed = paymentDTO.IsConfirmed,
                    };
                    payments.Add(payment);
                }
                orderToAdd.Payments = payments;
                orderToAdd.OrderDetails = orderDetails;
            }
            await _db.Orders.AddAsync(orderToAdd);
            await _db.SaveChangesAsync();
            return Ok(newOrder);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateOrder(int OrderID, [FromBody] OrderDTO UpdatedOrder)
        {
            var orderToUpdate = await _db.Orders
                        .SingleOrDefaultAsync(x => x.OrderId == OrderID);

            if (orderToUpdate != null)
            {
                orderToUpdate.UserId = UpdatedOrder.UserId;
                orderToUpdate.OrderDate = UpdatedOrder.OrderDate;
                orderToUpdate.TotalAmount = UpdatedOrder.TotalAmount;
                orderToUpdate.OrderDetails.Clear();
                orderToUpdate.Payments.Clear();

                List<OrderDetails> orderDetails = new List<OrderDetails>();
                if (UpdatedOrder.orderItems != null)
                {
                    foreach (var odDTO in UpdatedOrder.orderItems)
                    {
                        OrderDetails od = new OrderDetails()
                        {
                            ProductId = odDTO.ProductId,
                            Quantity = odDTO.Quantity,
                            UnitPrice = odDTO.UnitPrice,
                        };
                        orderDetails.Add(od);
                    }
                }

                List<Payment> payments = new List<Payment>();
                if (UpdatedOrder.Payments != null)
                {
                    foreach (var paymentDTO in UpdatedOrder.Payments)
                    {
                        Payment payment = new Payment()
                        {
                            PaymentId = paymentDTO.PaymentId,
                            OrderId = paymentDTO.OrderId,
                            PaymentDate = paymentDTO.PaymentDate,
                            PaymentMethod = paymentDTO.PaymentMethod,
                            AmountPaid = paymentDTO.AmountPaid,
                            IsConfirmed = paymentDTO.IsConfirmed,
                        };
                        payments.Add(payment);
                    }
                    orderToUpdate.Payments = payments;
                    orderToUpdate.OrderDetails = orderDetails;
                }
                await _db.SaveChangesAsync();
                return Ok(UpdatedOrder);
            }
            return NotFound();
        }

        [HttpDelete("{OrderID}")]
        public async Task<IActionResult> DeleteOrder(int OrderID)
        {
            var orderToDelete = await _db.Orders
                .Include(x => x.Payments)
                .SingleOrDefaultAsync(o => o.OrderId == OrderID);

            if (orderToDelete == null)
            {
                return NotFound();
            }

            // Remove associated payments
            if (orderToDelete.Payments != null && orderToDelete.Payments.Any())
            {
                _db.Payments.RemoveRange(orderToDelete.Payments);
            }

            // Remove the order itself
            _db.Orders.Remove(orderToDelete);
            await _db.SaveChangesAsync();

            return Ok($"Order with ID {OrderID} has been deleted!");
        }
    }
}
