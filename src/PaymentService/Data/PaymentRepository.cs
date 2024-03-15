using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaymentService.Entities;

namespace PaymentService.Data;

public class PaymentRepository : IPaymentRepository
{
	private readonly AppDbContext _context;
	private readonly IMapper _mapper;

	public PaymentRepository(AppDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public void AddPayment(Payment payment)
	{
		_context.Payments.Add(payment);
	}

	public async Task<Payment> GetPaymentByIdAsync(Guid id)
	{
		return await _context.Payments.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<bool> SaveChangesAsync()
	{
		return (await _context.SaveChangesAsync()) > 0;
	}

	public void RemovePayment(Payment payment)
	{
		_context.Payments.Remove(payment);
	}

	public async Task<List<Payment>> GetAllPayments()
	{
		return await _context.Payments.ToListAsync();
	}

	public void UpdatePayment(Payment payment)
	{
		_context.Payments.Update(payment);
	}
}