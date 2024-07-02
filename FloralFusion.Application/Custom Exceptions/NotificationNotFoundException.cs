﻿using System.Runtime.Serialization;

namespace FloralFusion.Application.Custom_Exceptions
{
    public class NotificationNotFoundException : Exception
    {
        public NotificationNotFoundException()
        {
        }

        public NotificationNotFoundException(string? message) : base(message)
        {
        }

        public NotificationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotificationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
