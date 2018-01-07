﻿using FoxDb.Interfaces;
using System.Collections.Generic;

namespace FoxTunes.Interfaces
{
    public interface IDatabaseQueries : IBaseComponent
    {
        IDatabaseQuery AddLibraryHierarchyNodeToPlaylist { get; }

        IDatabaseQuery AddLibraryHierarchyRecord { get; }

        IDatabaseQuery AddPlaylistSequenceRecord { get; }

        IDatabaseQuery AddLibraryItem { get; }

        IDatabaseQuery AddLibraryMetaDataItems { get; }

        IDatabaseQuery AddPlaylistItem { get; }

        IDatabaseQuery AddPlaylistMetaDataItems { get; }

        IDatabaseQuery ClearPlaylist { get; }

        IDatabaseQuery ClearLibrary { get; }

        IDatabaseQuery CopyMetaDataItems { get; }

        IDatabaseQuery GetPlaylistItemsWithoutMetaData { get; }

        IDatabaseQuery GetLibraryItems { get; }

        IDatabaseQuery GetLibraryHierarchyMetaDataItems { get; }

        IDatabaseQuery GetPlaylistMetaDataItems { get; }

        IDatabaseQuery GetLibraryHierarchyNodes { get; }

        IDatabaseQuery GetLibraryHierarchyNodesWithFilter { get; }

        IDatabaseQuery SetLibraryItemStatus { get; }

        IDatabaseQuery SetPlaylistItemStatus { get; }

        IDatabaseQuery ShiftPlaylistItems { get; }

        IDatabaseQuery VariousArtists { get; }

        IDatabaseQuery PlaylistSequenceBuilder(IEnumerable<string> metaDataNames);

        IDatabaseQuery LibraryHierarchyBuilder(IEnumerable<string> metaDataNames);

        IDatabaseQuery GetMetaDataNames { get; }
    }
}