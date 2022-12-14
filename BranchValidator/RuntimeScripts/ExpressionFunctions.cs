// <copyright file="ExpressionFunctions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable SA1137
#pragma warning disable SA1633
#pragma warning disable SA1600
#pragma warning disable SA1027
#pragma warning disable SA1515
#pragma warning disable CA1050
#pragma warning disable SA1005
// ReSharper disable ArrangeMethodOrOperatorBody
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Global

/*THIS SCRIPT IS AUTO-GENERATED AND SHOULD NOT BE CHANGED MANUALLY*/

using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public enum MatchType
{
	All,
	Start,
	End,
}

public static class ExpressionFunctions
{
	private const char MatchNumbers = '#';
	private const char MatchAnything = '*';
	private static readonly List<string> FunctionResults = new ();
	private const string BranchName = "//<branch-name/>";
	private static readonly char[] LowerCaseLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', };
	private static readonly char[] UpperCaseLetters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', };

    /// <summary>
    /// Gets all of the function results.
    /// </summary>
    /// <returns>The results of all the functions.</returns>
    public static string[] GetFunctionResults()
    {
        return FunctionResults.ToArray();
    }

    /// <summary>
    /// Returns a value indicating whether or not a branch with the given branch name
    /// matches the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to check against the branch name.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> is equal to the branch name.</returns>
    /// <remarks>
    ///     The comparison is case sensitive.
    /// </remarks>
    public static bool EqualTo(string value)
    {
        var branch = string.IsNullOrEmpty(BranchName) ? string.Empty : BranchName;
        value = string.IsNullOrEmpty(value) ? string.Empty : value;

        var hasGlobbingSyntax = value.Contains(MatchNumbers) || value.Contains(MatchAnything);
        var isEqual = hasGlobbingSyntax
            ? Match(branch, value, MatchType.All)
            : (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(branch)) || value == branch;

        RegisterFunctionResult($"{nameof(EqualTo)}({typeof(string)})", isEqual);

        return isEqual;
    }

    /// <summary>
    /// Returns a value indicating whether or not the branch name has all upper case letters.
    /// </summary>
    /// <returns><c>true</c> if all of the letters are uppercase.</returns>
    public static bool AllUpperCase()
    {
        if (string.IsNullOrEmpty(BranchName))
        {
            return false;
        }

        var result = true;

        foreach (var c in BranchName)
        {
            var isLetter = Contains(LowerCaseLetters, c) || Contains(UpperCaseLetters, c);

            // If the character is a symbol, ignore and move on to the next
            if (isLetter && Contains(UpperCaseLetters, c) is false)
            {
                result = false;
                break;
            }
        }

        RegisterFunctionResult($"{nameof(AllUpperCase)}()", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not the branch name has all lower case letters.
    /// </summary>
    /// <returns><c>true</c> if all of the letters are lowercase.</returns>
    public static bool AllLowerCase()
    {
        if (string.IsNullOrEmpty(BranchName))
        {
            return false;
        }

        var result = true;

        foreach (var c in BranchName)
        {
            var isLetter = Contains(LowerCaseLetters, c) || Contains(UpperCaseLetters, c);

            // If the character is a symbol, ignore and move on to the next
            if (isLetter && Contains(LowerCaseLetters, c) is false)
            {
                result = false;
                break;
            }
        }

        RegisterFunctionResult($"{nameof(AllLowerCase)}()", result);

        return result;
    }

    /// <summary>
    /// Registers the given function <paramref name="name"/> and its associated result.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <param name="result">The result of the function.</param>
    private static void RegisterFunctionResult(string name, bool result)
    {
        var newName = $"{name[0].ToString().ToLower()}{name[1..]}";

        // Replace types if they exist
        newName = newName.Replace($"{typeof(int)}", "number");
        newName = newName.Replace($"{typeof(uint)}", "number");
        newName = newName.Replace($"{typeof(string)}", "string");

        FunctionResults.Add($"{newName} -> {result.ToString().ToLower()}");
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="character"/>
    /// is contained in the given <c>string</c> <paramref name="characters"/>.
    /// </summary>
    /// <param name="characters">The <c>string</c> that may or may not contain the character.</param>
    /// <param name="character">The character to check.</param>
    /// <returns>
    ///     <c>true</c> if the given <paramref name="character"/> is
    ///     contained in the <c>string</c> <paramref name="characters"/>.
    /// </returns>
    private static bool Contains(char[] characters, char character)
    {
        foreach (var c in characters)
        {
            if (c == character)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="globbingPattern"/> contains a match
    /// to the given <c>string</c> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <c>string</c> to match.</param>
    /// <param name="globbingPattern">The globbing pattern and text to search.</param>
    /// <param name="matchType">The type of matching that should be performed.</param>
    /// <returns>
    ///     <c>true</c> if the globbing pattern finds a match in the given <c>string</c> <paramref name="value"/>.
    /// </returns>
    private static bool Match(string value, string globbingPattern, MatchType matchType)
    {
        // NOTE: Refer to this website for more regex information -> https://regex101.com/
        const char matchNumbers = '#';
        const char matchAnything = '*';
        const char regexMatchStart = '^';
        const char regexMatchEnd = '$';
        const string regexMatchNumbers = @"\d+";
        const string regexMatchAnything = ".+";

        // Remove any consecutive '#' and '*' symbols until no more consecutive symbols exists anymore
        globbingPattern = RemoveConsecutiveCharacters(new[] { matchNumbers, matchAnything }, globbingPattern);

        // Replace the '#' symbol with
        globbingPattern = globbingPattern.Replace(matchNumbers.ToString(), regexMatchNumbers);

        // Prefix all '.' symbols with '\' to match the '.' literally in regex
        globbingPattern = globbingPattern.Replace(".", @"\.");

        // Replace all '*' character with '.+'
        globbingPattern = globbingPattern.Replace(matchAnything.ToString(), regexMatchAnything);

        if (matchType == MatchType.Start)
        {
            globbingPattern = $"{regexMatchStart}{globbingPattern}";
        }
        else if (matchType == MatchType.End)
        {
            globbingPattern = $"{globbingPattern}{regexMatchEnd}";
        }

        return Regex.Matches(value, globbingPattern, RegexOptions.IgnoreCase).Count > 0;
    }

    /// <summary>
    /// Removes any consecutive occurrences of the given <paramref name="characters"/> from the given <c>string</c> <paramref name="value"/>.
    /// </summary>
    /// <param name="characters">The <c>char</c> to check.</param>
    /// <param name="value">The value that contains the consecutive characters to remove.</param>
    /// <returns>The original <c>string</c> value with the consecutive characters removed.</returns>
    private static string RemoveConsecutiveCharacters(IEnumerable<char> characters, string value)
    {
        foreach (var c in characters)
        {
            while (value.Contains($"{c}{c}"))
            {
                value = value.Replace($"{c}{c}", c.ToString());
            }
        }

        return value;
    }
}

//<expression/>
