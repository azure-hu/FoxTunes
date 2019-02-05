using FoxTunes.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FoxTunes
{
    public class ArtworkProvider : StandardComponent, IArtworkProvider
    {
        public static IDictionary<string, string[]> Names = GetNames();

        private static IDictionary<string, string[]> GetNames()
        {
            return new Dictionary<string, string[]>()
            {
                { CommonImageTypes.FrontCover, new [] { "front", "cover", "folder" } },
                { CommonImageTypes.BackCover, new [] { "back" } }
            };
        }

        public async Task<string> Find(string path, string type)
        {
            var names = default(string[]);
            if (!Names.TryGetValue(type, out names))
            {
                throw new NotImplementedException();
            }
            if (!string.IsNullOrEmpty(Path.GetPathRoot(path)))
            {
                var exception = default(Exception);
                try
                {
                    var directoryName = Path.GetDirectoryName(path);
                    foreach (var name in names)
                    {
                        foreach (var fileName in Directory.EnumerateFileSystemEntries(directoryName, string.Format("{0}.*", name)))
                        {
                            return fileName;
                        }
                    }
                }
                catch (Exception e)
                {
                    exception = e;
                }
                if (exception != null)
                {
                    await this.OnError(exception);
                }
            }
            return default(string);
        }

        public Task<string> Find(PlaylistItem playlistItem, string type)
        {
            var result = playlistItem.MetaDatas.FirstOrDefault(
                 metaDataItem =>
                     metaDataItem.Type == MetaDataItemType.Image &&
                     string.Equals(metaDataItem.Name, type, StringComparison.OrdinalIgnoreCase) &&
                     File.Exists(metaDataItem.FileValue)
             );
            if (result != null)
            {
#if NET40
                return TaskEx.FromResult(result.FileValue);
#else
                return Task.FromResult(result.FileValue);
#endif
            }
#if NET40
            return TaskEx.FromResult(default(string));
#else
            return Task.FromResult(default(string));
#endif
        }
    }
}
