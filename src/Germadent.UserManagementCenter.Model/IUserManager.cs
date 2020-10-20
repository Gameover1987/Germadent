using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.UserManagementCenter.Model
{
    public interface IUserManager
    {
        bool HasRight(string rightName);
    }
}