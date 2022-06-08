﻿// <copyright file="IJSONService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Services;

/// <summary>
/// Loads JSON file data.
/// </summary>
public interface IJSONService
{
    /// <summary>
    /// Serializes the specified object to a JSON string.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A JSON string representation of the object.</returns>
    public string Serialize(object? value);

    /// <summary>
    /// Deserializes the JSON to the specified .NET type.
    /// </summary>
    /// <param name="value">The JSON to deserialize.</param>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <returns>The deserialized object from the JSON string.</returns>
    public T? Deserialize<T>(string value);
}
