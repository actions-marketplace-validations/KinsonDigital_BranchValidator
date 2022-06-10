﻿// <copyright file="FuncAnalyzerServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="FuncAnalyzerService"/> class.
/// </summary>
public class FuncAnalyzerServiceTests
{
    private readonly Mock<IFunctionService> mockFunctionsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuncAnalyzerServiceTests"/> class.
    /// </summary>
    public FuncAnalyzerServiceTests() => this.mockFunctionsService = new Mock<IFunctionService>();

    public static IEnumerable<object[]> InvalidFuncNameChars()
    {
        yield return new object[] { '~' };
        yield return new object[] { '!' };
        yield return new object[] { '@' };
        yield return new object[] { '#' };
        yield return new object[] { '$' };
        yield return new object[] { '%' };
        yield return new object[] { '^' };
        yield return new object[] { '&' };
        yield return new object[] { '*' };
        yield return new object[] { '_' };
        yield return new object[] { '+' };
        yield return new object[] { '{' };
        yield return new object[] { '}' };
        yield return new object[] { '|' };
        yield return new object[] { ':' };
        yield return new object[] { '"' };
        yield return new object[] { '<' };
        yield return new object[] { '>' };
        yield return new object[] { '?' };
        yield return new object[] { '`' };
        yield return new object[] { '-' };
        yield return new object[] { '=' };
        yield return new object[] { '[' };
        yield return new object[] { ']' };
        yield return new object[] { '\\' };
        yield return new object[] { ';' };
        yield return new object[] { '\'' };
        yield return new object[] { '.' };
        yield return new object[] { '/' };
        yield return new object[] { ',' };
        yield return new object[] { '0' };
        yield return new object[] { '1' };
        yield return new object[] { '2' };
        yield return new object[] { '3' };
        yield return new object[] { '4' };
        yield return new object[] { '5' };
        yield return new object[] { '6' };
        yield return new object[] { '7' };
        yield return new object[] { '8' };
        yield return new object[] { '9' };
    }

    #region Method Tests
    [Theory]
    [MemberData(nameof(InvalidFuncNameChars))]
    public void Analyze_WhenFuncNameDoesNotContainLetter_ReturnsCorrectResult(char invalidChar)
    {
        // Arrange
        var expression = $"myF{invalidChar}unc()";
        var service = CreateService();

        // Act
        var actual = service.Analyze(expression);

        // Assert
        actual.valid.Should().Be(false);
        actual.msg.Should().Be("The name of a function can only contain lower or upper case letters.");
    }

    [Theory]
    [InlineData(null, false, "The expression must have a single valid function.")]
    [InlineData("", false, "The expression must have a single valid function.")]
    [InlineData("myFunc)", false, "The function signature is missing a '('.")]
    [InlineData("myFunc(", false, "The function signature is missing a ')'.")]
    [InlineData("myFu()nc", false, "The expression must end with a ')'.")]
    [InlineData("myFunc(()", false, "The function signature has too many '(' symbols.  A function signature can only contain a single '('.")]
    [InlineData("myFunc())", false, "The function signature has too many ')' symbols.  A function signature can only contain a single ')'.")]
    public void Analyze_WhenInvoked_ReturnsCorrectResult(
        string expression,
        bool expectedValidResult,
        string expectedMsgResult)
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.Analyze(expression);

        // Assert
        actual.valid.Should().Be(expectedValidResult);
        actual.msg.Should().Be(expectedMsgResult);
    }

    [Theory]
    [InlineData("notExistFunc()", false, 1, "The function 'notExistFunc' is not a valid function that can be used.")]
    [InlineData("funA()", false, 1, "The 'funA' function does not contain an argument.")]
    [InlineData("funA(123)", false, 1, "Parameter '1' for function 'funA' must be a string data type.")]
    [InlineData("funB('test')", false, 1, "Parameter '1' for function 'funB' must be a number data type.")]
    [InlineData("funB(12test34)", false, 1, "Parameter '1' for function 'funB' must be a number data type.")]
    [InlineData(
        "funA('test')",
        false,
        2,
        $"Incorrect number of function arguments.\r\nThe function 'funA' currently has '1' parameters but is expecting '2'.")]
    [InlineData("funA('test')", true, 1, "")]
    [InlineData("funA('123')", true, 1, "")]
    [InlineData("funB(123)", true, 1, "")]
    public void Analyze_WhenCheckingFunctionNames_ReturnsCorrectResult(
        string function,
        bool expectedValid,
        uint totalFuncParams,
        string expectedMsg)
    {
        // Arrange
        this.mockFunctionsService.SetupGet(p => p.FunctionNames)
            .Returns(new[] { "funA", "funB" }.ToReadOnlyCollection);
        this.mockFunctionsService.Setup(m => m.GetTotalFunctionParams("funA")).Returns(totalFuncParams);
        this.mockFunctionsService.Setup(m => m.GetTotalFunctionParams("funB")).Returns(totalFuncParams);
        this.mockFunctionsService.Setup(m => m.GetFunctionParamDataType("funA", 1))
            .Returns(DataTypes.String);
        this.mockFunctionsService.Setup(m => m.GetFunctionParamDataType("funB", 1))
            .Returns(DataTypes.Number);

        var service = CreateService();

        // Act
        var actual = service.Analyze(function);

        // Assert
        actual.valid.Should().Be(expectedValid, "the function name or signature is invalid");
        actual.msg.Should().Be(expectedMsg);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="FuncAnalyzerService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private FuncAnalyzerService CreateService() => new (this.mockFunctionsService.Object);
}
