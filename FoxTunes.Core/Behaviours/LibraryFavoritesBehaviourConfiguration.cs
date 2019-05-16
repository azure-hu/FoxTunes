using System.Collections.Generic;

namespace FoxTunes
{
    public static class LibraryFavoritesBehaviourConfiguration
    {
        public const string SECTION = LibraryBehaviourConfiguration.SECTION;

        public const string SHOW_FAVORITES_ELEMENT = "51ED8A2D-9161-479B-B6DA-29D5B232946D";

        public static IEnumerable<ConfigurationSection> GetConfigurationSections()
        {
            yield return new ConfigurationSection(SECTION, "Library")
                .WithElement(
                    new BooleanConfigurationElement(SHOW_FAVORITES_ELEMENT, "Show Favorites").WithValue(false)
            );
        }
    }
}
