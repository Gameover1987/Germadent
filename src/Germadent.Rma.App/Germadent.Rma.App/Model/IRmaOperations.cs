using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Common;
using Germadent.Common.Extensions;

namespace Germadent.Rma.App.Model
{
    public interface IRmaOperations
    {
        void Authorize(string user, string password);
    }

    public class RmaOperations : IRmaOperations
    {
        public void Authorize(string user, string password)
        {
            if(!user.IsNullOrWhiteSpace() && user == "Admin")
                return;

            throw new UserMessageException("Пользователь не авторизован");
        }
    }
}
