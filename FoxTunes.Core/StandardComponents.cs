﻿using FoxTunes.Interfaces;
using System.Linq;

namespace FoxTunes
{
    public class StandardComponents : IStandardComponents
    {
        private StandardComponents()
        {

        }

        public IConfiguration Configuration
        {
            get
            {
                return ComponentRegistry.Instance.GetComponent<IConfiguration>();
            }
        }

        public IUserInterface UserInterface
        {
            get
            {
                return ComponentRegistry.Instance.GetComponent<IUserInterface>();
            }
        }

        public IOutput Output
        {
            get
            {
                return ComponentRegistry.Instance.GetComponent<IOutput>();
            }
        }

        public IOutputStreamQueue OutputStreamQueue
        {
            get
            {
                return ComponentRegistry.Instance.GetComponent<IOutputStreamQueue>();
            }
        }

        public IScriptingRuntime ScriptingRuntime
        {
            get
            {
                return ComponentRegistry.Instance.GetComponent<IScriptingRuntime>();
            }
        }

        public ILogger Logger
        {
            get
            {
                return ComponentRegistry.Instance.GetComponent<ILogger>();
            }
        }

        public ISignalEmitter SignalEmitter
        {
            get
            {
                return ComponentRegistry.Instance.GetComponent<ISignalEmitter>();
            }
        }

        public ILibraryHierarchyBrowser LibraryHierarchyBrowser
        {
            get
            {
                return ComponentRegistry.Instance.GetComponent<ILibraryHierarchyBrowser>();
            }
        }

        public ILibraryHierarchyCache LibraryHierarchyCache
        {
            get
            {
                return ComponentRegistry.Instance.GetComponent<ILibraryHierarchyCache>();
            }
        }

        public IArtworkProvider ArtworkProvider
        {
            get
            {
                return ComponentRegistry.Instance.GetComponent<IArtworkProvider>();
            }
        }

        public static readonly IStandardComponents Instance = new StandardComponents();
    }
}
