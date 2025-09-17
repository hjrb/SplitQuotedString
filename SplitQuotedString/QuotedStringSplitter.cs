using System.Text;

namespace HunnyR.Tools;

public static class QuotedStringSplitterExtensions
{
	public static IEnumerable<string> SplitQuoted(
		this string source, HashSet<char> delimiters, HashSet<char>? quoters = null, bool treatConsequticveDelimitersAsOne = false, bool treatTwoQuotesAsLiteral = false
	) => QuotedStringSplitter.Split(source, delimiters, quoters, treatConsequticveDelimitersAsOne, treatTwoQuotesAsLiteral);
}

/// <summary>
/// split strings with support for quoted substrings and multiple delimiters
/// </summary>
public class QuotedStringSplitter
{
	/// <summary>
	/// if set to true (default is false) consequtive delimiters are treated as one e.g. "a,,b" with commma as delimiter will return ["a","b"]
	/// if false "a,,b" will return ["a","","b"] and ",," will return ["","",""]
	/// </summary>
	public bool TreatConsequticveDelimitersAsOne { get; set;} = false;

	/// <summary>
	/// if set to true two consecutive quote characters are treated as a literal quote character 
	/// </summary>
	public bool TreatTwoQuotesAsLiteral { get;set;} =false;

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
	public HashSet<char> Delimiters { get; set;} = [' ', '\t'];

	public static readonly HashSet<char> CsvDelimiters = [',',';','\t'];

	public static IEnumerable<string> Split(
		string source, HashSet<char> delimiters, HashSet<char>? quoters = null, bool treatConsequticveDelimitersAsOne = false, bool treatTwoQuotesAsLiteral = false
	)
	{
		var x = new QuotedStringSplitter()
		{
			TreatConsequticveDelimitersAsOne = treatConsequticveDelimitersAsOne,
			TreatTwoQuotesAsLiteral = treatTwoQuotesAsLiteral,
			Delimiters=delimiters,
		};
		if (quoters != null) x.Quoters = quoters;
		return x.Split(source);
	}


	public IEnumerable<string> Split(
		string source
	)
	{
		// if is debateable if an empty string should return a single empty item or no items
		if (source.Length == 0)
		{
				yield break;
		}
		var currentToken = new StringBuilder();
		var inQuote = false;
		var currentQuote = (char)0;
		char previousChar;
		var i = 0;
		var currentChar=(char)0;
		for (; i < source.Length; i++)
		{
				previousChar=currentChar;
				currentChar = source[i];
				if (Quoters.Contains(currentChar))
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
							if (TreatTwoQuotesAsLiteral && previousChar==currentChar) currentToken.Append(currentChar);
							inQuote = false;
						}
						else
						{
							currentToken.Append(currentChar);
						}
					}
					continue;
				}
				if (Delimiters.Contains(currentChar))
				{
					if (inQuote)
					{
						currentToken.Append(currentChar);
					}
					else
					{
						yield return currentToken.ToString();
						currentToken.Clear();
						if (TreatConsequticveDelimitersAsOne)
						{
								var k = i + 1;
								while (k < source.Length && Delimiters.Contains(source[k])) ++k;
								i = k - 1;
						}
					}
					continue;
				}
				currentToken.Append(currentChar);
		}
		yield return currentToken.ToString();
	}
}