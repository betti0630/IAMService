namespace Iam.Application.Contracts.Notification;

public interface INotificationService
{
    Task SendRegistrationEmail(int userId, string token, string verificationLink);
}
