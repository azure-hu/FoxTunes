using System;
using System.Collections.Generic;
using System.Linq;

namespace FoxTunes
{
    public static class MetaDataSourceFactoryConfiguration
    {
        public static readonly string[] TAGS = new[]
        {
            CommonMetaData.Album,
            CommonMetaData.AlbumArtists,
            CommonMetaData.Artists,
            CommonMetaData.Composers,
            CommonMetaData.Conductor,
            CommonMetaData.Disc,
            CommonMetaData.DiscCount,
            CommonMetaData.Genres,
            CommonMetaData.Performers,
            CommonMetaData.Title,
            CommonMetaData.Track,
            CommonMetaData.TrackCount,
            CommonMetaData.Year,
            CommonMetaData.FirstAlbumArtist,
            CommonMetaData.FirstArtist,
            CommonMetaData.FirstComposer,
            CommonMetaData.FirstGenre,
            CommonMetaData.FirstPerformer,
        };

        public static readonly string[] PROPERTIES = new[]
        {
            CommonProperties.Duration,
            CommonProperties.AudioBitrate,
            CommonProperties.AudioChannels,
            CommonProperties.AudioSampleRate,
            CommonProperties.BitsPerSample,
        };

        public static readonly string[] IMAGES = new[]
        {
            CommonImageTypes.FrontCover
        };

        public const string SECTION = "5CCE5DA6-E4C0-49CD-9D5C-BD60FC036832";

        public const string THREADS = "AAAA71E7-A0B4-4A69-9B23-74661E9476F4";

        public static IEnumerable<ConfigurationSection> GetConfigurationSections()
        {
            var section = new ConfigurationSection(SECTION, "Tagging");
            section.WithElement(new TextConfigurationElement(THREADS, "Threads").WithValue(Environment.ProcessorCount.ToString()).WithValidationRule(new IntegerValidationRule(1, 64)));
            foreach (var key in CommonMetaData.Lookup.Keys)
            {
                section.WithElement(new BooleanConfigurationElement(key, key, path: "Tags").WithValue(TAGS.Contains(key)));
            }
            foreach (var key in CommonProperties.Lookup.Keys)
            {
                section.WithElement(new BooleanConfigurationElement(key, key, path: "Properties").WithValue(PROPERTIES.Contains(key)));
            }
            foreach (var key in CommonImageTypes.Lookup.Keys)
            {
                section.WithElement(new BooleanConfigurationElement(key, key, path: "Images").WithValue(IMAGES.Contains(key)));
            }
            yield return section;
        }
    }
}
