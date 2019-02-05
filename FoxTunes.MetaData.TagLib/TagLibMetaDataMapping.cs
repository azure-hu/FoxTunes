using FoxDb;
using FoxDb.Interfaces;
using FoxTunes.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TagLib;

namespace FoxTunes
{
    public class TagLibMetaDataMapping
    {
        private static readonly IPropertyAccessorStrategy PropertyAccessorStrategy = new LambdaPropertyAccessorStrategy(false);

        private static readonly ConcurrentDictionary<string, Func<Tag, object>> TagAccessors = new ConcurrentDictionary<string, Func<Tag, object>>();

        private static readonly ConcurrentDictionary<string, Func<Properties, object>> PropertyAccessors = new ConcurrentDictionary<string, Func<Properties, object>>();

        private TagLibMetaDataMapping()
        {
            this.Tags = new HashSet<string>();
            this.Properties = new HashSet<string>();
            this.Images = new HashSet<string>();
        }

        public TagLibMetaDataMapping(IConfiguration configuration) : this()
        {
            var section = configuration.GetSection(MetaDataSourceFactoryConfiguration.SECTION);
            foreach (var element in section.Elements.OfType<BooleanConfigurationElement>())
            {
                if (!element.Value)
                {
                    continue;
                }
                switch (element.Path)
                {
                    case "Tags":
                        this.Tags.Add(element.Id);
                        break;
                    case "Properties":
                        this.Properties.Add(element.Id);
                        break;
                    case "Images":
                        this.Images.Add(element.Id);
                        break;
                }
            }
        }

        public HashSet<string> Tags { get; private set; }

        public HashSet<string> Properties { get; private set; }

        public HashSet<string> Images { get; private set; }

        public IEnumerable<KeyValuePair<string, object>> GetTags(Tag tag)
        {
            foreach (var element in this.Tags)
            {
                var value = this.GetTag(tag, element);
                if (string.IsNullOrEmpty(value.Key) || value.Value == null)
                {
                    continue;
                }
                if (value.Value.GetType().IsArray)
                {
                    foreach (var subValue in (Array)value.Value)
                    {
                        yield return new KeyValuePair<string, object>(value.Key, subValue);
                    }
                }
                else
                {
                    yield return value;
                }
            }
        }

        private KeyValuePair<string, object> GetTag(Tag tag, string element)
        {
            var propertyAccessor = TagAccessors.GetOrAdd(
                element,
                key =>
                {
                    var property = typeof(Tag).GetProperty(element);
                    return PropertyAccessorStrategy.CreateGetter<Tag, object>(property);
                }
            );
            return new KeyValuePair<string, object>(element, propertyAccessor(tag));
        }

        public IEnumerable<KeyValuePair<string, object>> GetProperties(Properties properties)
        {
            foreach (var element in this.Properties)
            {
                var value = this.GetProperty(properties, element);
                if (string.IsNullOrEmpty(value.Key) || value.Value == null)
                {
                    continue;
                }
                yield return value;
            }
        }

        private KeyValuePair<string, object> GetProperty(Properties properties, string element)
        {
            var propertyAccessor = PropertyAccessors.GetOrAdd(
                element,
                key =>
                {
                    var property = typeof(Properties).GetProperty(element);
                    return PropertyAccessorStrategy.CreateGetter<Properties, object>(property);
                }
            );
            return new KeyValuePair<string, object>(element, propertyAccessor(properties));
        }

        public IEnumerable<KeyValuePair<string, IPicture>> GetPictures(IPicture[] pictures)
        {
            foreach (var element in this.Images)
            {
                var value = this.GetPicture(pictures, element);
                if (string.IsNullOrEmpty(value.Key) || value.Value == null)
                {
                    continue;
                }
                yield return value;
            }
        }

        private KeyValuePair<string, IPicture> GetPicture(IPicture[] pictures, string element)
        {
            foreach (var picture in pictures)
            {
                var type = Enum.GetName(typeof(PictureType), picture.Type);
                if (string.Equals(element, type, StringComparison.OrdinalIgnoreCase))
                {
                    return new KeyValuePair<string, IPicture>(element, picture);
                }
            }
            return default(KeyValuePair<string, IPicture>);
        }
    }
}
