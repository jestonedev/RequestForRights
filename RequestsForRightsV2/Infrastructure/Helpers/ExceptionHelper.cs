﻿using System;

namespace RequestsForRights.Infrastructure.Helpers
{
    public static class ExceptionHelper
    {
        public static Exception RollToInnerException(Exception exception)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            return exception;
        }
    }
}