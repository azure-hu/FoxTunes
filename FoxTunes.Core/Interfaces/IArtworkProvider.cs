using System.Threading.Tasks;

namespace FoxTunes.Interfaces
{
    public interface IArtworkProvider : IStandardComponent
    {
        Task<string> Find(PlaylistItem playlistItem, string type);

        Task<string> Find(string path, string type);
    }
}
