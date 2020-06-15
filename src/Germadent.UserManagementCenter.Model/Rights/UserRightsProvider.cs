using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Germadent.UserManagementCenter.Model.Rights
{
    public static class UserRightsProvider
    {
        public static RightDto[] GetAllUserRights()
        {
            var rightsAssembly = Assembly.GetAssembly(typeof(UserRightsProvider));
            var rightModules = rightsAssembly.GetTypes().Where(x => x.IsSubclassOf(typeof(UserRightsBase))).ToArray();

            var rights = new List<RightDto>();

            foreach (var rightModule in rightModules)
            {
                var moduleDescription = (DescriptionAttribute) rightModule.GetCustomAttributes(typeof(DescriptionAttribute)).First();
                var rightsByModule = rightModule
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(x => x.IsLiteral)
                    .Select(x =>
                    {
                        var rightDescription = (DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute)).First();
                        return new RightDto
                        {
                            ApplicationName = moduleDescription.Description,
                            RightName = rightDescription.Description
                        };
                    })
                    .ToArray();
                rights.AddRange(rightsByModule);
            }

            return rights.ToArray();
        }
    }
}