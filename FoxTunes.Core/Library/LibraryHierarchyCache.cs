﻿using FoxTunes.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FoxTunes
{
    [ComponentDependency(Slot = ComponentSlots.Database)]
    public class LibraryHierarchyCache : StandardComponent, ILibraryHierarchyCache
    {
        public IEnumerable<LibraryHierarchyCacheKey> Keys
        {
            get
            {
                return this.Nodes.Keys.ToArray();
            }
        }

        public Lazy<IList<LibraryHierarchy>> Hierarchies { get; private set; }

        public ConcurrentDictionary<LibraryHierarchyCacheKey, Lazy<IList<LibraryHierarchyNode>>> Nodes { get; private set; }

        public ISignalEmitter SignalEmitter { get; private set; }

        public LibraryHierarchyCache()
        {
            this.Reset();
        }

        public override void InitializeComponent(ICore core)
        {
            this.SignalEmitter = core.Components.SignalEmitter;
            this.SignalEmitter.Signal += this.OnSignal;
            base.InitializeComponent(core);
        }

        protected virtual Task OnSignal(object sender, ISignal signal)
        {
            switch (signal.Name)
            {
                case CommonSignals.HierarchiesUpdated:
                    if (!object.Equals(signal.State, CommonSignalFlags.SOFT))
                    {
                        Logger.Write(this, LogLevel.Debug, "Hierarchies were updated, resetting cache.");
                        this.Reset();
                    }
                    else
                    {
                        Logger.Write(this, LogLevel.Debug, "Hierarchies were updated but soft flag was specified, ignoring.");
                    }
                    break;
            }
#if NET40
            return TaskEx.FromResult(false);
#else
            return Task.CompletedTask;
#endif
        }

        public IEnumerable<LibraryHierarchy> GetHierarchies(Func<IEnumerable<LibraryHierarchy>> factory)
        {
            if (this.Hierarchies == null)
            {
                this.Hierarchies = new Lazy<IList<LibraryHierarchy>>(() => new List<LibraryHierarchy>(factory()));
            }
            return this.Hierarchies.Value;
        }

        public IEnumerable<LibraryHierarchyNode> GetNodes(LibraryHierarchyCacheKey key, Func<IEnumerable<LibraryHierarchyNode>> factory)
        {
            return this.Nodes.GetOrAdd(key, _key => new Lazy<IList<LibraryHierarchyNode>>(() => new List<LibraryHierarchyNode>(factory()))).Value;
        }

        public void Reset()
        {
            this.Hierarchies = null;
            this.Nodes = new ConcurrentDictionary<LibraryHierarchyCacheKey, Lazy<IList<LibraryHierarchyNode>>>();
        }

        public void Evict(LibraryHierarchyCacheKey key)
        {
            Logger.Write(this, LogLevel.Debug, "Evicting cache entry: {0}", key);
            this.Nodes.TryRemove(key);
        }
    }
}
