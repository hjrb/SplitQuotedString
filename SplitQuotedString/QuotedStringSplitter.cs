using System.Text;

namespace HunnyR.Tools;

public static class QuotedStringSplitterExtensions
{
	public static IEnumerable<string> SplitQuoted(
		this string source, HashSet<char> delimiters, HashSet<char>? quoters = null, bool treatConsecutiveDelimitersAsOne = false, bool treatTwoQuotesAsLiteral = false
	) => QuotedStringSplitter.Split(source, delimiters, quoters, treatConsecutiveDelimitersAsOne, treatTwoQuotesAsLiteral);
}

/// <summary>
/// split strings with support for quoted substrings and multiple delimiters
/// </summary>
public class QuotedStringSplitter
{
	/// <summary>
	/// if set to true (default is false) consecutive delimiters are treated as one e.g. "a,,b" with comma as delimiter will return ["a","b"]
	/// if false "a,,b" will return ["a","","b"] and ",," will return ["","",""]
	/// </summary>
	public bool TreatConsecutiveDelimitersAsOne { get; set; } = false;

	/// <summary>
	/// if set to true two consecutive quote characters are treated as a literal quote character 
	/// </summary>
	public bool TreatTwoQuotesAsLiteral { get; set; } = false;

	/// <summary>
	/// quote characters
	/// default are double quote(") and single quote (')
	/// delimiter or quote characters inside a quoted string are treated as normal characters
	/// can be empty
	/// '"a.b"' will return ["a.b"] not matter what delimiters
	/// ''a.b'' will return ["a","b"] if . is the delimiter and TreatTwoQuotesAsLiteral as literal is false else it will return ["'a.b'"]
	/// </summary>
	public HashSet<char> Quoters { get; set; } = ['"', '\''];

	/// <summary>
	/// delimiters
	/// default is space and tab
	/// should contain at least one character. if empty will return the full string as single token
	/// </summary>
	public HashSet<char> Delimiters { get; set; } = [' ', '\t'];

	public static readonly HashSet<char> CsvDelimiters = [',', ';', '\t'];

	public static IEnumerable<string> Split(
		string source, HashSet<char> delimiters, HashSet<char>? quoters = null, bool treatConsecutiveDelimitersAsOne = false, bool treatTwoQuotesAsLiteral = false
	)
	{
		var x = new QuotedStringSplitter()
		{
				TreatConsecutiveDelimitersAsOne = treatConsecutiveDelimitersAsOne,
				TreatTwoQuotesAsLiteral = treatTwoQuotesAsLiteral,
				Delimiters = delimiters,
		};
		if (quoters != null)
		{
			x.Quoters = quoters;
		}

		return x.Split(source);
	}


	public IEnumerable<string> Split(
		string source
	)
	{
		// if is debatable if an empty string should return a single empty item or no items
		if (source.Length == 0)
		{
				yield break;
		}
		var currentToken = new StringBuilder();
		var inQuote = false;
		var currentQuote = (char)0;
		char previousChar;
		var currentChar = (char)0;
		var i = 0;
		for (; i < source.Length; i++)
		{
				previousChar = currentChar; 
				currentChar = source[i];

				// handle quoters
				if (this.Quoters.Contains(currentChar))
				{
					if (!inQuote)
					{
						currentQuote = currentChar;
						inQuote = true;
					}
					else
					{
						if (currentQuote == currentChar)
						{
							inQuote = false;
							if (this.TreatTwoQuotesAsLiteral && previousChar == currentChar)
							{
								_ = currentToken.Append(currentChar);
							}
						}
						else
						{
							_ = currentToken.Append(currentChar);
						}
					}
					continue;
				}

				// handle delimters
				if (this.Delimiters.Contains(currentChar))
				{
					if (inQuote)
					{
					_ = currentToken.Append(currentChar);
					}
					else
					{
						yield return currentToken.ToString();
						_ = currentToken.Clear();
						if (this.TreatConsecutiveDelimitersAsOne)
						{
								var k = i + 1;
								while (k < source.Length && this.Delimiters.Contains(source[k]))
						{
							++k;
						}

						i = k - 1;
						}
					}
					continue;
				}
			// ordanary 
			_ = currentToken.Append(currentChar);
		}
		// this will sometimes return an empty token at the end. This is on purpose! e.g. "a,b," with comma as delimiter will return ["a","b",""]
		yield return currentToken.ToString();
	}

}