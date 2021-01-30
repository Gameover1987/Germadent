using System;
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
                var rightGroupAttribute = (RightGroupAttribute) rightModule.GetCustomAttributes(typeof(RightGroupAttribute)).First();
                var rightsByModule = rightModule
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(x => x.IsLiteral)
                    .Select(x =>
                    {
                        var applicationRightAttribute = (ApplicationRightAttribute)x.GetCustomAttributes(typeof(ApplicationRightAttribute)).First();

                        var fieldValue = x.GetValue(Activator.CreateInstance(rightModule)).ToString();

                        return new RightDto
                        {
                            ApplicationModule = rightGroupAttribute.ApplicationModule,
                            RightDescription = applicationRightAttribute.RightDescription,
                            RightName = fieldValue
                        };
                    })
                    .ToArray();
                rights.AddRange(rightsByModule);
            }

            return rights.ToArray();
        }
    }
}