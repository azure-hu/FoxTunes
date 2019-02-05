using FoxTunes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TagLib;

namespace FoxTunes
{
    public class TagLibMetaDataSource : BaseComponent, IMetaDataSource
    {
        public static SemaphoreSlim Semaphore { get; private set; }

        static TagLibMetaDataSource()
        {
            Semaphore = new SemaphoreSlim(1, 1);
        }

        public TagLibMetaDataSource(TagLibMetaDataMapping mapping)
        {
            this.Mapping = mapping;
        }

        public TagLibMetaDataMapping Mapping { get; private set; }

        public async Task<IEnumerable<MetaDataItem>> GetMetaData(string fileName)
        {
            if (!this.IsSupported(fileName))
            {
                Logger.Write(this, LogLevel.Warn, "Unsupported file format: {0}", fileName);
                return Enumerable.Empty<MetaDataItem>();
            }
            var metaData = new List<MetaDataItem>();
            Logger.Write(this, LogLevel.Trace, "Reading meta data for file: {0}", fileName);
            try
            {
                var file = this.Create(fileName);
                this.AddTags(metaData, file.Tag);
                this.AddProperties(metaData, file.Properties);
                if (file.Tag.Pictures != null)
                {
                    await this.AddImages(metaData, file.Tag, file.Tag.Pictures);
                }
            }
            catch (UnsupportedFormatException)
            {
                Logger.Write(this, LogLevel.Warn, "Unsupported file format: {0}", fileName);
            }
            return metaData;
        }

        protected virtual bool IsSupported(string fileName)
        {
            var mimeType = string.Format("taglib/{0}", fileName.GetExtension());
            return FileTypes.AvailableTypes.ContainsKey(mimeType);
        }

        protected virtual File Create(string fileName)
        {
            var file = File.Create(fileName);
            if (file.PossiblyCorrupt)
            {
                foreach (var reason in file.CorruptionReasons)
                {
                    Logger.Write(this, LogLevel.Debug, "Meta data corruption detected: {0} => {1}", fileName, reason);
                }
            }
            return file;
        }

        private void AddTags(IList<MetaDataItem> metaData, Tag tag)
        {
            foreach (var element in this.Mapping.GetTags(tag))
            {
                this.AddMetaData(metaData, MetaDataItemType.Tag, element);
            }
        }

        private void AddProperties(IList<MetaDataItem> metaData, Properties properties)
        {
            foreach (var element in this.Mapping.GetProperties(properties))
            {
                this.AddMetaData(metaData, MetaDataItemType.Property, element);
            }
        }

        private void AddMetaData(IList<MetaDataItem> metaData, MetaDataItemType type, KeyValuePair<string, object> element)
        {
            switch (Type.GetTypeCode(element.Value.GetType()))
            {
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    metaData.Add(new MetaDataItem(element.Key, type)
                    {
                        NumericValue = Convert.ToInt32(element.Value)
                    });
                    break;
                case TypeCode.String:
                    metaData.Add(new MetaDataItem(element.Key, type)
                    {
                        TextValue = Convert.ToString(element.Value)
                    });
                    break;
                default:
                    if (element.Value is TimeSpan)
                    {
                        metaData.Add(new MetaDataItem(element.Key, type)
                        {
                            NumericValue = Convert.ToInt32(((TimeSpan)element.Value).TotalMilliseconds)
                        });
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    break;
            }
        }

        private async Task AddImages(IList<MetaDataItem> metaData, Tag tag, IPicture[] pictures)
        {
            foreach (var element in this.Mapping.GetPictures(pictures))
            {
                var id = this.GetImageId(tag, element.Key, element.Value);
                var fileName = await this.AddImage(id, element.Value);
                metaData.Add(new MetaDataItem(element.Key, MetaDataItemType.Image)
                {
                    FileValue = fileName
                });
            }
        }

        private async Task<string> AddImage(string id, IPicture value)
        {
            var fileName = default(string);
            if (!FileMetaDataStore.Exists(id, out fileName))
            {
#if NET40
                Semaphore.Wait();
#else
                await Semaphore.WaitAsync();
#endif
                try
                {
                    if (!FileMetaDataStore.Exists(id, out fileName))
                    {
                        return await FileMetaDataStore.Write(id, value.Data.Data);
                    }
                }
                finally
                {
                    Semaphore.Release();
                }
            }
            return fileName;
        }

#pragma warning disable 612, 618
        private string GetImageId(Tag tag, string type, IPicture value)
        {
            return string.Format(
                "{0}_{1}_{2}",
                tag.FirstAlbumArtist
                    .IfNullOrEmpty(tag.FirstAlbumArtistSort)
                    .IfNullOrEmpty(tag.FirstArtist),
                tag.Album,
                type
            );
        }
#pragma warning restore 612, 618

        public static readonly IDictionary<PictureType, string> PictureTypeMapping = new Dictionary<PictureType, string>()
        {
            { PictureType.FrontCover, CommonImageTypes.FrontCover },
            { PictureType.BackCover, CommonImageTypes.BackCover },
        };

        public static string GetArtworkType(PictureType pictureType)
        {
            var artworkType = default(string);
            if (PictureTypeMapping.TryGetValue(pictureType, out artworkType))
            {
                return artworkType;
            }
            return default(string);
        }
    }
}
