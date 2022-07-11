using System.ComponentModel.DataAnnotations;

namespace CLOFT.SerenUp.WebApp.Services
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class UserNameControlAttribute : ValidationAttribute
    {
        List<string> userNames = new List<string>();

        public UserNameControlAttribute(List<string> userNames)
        {
            this.userNames = userNames;
        }

        public override bool IsValid(object? value)
        {
            if (userNames.Contains(value))
                return false;

            return base.IsValid(value);
        }

    }
}
