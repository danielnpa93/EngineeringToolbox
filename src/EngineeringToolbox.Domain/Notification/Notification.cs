﻿namespace EngineeringToolbox.Domain.Nofication
{
    public class Notification
    {
        public string Message { get; }

        public Notification(string message)
        {
            Message = message;
        }
    }
}
