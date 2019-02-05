using FoxTunes.Interfaces;
using System.Collections.Generic;

namespace FoxTunes
{
    public abstract class MetaDataSourceFactory : StandardFactory, IMetaDataSourceFactory, IConfigurableComponent
    {
        public abstract IMetaDataSource Create();

        public IEnumerable<ConfigurationSection> GetConfigurationSections()
        {
            return MetaDataSourceFactoryConfiguration.GetConfigurationSections();
        }
    }
}
