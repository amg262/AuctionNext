using PaymentService.Entities;

namespace PaymentService.Data;

/// <summary>
/// Defines the interface for payment repository operations,
/// allowing for abstraction over specific data access mechanisms.
/// </summary>
public interface IPaymentRepository
{
	/// <summary>
	/// Adds a new payment to the repository.
	/// </summary>
	/// <param name="payment">The payment entity to add.</param>
	void AddPayment(Payment payment);

	/// <summary>
	/// Retrieves a payment by its unique identifier.
	/// </summary>
	/// <param name="id">The GUID of the payment to retrieve.</param>
	/// <returns>The payment entity found, or null if no payment with the given id exists.</returns>
	Task<Payment> GetPaymentByIdAsync(Guid id);

	/// <summary>
	/// Saves all changes made in the context to the database.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a boolean value that indicates if any changes were saved to the database successfully.</returns>
	Task<bool> SaveChangesAsync();

	/// <summary>
	/// Removes a payment from the repository.
	/// </summary>
	/// <param name="payment">The payment entity to remove.</param>
	void RemovePayment(Payment payment);

	/// <summary>
	/// Retrieves all payments from the repository.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of payment entities.</returns>
	Task<List<Payment>> GetAllPayments();

	/// <summary>
	/// Updates an existing payment in the repository.
	/// </summary>
	/// <param name="payment">The payment entity to update.</param>
	void UpdatePayment(Payment payment);
}