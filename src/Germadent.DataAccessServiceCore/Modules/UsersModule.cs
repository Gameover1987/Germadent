using System;
using Germadent.Common.Logging;
using Nancy;

namespace Germadent.DataAccessServiceCore.Modules
{
    public class UsersModule : NancyModule
    {
        private ILogger _logger;

        public UsersModule(ILogger logger)
        {
            _logger = logger;

            ModulePath = "api/Umc";

            Get["/users"] = o => GetUsers();
        }

        private object GetUsers()
        {
            return ExecuteWithLogging(() => { return null; });
        }

        private object GetRoles()
        {
            return ExecuteWithLogging(() => { return null; });
        }

        private object GetRights()
        {
            return ExecuteWithLogging(() => { return null; });
        }

        private object ExecuteWithLogging(Func<object> func)
        {
            try
            {
                return func();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }

            return null;
        }
    }
}
