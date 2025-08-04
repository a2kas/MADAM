using System.ComponentModel;

namespace Tamro.Madam.Application.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum e)
    {
        var name = e.ToString();
        var memberInfo = e.GetType().GetMember(name)[0];
        var descriptionAttributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
        if (descriptionAttributes.Any())
        {
            return ((DescriptionAttribute)descriptionAttributes[0]).Description;
        }
        return name;
    }
}
