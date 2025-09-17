using System.Text;

namespace HunnyR.Tools;

/// <summary>
/// yet another string splitter
/// </summary>
public static class StringSplit
{
	public static readonly char[] DefaultQuoteChars = ['"', '\''];
	public static readonly char[] DefaultDelimiters = [' ', '\t'];
	/// <inheritdoc cref="GetTokens(System.ReadOnlySpan{char}, bool, System.ReadOnlySpan{char}, System.ReadOnlySpan{char})" />
	public static IEnumerable<string> GetTokens(
		string source,
		bool treatConsequticveDelimitersAsOne = false,
		char[]? quoters = null,
		char[]? delimiters = null
	) =>
		GetTokens(source: source, treatConsequticveDelimitersAsOne: treatConsequticveDelimitersAsOne, quoters: (quoters ?? DefaultQuoteChars).AsSpan(), delimiters: (delimiters ?? DefaultDelimiters).AsSpan());

	/// <inheritdoc cref="GetTokens(System.ReadOnlySpan{char}, bool, System.ReadOnlySpan{char}, System.ReadOnlySpan{char})" />
	public static IEnumerable<string> GetTokens(
		ReadOnlySpan<char> source,
		bool treatConsequticveDelimitersAsOne = false
	) => GetTokens(source, treatConsequticveDelimitersAsOne, DefaultQuoteChars.AsSpan(), DefaultDelimiters.AsSpan());


	/// <summary>
	/// Splits a character span into tokens, honoring quoted substrings. Delimiters that occur inside a matching quote pair
	/// are treated as literal characters and not as token separators. Quote characters are not included in the returned tokens.
	/// </summary>
	/// <param name="source">The source span to parse.</param>
	/// <param name="treatConsequticveDelimitersAsOne">If true, empty tokens (adjacent delimiters or leading/trailing delimiters) are included; otherwise they are skipped.</param>
	/// <param name="quoters">Set of quote characters. When null, defaults to single quote (') and double quote (").</param>
	/// <param name="delimiters">Set of delimiter (separator) characters. When null, defaults to space and tab.</param>
	/// <returns>An enumerable of parsed tokens in order of appearance.</returns>
	public static IEnumerable<string> GetTokens(
		ReadOnlySpan<char> source,
		bool treatConsequticveDelimitersAsOne,
		ReadOnlySpan<char> quoters,
		ReadOnlySpan<char> delimiters
	 )
	 {
		if (source.Length == 0)
		{
			return [];
		}
		var tokens = new List<string>(source.Length / 2); // rough estimate
		var currentToken = new StringBuilder();
		var inQuote = false;
		var currentQuote = (char)0;
		var i = 0;
		for (; i < source.Length; i++)
		{
			var c = source[i];
			//  quote character ?
			if (quoters.Contains(c))
			{
			// if we are not in quote set quote to true and remember quote character
				if (!inQuote)
				{
					currentQuote = c;
					inQuote = true;
				}
				else
				{
					// if the same quote end of quote reached
					if (currentQuote == c)
					{
						inQuote = !inQuote;
					}
					else
					{
						// apppend to current token
						currentToken.Append(c);
					}
				}
				continue;
			}

			// delimiter?
			if (delimiters.Contains(c))
			{
				if (inQuote)
				{
					// treat delimiter as normal character if in quote
					currentToken.Append(c);
				}
				else
				{
					tokens.Add(currentToken.ToString());
					currentToken.Clear();
					if (treatConsequticveDelimitersAsOne)
					{
						var k = i + 1;
						while (k < source.Length && delimiters.Contains(source[k])) ++k;
						i = k - 1;
					}
				}
				continue;
			}
			// regular character: add to current token
			currentToken.Append(c);
		}
		tokens.Add(currentToken.ToString());
		return tokens;
	 }
}