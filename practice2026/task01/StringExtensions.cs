using System.Text;

namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        var cleaned = new StringBuilder();
        foreach (char c in input.ToLower())
        {
            if (!char.IsPunctuation(c) && !char.IsWhiteSpace(c))
            {
                cleaned.Append(c);
            }
        }

        string result = cleaned.ToString();
        string reversed = new string(result.Reverse().ToArray());

        return result == reversed;
        // test CI
    }
}
