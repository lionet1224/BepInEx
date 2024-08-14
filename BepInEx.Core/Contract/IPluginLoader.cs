﻿namespace BepInEx;

/// <summary>
///     An interface that allows to dynamically load and track an assembly
///     which can be obtained from a provided
/// </summary>
public interface IPluginLoader
{
    /// <summary>
    ///     An identifier that uniquely identifies an assembly from a provider no matter its revision
    /// </summary>
    public string AssemblyIdentifier { get; }
    
    /// <summary>
    ///     An optional hash that changes each time the assembly changes which can be tracked for cache
    ///     invalidation purposes. If this is null, no caching occurs for this assembly load
    /// </summary>
    public string AssemblyHash { get; }
    
    /// <summary>
    ///     Obtains the assembly's raw data without loading it into the appdomain.
    ///     This may be called multiple times by the chainloader
    /// </summary>
    /// <returns>The assembly's raw data in bytes</returns>
    public byte[] GetAssemblyData();
}
