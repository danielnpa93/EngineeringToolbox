using FluentValidation.Results;

namespace EngineeringToolbox.Domain.Nofication
{
    public class NotificationContext
    {
        private readonly List<Notification> _notifications;
        public IReadOnlyCollection<Notification> Notifications => _notifications;
        public bool HasNotifications => _notifications.Any();
        public StatusCode Status { get; private set; }

        public NotificationContext()
        {
            Status = StatusCode.BadRequest;
            _notifications = new List<Notification>();
        }

        public void AddNotification(string message)
        {
            _notifications.Add(new Notification(message));
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotifications(IEnumerable<string> messages)
        {
            var notifications = new List<Notification>();

            foreach (var m in messages)
            {
                notifications.Add(new Notification(m));
            }

            _notifications.AddRange(notifications);
        }

        public void AddNotifications(IReadOnlyCollection<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(IList<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(ICollection<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddNotification(error.ErrorMessage);
            }
        }

        public void SetStatusCode(StatusCode statusCode)
        {
            Status = statusCode;
        }
    }
}
