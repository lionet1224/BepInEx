﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx.Logging;

namespace BepInEx.PluginProvider;

[BepInPluginProvider("BepInExPluginProvider", "BepInExPluginProvider", "1.0")]
internal class BepInExPluginProvider : BasePluginProvider
{
    private static readonly Dictionary<string, string> AssemblyLocationsByFilename = new();
    
    public override IList<IPluginLoadContext> GetPlugins()
    {
        var loadContexts = new List<IPluginLoadContext>();
        foreach (var dll in Directory.GetFiles(Path.GetFullPath(Paths.PluginPath), "*.dll", SearchOption.AllDirectories))
        {
            try
            {
                AssemblyLocationsByFilename.Add(Path.GetFileNameWithoutExtension(dll), Path.GetDirectoryName(dll));
                loadContexts.Add(new BepInExPluginLoadContext
                {
                    AssemblyHash = File.GetLastWriteTimeUtc(dll).ToString("O"),
                    AssemblyIdentifier = dll
                });
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, e);
            }
        }
        
        return loadContexts;
    }

    public override Assembly ResolveAssembly(string name)
    {
        if (!AssemblyLocationsByFilename.TryGetValue(name, out var location))
            return null;

        if (!Utility.TryResolveDllAssemblyWithSymbols(new(name), location, out var ass))
            return null;

        return ass;
    }
}
