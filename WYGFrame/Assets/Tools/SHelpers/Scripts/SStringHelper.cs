using System.Text.RegularExpressions;

public static class SStringHelper
{
    private static string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";//判断邮箱格式的正则表达式判断条件
    
    /// <summary>
    /// 判断string是否符合邮箱格式
    /// </summary>
    /// <param name="email">string内容</param>
    /// <returns>是否符合</returns>
    public static bool IfEmail(this string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase); ;
    }

    /// <summary>
    /// 判断string是否符合电话号码格式
    /// </summary>
    /// <param name="phoneNumber">string内容</param>
    /// <returns>是否符合</returns>
    public static bool IfPhoneNumber(this string phoneNumber) 
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return false;

        if(phoneNumber.Length < 7 ||  phoneNumber.Length > 11)
            return false;

        return !Regex.IsMatch(phoneNumber, @"[^0-9]"); ;
    }
}
