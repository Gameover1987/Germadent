using System;

namespace Germadent.Model.Rights
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RightGroupAttribute : Attribute
    {
        public RightGroupAttribute(ApplicationModule applicationModule)
        {
            ApplicationModule = applicationModule;
        }

        public ApplicationModule ApplicationModule { get; set; }
    }


    [AttributeUsage(AttributeTargets.Field)]
    public class ApplicationRightAttribute : Attribute
    {
        public ApplicationRightAttribute(string rightDescription)
        {
            RightDescription = rightDescription;
        }

        public string RightDescription { get; set; }
    }
}