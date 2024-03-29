﻿using FoxDb;
using FoxTunes.Integration;
using FoxTunes.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoxTunes.ViewModel
{
    public class Playlist : PlaylistBase
    {
        public PlaylistGridViewColumnFactory GridViewColumnFactory { get; private set; }

        public IList SelectedItems
        {
            get
            {
                if (this.PlaylistManager == null)
                {
                    return null;
                }
                return this.PlaylistManager.SelectedItems;
            }
            set
            {
                if (this.PlaylistManager == null)
                {
                    return;
                }
                this.PlaylistManager.SelectedItems = new ObservableCollection<PlaylistItem>(value.OfType<PlaylistItem>());
                this.OnSelectedItemsChanged();
            }
        }

        protected virtual void OnSelectedItemsChanged()
        {
            if (this.SelectedItemsChanged != null)
            {
                this.SelectedItemsChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("SelectedItems");
        }

        public event EventHandler SelectedItemsChanged;

        private bool _InsertActive { get; set; }

        public bool InsertActive
        {
            get
            {
                return this._InsertActive;
            }
            set
            {
                this._InsertActive = value;
                this.OnInsertActiveChanged();
            }
        }

        protected virtual void OnInsertActiveChanged()
        {
            if (this.InsertActiveChanged != null)
            {
                this.InsertActiveChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("InsertActive");
        }

        public event EventHandler InsertActiveChanged;

        private PlaylistItem _InsertItem { get; set; }

        public PlaylistItem InsertItem
        {
            get
            {
                return this._InsertItem;
            }
            set
            {
                this._InsertItem = value;
                this.OnInsertItemChanged();
            }
        }

        protected virtual void OnInsertItemChanged()
        {
            if (this.InsertItemChanged != null)
            {
                this.InsertItemChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("InsertItem");
        }

        public event EventHandler InsertItemChanged;

        private int _InsertOffset { get; set; }

        public int InsertOffset
        {
            get
            {
                return this._InsertOffset;
            }
            set
            {
                this._InsertOffset = value;
                this.OnInsertOffsetChanged();
            }
        }

        protected virtual void OnInsertOffsetChanged()
        {
            if (this.InsertOffsetChanged != null)
            {
                this.InsertOffsetChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("InsertOffset");
        }

        public event EventHandler InsertOffsetChanged;

        private ObservableCollection<GridViewColumn> _GridColumns { get; set; }

        public ObservableCollection<GridViewColumn> GridColumns
        {
            get
            {
                return this._GridColumns;
            }
            set
            {
                this._GridColumns = value;
                this.OnGridColumnsChanged();
            }
        }

        protected virtual void OnGridColumnsChanged()
        {
            if (this.GridColumnsChanged != null)
            {
                this.GridColumnsChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("GridColumns");
        }

        public event EventHandler GridColumnsChanged;

        public override void InitializeComponent(ICore core)
        {
            base.InitializeComponent(core);
            //TODO: This is a hack in order to make the playlist's "is playing" field update.
            this.PlaybackManager.CurrentStreamChanged += this.OnCurrentStreamChanged;
            this.GridViewColumnFactory = new PlaylistGridViewColumnFactory(this.PlaybackManager, this.ScriptingRuntime);
            this.GridViewColumnFactory.PositionChanged += this.OnColumnChanged;
            this.GridViewColumnFactory.WidthChanged += this.OnColumnChanged;
            var task = this.Refresh();
        }

        protected virtual async void OnCurrentStreamChanged(object sender, AsyncEventArgs e)
        {
            using (e.Defer())
            {
                await this.RefreshColumns();
            }
        }

        protected virtual void OnColumnChanged(object sender, PlaylistColumn e)
        {
            if (this.DatabaseFactory != null)
            {
                using (var database = this.DatabaseFactory.Create())
                {
                    using (var transaction = database.BeginTransaction(database.PreferredIsolationLevel))
                    {
                        var set = database.Set<PlaylistColumn>(transaction);
                        set.AddOrUpdate(e);
                        transaction.Commit();
                    }
                }
            }
        }

        protected override async Task OnSignal(object sender, ISignal signal)
        {
            await base.OnSignal(sender, signal);
            switch (signal.Name)
            {
                case CommonSignals.PlaylistUpdated:
                    await this.RefreshColumns();
                    break;
                case CommonSignals.PlaylistColumnsUpdated:
                    await this.ReloadColumns();
                    break;
            }
        }

        public ICommand RemovePlaylistItemsCommand
        {
            get
            {
                return CommandFactory.Instance.CreateCommand(
                    new Func<Task>(this.RemovePlaylistItems)
                );
            }
        }

        protected virtual Task RemovePlaylistItems()
        {
            return this.PlaylistManager.Remove(this.SelectedItems.OfType<PlaylistItem>());
        }

        protected virtual Task CropPlaylistItems()
        {
            return this.PlaylistManager.Crop(this.SelectedItems.OfType<PlaylistItem>());
        }

        protected virtual Task LocatePlaylistItems()
        {
            foreach (var item in this.SelectedItems.OfType<PlaylistItem>())
            {
                Explorer.Select(item.FileName);
            }
#if NET40
            return TaskEx.FromResult(false);
#else
            return Task.CompletedTask;
#endif
        }

        public ICommand PlaySelectedItemCommand
        {
            get
            {
                return CommandFactory.Instance.CreateCommand(
                    () =>
                    {
                        var playlistItem = this.SelectedItems[0] as PlaylistItem;
                        return this.PlaylistManager.Play(playlistItem);
                    },
                    () => this.PlaylistManager != null && this.SelectedItems.Count > 0
                );
            }
        }

        public ICommand DragEnterCommand
        {
            get
            {
                return new Command<DragEventArgs>(this.OnDragEnter);
            }
        }

        protected virtual void OnDragEnter(DragEventArgs e)
        {
            this.UpdateDragDropEffects(e);
        }

        public ICommand DragOverCommand
        {
            get
            {
                return new Command<DragEventArgs>(this.OnDragOver);
            }
        }

        protected virtual void OnDragOver(DragEventArgs e)
        {
            this.UpdateDragDropEffects(e);
        }

        protected virtual void UpdateDragDropEffects(DragEventArgs e)
        {
            var effects = DragDropEffects.None;
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    effects = DragDropEffects.Copy;
                }
                if (e.Data.GetDataPresent(typeof(LibraryHierarchyNode)))
                {
                    effects = DragDropEffects.Copy;
                }
                if (e.Data.GetDataPresent<IEnumerable<PlaylistItem>>(true))
                {
                    effects = DragDropEffects.Copy;
                }
#if VISTA
                if (ShellIDListHelper.GetDataPresent(e.Data))
                {
                    effects = DragDropEffects.Copy;
                }
#endif
            }
            catch (Exception exception)
            {
                Logger.Write(this, LogLevel.Warn, "Failed to query clipboard contents: {0}", exception.Message);
            }
            e.Effects = effects;
        }

        public ICommand DropCommand
        {
            get
            {
                return CommandFactory.Instance.CreateCommand<DragEventArgs>(
                    new Func<DragEventArgs, Task>(this.OnDrop)
                );
            }
        }

        protected virtual Task OnDrop(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var paths = e.Data.GetData(DataFormats.FileDrop) as IEnumerable<string>;
                    return this.AddToPlaylist(paths);
                }
                if (e.Data.GetDataPresent(typeof(LibraryHierarchyNode)))
                {
                    var libraryHierarchyNode = e.Data.GetData(typeof(LibraryHierarchyNode)) as LibraryHierarchyNode;
                    return this.AddToPlaylist(libraryHierarchyNode);
                }
                if (e.Data.GetDataPresent<IEnumerable<PlaylistItem>>(true))
                {
                    var playlistItems = e.Data
                        .GetData<IEnumerable<PlaylistItem>>(true)
                        .OrderBy(playlistItem => playlistItem.Sequence);
                    return this.AddToPlaylist(playlistItems);
                }
#if VISTA
                if (ShellIDListHelper.GetDataPresent(e.Data))
                {
                    var paths = ShellIDListHelper.GetData(e.Data);
                    return this.AddToPlaylist(paths);
                }
#endif
            }
            catch (Exception exception)
            {
                Logger.Write(this, LogLevel.Warn, "Failed to process clipboard contents: {0}", exception.Message);
            }
#if NET40
            return TaskEx.FromResult(false);
#else
            return Task.CompletedTask;
#endif
        }

        private Task AddToPlaylist(IEnumerable<string> paths)
        {
            var sequence = default(int);
            if (this.TryGetInsertSequence(out sequence))
            {
                return this.PlaylistManager.Insert(sequence, paths, false);
            }
            else
            {
                return this.PlaylistManager.Add(paths, false);
            }
        }

        private Task AddToPlaylist(LibraryHierarchyNode libraryHierarchyNode)
        {
            var sequence = default(int);
            if (this.TryGetInsertSequence(out sequence))
            {
                return this.PlaylistManager.Insert(sequence, libraryHierarchyNode, false);
            }
            else
            {
                return this.PlaylistManager.Add(libraryHierarchyNode, false);
            }
        }

        private Task AddToPlaylist(IEnumerable<PlaylistItem> playlistItems)
        {
            var sequence = default(int);
            if (this.TryGetInsertSequence(out sequence))
            {
                //TODO: This was really confusing, there is some disconnection between what the UI suggests will happen (where the items will be moved to) and what happens.
                //TODO: I don't really understand it but if the current sequence is less than the target sequence (moving down the list) then we have to decrement the target sequence.
                //TODO: The unit tests don't have this problem for some reason.
                var playlistItem = playlistItems.FirstOrDefault();
                if (playlistItem != null && playlistItem.Sequence < sequence)
                {
                    sequence--;
                }
                return this.PlaylistManager.Move(sequence, playlistItems);
            }
            else
            {
                return this.PlaylistManager.Move(playlistItems);
            }
        }

        protected virtual bool TryGetInsertSequence(out int sequence)
        {
            if (!this.InsertActive || this.InsertItem == null)
            {
                sequence = 0;
                return false;
            }
            sequence = this.InsertItem.Sequence + this.InsertOffset;
            return true;
        }

        protected virtual IEnumerable<GridViewColumn> GetGridColumns()
        {
            if (this.DatabaseFactory != null && this.GridViewColumnFactory != null)
            {
                using (var database = this.DatabaseFactory.Create())
                {
                    using (var transaction = database.BeginTransaction(database.PreferredIsolationLevel))
                    {
                        foreach (var column in database.Set<PlaylistColumn>(transaction))
                        {
                            yield return this.GridViewColumnFactory.Create(column);
                        }
                    }
                }
            }
        }

        public virtual async Task Refresh()
        {
            await this.RefreshItems();
            await this.RefreshSelectedItems();
            await this.RefreshColumns();
        }

        public virtual Task RefreshSelectedItems()
        {
            return Windows.Invoke(() => this.OnSelectedItemsChanged());
        }

        public virtual async Task RefreshColumns()
        {
            if (this.GridColumns == null || this.GridColumns.Count == 0)
            {
                await this.ReloadColumns();
            }
            if (this.GridColumns != null)
            {
                foreach (var column in this.GridColumns)
                {
                    await this.RefreshColumn(column);
                }
            }
        }

        protected virtual Task RefreshColumn(GridViewColumn column)
        {
            return Windows.Invoke(() => this.GridViewColumnFactory.Refresh(column));
        }

        protected virtual Task ReloadColumns()
        {
            var columns = this.GetGridColumns();
            return Windows.Invoke(() => this.GridColumns = new ObservableCollection<GridViewColumn>(columns));
        }

        protected override void OnDisposing()
        {
            if (this.PlaybackManager != null)
            {
                this.PlaybackManager.CurrentStreamChanged -= this.OnCurrentStreamChanged;
            }
            if (this.GridViewColumnFactory != null)
            {
                this.GridViewColumnFactory.PositionChanged -= this.OnColumnChanged;
                this.GridViewColumnFactory.WidthChanged -= this.OnColumnChanged;
                this.GridViewColumnFactory.Dispose();
            }
            base.OnDisposing();
        }

        protected override Freezable CreateInstanceCore()
        {
            return new Playlist();
        }
    }
}
