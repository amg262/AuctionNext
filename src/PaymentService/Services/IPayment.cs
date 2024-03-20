namespace PaymentService.Services;

/// <summary>
/// Eventually, we will have multiple payment methods like PayPal, Stripe, etc.
/// So, we will create an interface for the payment service to use strategy pattern to switch between payment methods.
/// </summary>
public interface IPayment
{
}