using System;

namespace Germadent.WebApi.DataAccess.UserManagement
{
    public class UserNotAuthorizedException : Exception
    {
        public UserNotAuthorizedException(string message) 
            : base(message)
        {
            
        }
    }
}