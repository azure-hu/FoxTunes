﻿using FoxDb;
using FoxTunes.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FoxTunes.ViewModel
{
    public class LibrarySettings : ViewModelBase
    {
        public ILibraryManager LibraryManager { get; private set; }

        public IHierarchyManager HierarchyManager { get; private set; }

        public IDatabaseFactory DatabaseFactory { get; private set; }

        private CollectionManager<LibraryHierarchy> _LibraryHierarchies { get; set; }

        public CollectionManager<LibraryHierarchy> LibraryHierarchies
        {
            get
            {
                return this._LibraryHierarchies;
            }
            set
            {
                this._LibraryHierarchies = value;
                this.OnLibraryHierarchiesChanged();
            }
        }

        protected virtual void OnLibraryHierarchiesChanged()
        {
            this.OnPropertyChanged("LibraryHierarchies");
        }

        private CollectionManager<LibraryHierarchyLevel> _LibraryHierarchyLevels { get; set; }

        public CollectionManager<LibraryHierarchyLevel> LibraryHierarchyLevels
        {
            get
            {
                return this._LibraryHierarchyLevels;
            }
            set
            {
                this._LibraryHierarchyLevels = value;
                this.OnLibraryHierarchyLevelsChanged();
            }
        }

        protected virtual void OnLibraryHierarchyLevelsChanged()
        {
            this.OnPropertyChanged("LibraryHierarchyLevels");
        }

        private bool _SettingsVisible { get; set; }

        public bool SettingsVisible
        {
            get
            {
                return this._SettingsVisible;
            }
            set
            {
                this._SettingsVisible = value;
                this.OnSettingsVisibleChanged();
            }
        }

        protected virtual void OnSettingsVisibleChanged()
        {
            if (this.SettingsVisibleChanged != null)
            {
                this.SettingsVisibleChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("SettingsVisible");
        }

        public event EventHandler SettingsVisibleChanged;

        public ICommand SaveCommand
        {
            get
            {
                var command = CommandFactory.Instance.CreateCommand(
                    new Func<Task>(this.Save)
                );
                command.Tag = CommandHints.DISMISS;
                return command;
            }
        }

        public async Task Save()
        {
            var exception = default(Exception);
            try
            {
                using (var database = this.DatabaseFactory.Create())
                {
                    using (var transaction = database.BeginTransaction(database.PreferredIsolationLevel))
                    {
                        var libraryHierarchies = database.Set<LibraryHierarchy>(transaction);
                        await libraryHierarchies.RemoveAsync(libraryHierarchies.Except(this.LibraryHierarchies.ItemsSource));
                        await libraryHierarchies.AddOrUpdateAsync(this.LibraryHierarchies.ItemsSource);
                        transaction.Commit();
                    }
                }
                await this.Rebuild();
                return;
            }
            catch (Exception e)
            {
                exception = e;
            }
            await this.OnError("Save", exception);
            throw exception;
        }

        public ICommand RebuildCommand
        {
            get
            {
                var command = CommandFactory.Instance.CreateCommand(
                    new Func<Task>(this.Rebuild)
                );
                command.Tag = CommandHints.DISMISS;
                return command;
            }
        }

        public async Task Rebuild()
        {
            await this.HierarchyManager.Clear(null);
            await this.HierarchyManager.Build(null);
        }

        public ICommand RescanCommand
        {
            get
            {
                var command = CommandFactory.Instance.CreateCommand(
                    new Func<Task>(this.Rescan)
                );
                command.Tag = CommandHints.DISMISS;
                return command;
            }
        }

        public Task Rescan()
        {
            return this.LibraryManager.Rescan();
        }

        public ICommand ClearCommand
        {
            get
            {
                var command = CommandFactory.Instance.CreateCommand(
                    new Func<Task>(this.Clear)
                );
                command.Tag = CommandHints.DISMISS;
                return command;
            }
        }

        public async Task Clear()
        {
            await this.HierarchyManager.Clear(null);
            await this.LibraryManager.Clear(null);
        }

        public ICommand CancelCommand
        {
            get
            {
                return new Command(this.Cancel)
                {
                    Tag = CommandHints.DISMISS
                };
            }
        }

        public void Cancel()
        {
            var task = this.Refresh();
        }

        public ICommand ResetCommand
        {
            get
            {
                return CommandFactory.Instance.CreateCommand(this.Reset);
            }
        }

        public async Task Reset()
        {
            await this.Clear();
            using (var database = this.DatabaseFactory.Create())
            {
                global::FoxTunes.HierarchyManager.CreateDefaultData(database, ComponentRegistry.Instance.GetComponent<IScriptingRuntime>().CoreScripts);
            }
            await this.Refresh();
        }

        public override void InitializeComponent(ICore core)
        {
            this.LibraryManager = this.Core.Managers.Library;
            this.HierarchyManager = this.Core.Managers.Hierarchy;
            this.DatabaseFactory = this.Core.Factories.Database;
            this.LibraryHierarchyLevels = new CollectionManager<LibraryHierarchyLevel>()
            {
                ItemFactory = () =>
                {
                    using (var database = this.DatabaseFactory.Create())
                    {
                        return database.Set<LibraryHierarchyLevel>().Create().With(libraryHierarchyLevel =>
                        {
                            libraryHierarchyLevel.Script = "'New'";
                            libraryHierarchyLevel.Sequence = this.LibraryHierarchyLevels.ItemsSource.Count();
                        });
                    }
                },
                ExchangeHandler = (item1, item2) =>
                {
                    var temp = item1.Sequence;
                    item1.Sequence = item2.Sequence;
                    item2.Sequence = temp;
                }
            };
            this.LibraryHierarchies = new CollectionManager<LibraryHierarchy>()
            {
                ItemFactory = () =>
                {
                    using (var database = this.DatabaseFactory.Create())
                    {
                        return database.Set<LibraryHierarchy>().Create().With(libraryHierarchy =>
                        {
                            libraryHierarchy.Name = "New";
                            libraryHierarchy.Sequence = this.LibraryHierarchies.ItemsSource.Count();
                        });
                    }
                },
                ExchangeHandler = (item1, item2) =>
                {
                    var temp = item1.Sequence;
                    item1.Sequence = item2.Sequence;
                    item2.Sequence = temp;
                }
            };
            this.LibraryHierarchies.SelectedValueChanged += this.OnSelectedValueChanged;
            var task = this.Refresh();
            base.InitializeComponent(core);
        }

        protected virtual void OnSelectedValueChanged(object sender, EventArgs e)
        {
            if (this.LibraryHierarchies.SelectedValue != null)
            {
                this.LibraryHierarchyLevels.ItemsSource = this.LibraryHierarchies.SelectedValue.Levels;
            }
            else
            {
                this.LibraryHierarchyLevels.ItemsSource = null;
            }
        }

        protected virtual Task Refresh()
        {
            return Windows.Invoke(() =>
            {
                using (var database = this.DatabaseFactory.Create())
                {
                    using (var transaction = database.BeginTransaction(database.PreferredIsolationLevel))
                    {
                        this.LibraryHierarchies.ItemsSource = new ObservableCollection<LibraryHierarchy>(
                            database.Set<LibraryHierarchy>(transaction)
                        );
                    }
                }
            });
        }

        protected override void OnDisposing()
        {
            if (this.LibraryHierarchies != null)
            {
                this.LibraryHierarchies.SelectedValueChanged -= this.OnSelectedValueChanged;
            }
            base.OnDisposing();
        }

        protected override Freezable CreateInstanceCore()
        {
            return new LibrarySettings();
        }
    }
}
