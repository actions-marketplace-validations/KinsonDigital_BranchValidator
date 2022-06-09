﻿// <copyright file="MethodExecutor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class MethodExecutor : IMethodExecutor
{
    /// <inheritdoc/>
    public (bool result, string method) ExecuteMethod(object obj, string name, string[]? argValues)
    {
        name = name.ToPascalCase();

        var getMethodResult = obj.GetMethod(name, typeof(bool), argValues);

        if (getMethodResult.result is false)
        {
            return (getMethodResult.result, getMethodResult.msg);
        }

        if (getMethodResult.result is false || getMethodResult.method is null)
        {
            return (getMethodResult.result, getMethodResult.msg);
        }

        var methodParams = getMethodResult.method.GetParameters();

        var methodArgValues = new List<object>();

        for (var i = 0; i < argValues?.Length; i++)
        {
            var parameter = argValues?[i] ?? string.Empty;

            // If the param is a number
            if (parameter.IsWholeNumber())
            {
                bool parseSuccess;
                var paramType = methodParams[i].ParameterType;

                var intValue = -1;
                uint uintValue = 0;

                if (paramType == typeof(int))
                {
                    parseSuccess = int.TryParse(parameter, out intValue);
                }
                else if (paramType == typeof(uint))
                {
                    parseSuccess = uint.TryParse(parameter.Contains('-') ? "0" : parameter, out uintValue);
                }
                else
                {
                    var paramName = methodParams[i].Name;

                    var exceptionMsg = $"Could not convert value to '{paramType}' for parameter '{paramName}'.";
                    exceptionMsg += $"{Environment.NewLine}Can only use 'int' or 'uint' for function '{getMethodResult.method.Name}'.";

                    throw new Exception(exceptionMsg);
                }

                if (parseSuccess is false)
                {
                    throw new Exception($"Issues parsing the '{i + 1}' parameter for the '{name}' method..");
                }

                methodArgValues.Add(paramType == typeof(int) ? intValue : uintValue);
            }
            else
            {
                // Is a string parameter
                methodArgValues.Add(parameter.Trim('\'').Trim('"'));
            }
        }

        var methodResult = (bool)(getMethodResult.method.Invoke(obj, methodArgValues.ToArray()) ?? false);

        return (methodResult, string.Empty);
    }
}
