using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hr.Tools;

/// <summary>
/// yet another string splitter
/// </summary>
public static class StringSplit
{
	public static readonly char[] DefaultQuoteChars = ['"', '\''];
	public static readonly char[] DefaultDelimiters = [' ', '\t'];
	/// <summary>
	/// splits a string into tokens, honoring quoted substrings
	/// </summary>
	/// <param name="source">the string to split</param>
	/// <param name="includeEmpty"></param>
	/// <param name="quotechars">a set of quote characters, if null the default set is single quote (') and double quote (")</param>
	/// <param name="delimiters">a set of delimiter (separator) characters</param>. if null the default is space ( ) and tabulator (\t)
	/// <returns></returns>
	public static IEnumerable<string> GetTokens(
			string source,
			bool includeEmpty = false,
			char[]? quotechars = null,
			char[]? delimiters = null)
	{
		quotechars ??= DefaultQuoteChars;
		delimiters ??= DefaultDelimiters;

		var currentToken = new StringBuilder();
		var inQuote = false;
		var currentQuote = (char)0;
		bool shouldReturn() => includeEmpty || currentToken.Length > 0;
		
		foreach (var c in source)
		{
			// did we find a quote
			if (quotechars.Contains(c))
			{
				if (inQuote)
				{
					if (currentQuote == c)
					{
						// close quote by returning token after empty check
						if (shouldReturn())
						{
							yield return currentToken.ToString(); // scannedTokens.Add(currentToken.ToString());
						}
						currentToken.Clear();
						// toggle inquote
						inQuote = !inQuote;
					}
					else
					{
						currentToken.Append(c);
					}
				}
				else
				{
					currentQuote = c;
					inQuote = true;
				}
			}

			// did we find a delimiter
			else if (delimiters.Contains(c))
			{
				// are we NOT inside a quote
				if (!inQuote)
				{
					// return token after empty check
					if (shouldReturn())
					{
						yield return currentToken.ToString();
					}
					currentToken.Clear();
				}
				else
				{
					// add to current token
					currentToken.Append(c);
				}
			}
			else
			{
				currentToken.Append(c);
			}
		}
		// handle end of loop
		if (shouldReturn())
		{
			yield return currentToken.ToString();
		}
	}
}