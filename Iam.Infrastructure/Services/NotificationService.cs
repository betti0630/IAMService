using Grpc.Common.Notification;

using Grpc.Net.Client;

using System.Net;

using static Grpc.Common.Notification.NotificationService;
using Iam.Application.Contracts.Notification;

namespace Iam.Infrastructure.Services;

internal sealed class NotificationService : INotificationService
{
    private readonly NotificationServiceClient _client;

    public NotificationService(Uri notificationApiUrl)
    {
        var handler = new HttpClientHandler
        {
            // Ha self-signed cert van (pl. dev Docker környezet)
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var channel = GrpcChannel.ForAddress(notificationApiUrl, new GrpcChannelOptions
        {
            HttpHandler = handler,
            HttpVersion = HttpVersion.Version20,
            HttpVersionPolicy = HttpVersionPolicy.RequestVersionExact
        });

        _client = new NotificationServiceClient(channel);
    }


    public async Task SendRegistrationEmail(int userId, string token, string verificationLink)
    {
        var request = new RegistrationEmailRequest { 
            UserId = userId,
            Token = token,
            VerificationLink = verificationLink
        };
        await _client.SendRegistrationEmailAsync(request);
    }
}
