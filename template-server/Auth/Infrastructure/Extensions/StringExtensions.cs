using System.Text;

namespace Infrastructure.Extensions;

public static class StringExtensions
{
    public static string? ToSnakeCase(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        StringBuilder builder = new StringBuilder();
        char[] characters = input.ToCharArray();

        builder.Append(char.ToLower(characters[0]));

        for (int i = 1; i < characters.Length; i++)
        {
            if (char.IsUpper(characters[i]))
            {
                builder.Append('_');
                builder.Append(char.ToLower(characters[i]));
            }
            else
            {
                builder.Append(characters[i]);
            }
        }

        return builder.ToString();
    }
}
