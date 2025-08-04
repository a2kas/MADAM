namespace Tamro.Madam.Common.Utils;

public static class UserInfoUtils
{
    public static string GetInitials(string str)
    {
        string[] words = str.Split('.');
        string initials = "";

        foreach (var word in words)
        {
            if (!string.IsNullOrEmpty(word))
            {
                initials += word[0];
            }
        }

        return initials.ToUpper();
    }
}
