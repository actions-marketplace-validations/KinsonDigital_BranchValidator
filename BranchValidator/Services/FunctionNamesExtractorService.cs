﻿// <copyright file="FunctionNamesExtractorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Dynamic;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class FunctionNamesExtractorService : IFunctionNamesExtractorService
{
    private const string AndOperator = "&&";
    private const string OrOperator = "||";
    private const char LeftParen = '(';

    private readonly IExpressionValidatorService expressionValidatorService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionNamesExtractorService"/> class.
    /// </summary>
    /// <param name="expressionValidatorService"></param>
    public FunctionNamesExtractorService(IExpressionValidatorService expressionValidatorService)
        => this.expressionValidatorService = expressionValidatorService;

    /// <inheritdoc/>
    public IEnumerable<string> ExtractNames(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Array.Empty<string>();
        }

        if (this.expressionValidatorService.Validate(expression).isValid is false)
        {
            return Array.Empty<string>();
        }


        const StringSplitOptions splitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
        var doesNotContainAnyOperators = expression.Contains(AndOperator) is false && expression.Contains(OrOperator) is false;

        // If no operators exist, then it is a single function expression
        if (doesNotContainAnyOperators)
        {
            return new[] { expression.Split(LeftParen, splitOptions)[0] };
        }

        var funcNames = new List<string>();

        var andSections = expression.Split(AndOperator, splitOptions);

        foreach (var section in andSections)
        {
            if (section.Contains(OrOperator))
            {
                var orSections = section.Split(OrOperator, splitOptions);

                foreach (var orSection in orSections)
                {
                    var sectionSplit = orSection.Split(LeftParen, splitOptions);

                    funcNames.Add(sectionSplit[0]);
                }
            }
            else
            {
                var sectionSplit = section.Split(LeftParen, splitOptions);

                funcNames.Add(sectionSplit[0]);
            }
        }

        return funcNames.ToArray();
    }
}
