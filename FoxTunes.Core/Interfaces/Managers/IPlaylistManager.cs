﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FoxTunes.Interfaces
{
    public interface IPlaylistManager : IStandardManager, IBackgroundTaskSource, IInvocableComponent
    {
        Task Add(IEnumerable<string> paths, bool clear);

        Task Insert(int index, IEnumerable<string> paths, bool clear);

        Task Add(LibraryHierarchyNode libraryHierarchyNode, bool clear);

        Task Insert(int index, LibraryHierarchyNode libraryHierarchyNode, bool clear);

        Task Move(IEnumerable<PlaylistItem> playlistItems);

        Task Move(int index, IEnumerable<PlaylistItem> playlistItems);

        Task Remove(IEnumerable<PlaylistItem> playlistItems);

        Task Crop(IEnumerable<PlaylistItem> playlistItems);

        Task Play(PlaylistItem playlistItem);

        Task Play(string fileName);

        Task Play(int sequence);

        bool CanNavigate { get; }

        event AsyncEventHandler CanNavigateChanged;

        Task<PlaylistItem> Get(int sequence);

        Task<PlaylistItem> Get(string fileName);

        Task<PlaylistItem> GetNext(bool navigate);

        Task<PlaylistItem> GetPrevious(bool navigate);

        Task<int> GetInsertIndex();

        Task Next();

        Task Previous();

        Task Clear();

        PlaylistItem CurrentItem { get; }

        event AsyncEventHandler CurrentItemChanged;

        ObservableCollection<PlaylistItem> SelectedItems { get; set; }

        event EventHandler SelectedItemsChanged;
    }
}
