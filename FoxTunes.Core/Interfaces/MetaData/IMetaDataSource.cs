﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoxTunes.Interfaces
{
    public interface IMetaDataSource : IBaseComponent
    {
        Task<IEnumerable<MetaDataItem>> GetMetaData(string fileName);

        Task SetMetaData(string fileName, IEnumerable<MetaDataItem> metaDataItems);
    }

    [Flags]
    public enum MetaDataCategory : byte
    {
        None = 0,
        Standard = 1,
        Extended = 2,
        MusicBrainz = 4,
        MultiMedia = 8
    }

    [Flags]
    public enum ArtworkType : byte
    {
        None = 0,
        FrontCover = 1,
        BackCover = 2,
        Unknown = 255
    }
}
