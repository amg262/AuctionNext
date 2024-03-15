using PaymentService.Entities;

namespace PaymentService.Data;

public interface IPaymentRepository
{
	void AddPayment(Payment payment);
	Task<Payment> GetPaymentByIdAsync(Guid id);
	Task<bool> SaveChangesAsync();
	void RemovePayment(Payment payment);
	Task<List<Payment>> GetAllPayments();
	void UpdatePayment(Payment payment);
}