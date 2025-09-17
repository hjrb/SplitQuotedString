using System.Runtime.CompilerServices;
using System.Text;

namespace HunnyR.Tools;

/// <summary>
/// yet another string splitter
/// </summary>
public class StringSplit
{
	 public bool TreatConsequticveDelimitersAsOne { get; set;} = false;
	 public char[] Quoters { get; set; } = ['"', '\''];
	 public char[] Delimiters { get; set;} = [' ', '\t'];

	 
	 public IEnumerable<string> GetTokens(
		string source
	 )
	 {
		  if (source.Length == 0)
		  {
				yield break;
		  }
		  var currentToken = new StringBuilder();
		  var inQuote = false;
		  var currentQuote = (char)0;
		  var i = 0;
		  for (; i < source.Length; i++)
		  {
				var c = source[i];
				if (Quoters.Contains(c))
				{
					 if (!inQuote)
					 {
						  currentQuote = c;
						  inQuote = true;
					 }
					 else
					 {
						  if (currentQuote == c)
						  {
								inQuote = false;
						  }
						  else
						  {
								currentToken.Append(c);
						  }
					 }
					 continue;
				}
				if (Delimiters.Contains(c))
				{
					 if (inQuote)
					 {
						  currentToken.Append(c);
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
				currentToken.Append(c);
		  }
		  yield return currentToken.ToString();
	 }
}