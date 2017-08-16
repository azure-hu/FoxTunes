﻿using FoxTunes.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FoxTunes.ViewModel
{
    public class Library : ViewModelBase
    {
        public Library()
        {
            this._Items = new Dictionary<LibraryHierarchy, ObservableCollection<RenderableLibraryHierarchyItem>>();
        }

        public IDatabase Database { get; private set; }

        public IDatabaseQuery<LibraryHierarchyItem> LibraryHierarchyItemQuery { get; private set; }

        private LibraryHierarchy _SelectedHierarchy { get; set; }

        public LibraryHierarchy SelectedHierarchy
        {
            get
            {
                return this._SelectedHierarchy;
            }
            set
            {
                this._SelectedHierarchy = value;
                this.OnSelectedHierarchyChanged();
            }
        }

        protected virtual void OnSelectedHierarchyChanged()
        {
            if (this.SelectedHierarchyChanged != null)
            {
                this.SelectedHierarchyChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("SelectedHierarchy");
            this.Refresh();
        }

        public event EventHandler SelectedHierarchyChanged = delegate { };

        private Dictionary<LibraryHierarchy, ObservableCollection<RenderableLibraryHierarchyItem>> _Items { get; set; }

        public ObservableCollection<RenderableLibraryHierarchyItem> Items
        {
            get
            {
                if (this.Database == null || this.SelectedHierarchy == null)
                {
                    return null;
                }
                if (!this._Items.ContainsKey(this.SelectedHierarchy))
                {
                    var libraryHierarchyItems = this.SelectedHierarchy.Items
                        .Select(libraryHierarchyItem => new RenderableLibraryHierarchyItem(libraryHierarchyItem, this.Database));
                    this._Items[this.SelectedHierarchy] = new ObservableCollection<RenderableLibraryHierarchyItem>(libraryHierarchyItems);
                }
                return this._Items[this.SelectedHierarchy];
            }
        }

        protected virtual void OnItemsChanged()
        {
            if (this.ItemsChanged != null)
            {
                this.ItemsChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("Items");
        }

        public void Reload()
        {
            this._Items.Clear();
            this.Refresh();
        }

        public void Refresh()
        {
            this.OnItemsChanged();
        }

        public event EventHandler ItemsChanged = delegate { };

        protected override void OnCoreChanged()
        {
            this.Database = this.Core.Components.Database;
            this.Core.Managers.Library.Updated += (sender, e) => this.Reload();
            this.Refresh();
            base.OnCoreChanged();
        }

        private string _Filter { get; set; }

        public string Filter
        {
            get
            {
                return this._Filter;
            }
            set
            {
                this._Filter = value;
                this.OnFilterChanged();
            }
        }

        protected virtual void OnFilterChanged()
        {
            if (this.FilterChanged != null)
            {
                this.FilterChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("Filter");
            this.Reload();
        }

        public event EventHandler FilterChanged = delegate { };

        protected override Freezable CreateInstanceCore()
        {
            return new Library();
        }
    }
}