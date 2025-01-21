namespace WMS.Core.SingalR.Abstractions;

public interface IOrderNotificationHub
{
    Task GetOrderNotification(string notificationMessage);
}