﻿namespace FoxTunes.Interfaces
{
    public interface IStandardComponents
    {
        IConfiguration Configuration { get; }

        IUserInterface UserInterface { get; }

        IOutput Output { get; }

        IOutputStreamQueue OutputStreamQueue { get; }

        IScriptingRuntime ScriptingRuntime { get; }

        ILogger Logger { get; }

        ISignalEmitter SignalEmitter { get; }

        ILibraryHierarchyBrowser LibraryHierarchyBrowser { get; }

        ILibraryHierarchyCache LibraryHierarchyCache { get; }

        IArtworkProvider ArtworkProvider { get; }
    }
}
