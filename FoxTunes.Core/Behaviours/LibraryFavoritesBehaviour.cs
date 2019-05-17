using FoxTunes.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoxTunes
{
    [ComponentDependency(Slot = ComponentSlots.Output)]
    public class LibraryFavoritesBehaviour : StandardBehaviour, IInvocableComponent, IConfigurableComponent
    {
        public const string TOGGLE_FAVORITE = "AAAD";

        public const string TOGGLE_SHOW_FAVORITES = "AAAE";

        public ILibraryManager LibraryManager { get; private set; }

        public ISignalEmitter SignalEmitter { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public BooleanConfigurationElement ShowFavorites { get; private set; }

        public bool IsFavorite
        {
            get
            {
                var libraryHierarchyNode = this.LibraryManager.SelectedItem;
                if (libraryHierarchyNode == null)
                {
                    return false;
                }
                //TODO: Bad .Result
                return this.LibraryManager.GetIsFavorite(libraryHierarchyNode).Result;
            }
        }

        public override void InitializeComponent(ICore core)
        {
            this.LibraryManager = core.Managers.Library;
            this.SignalEmitter = core.Components.SignalEmitter;
            this.Configuration = core.Components.Configuration;
            this.ShowFavorites = this.Configuration.GetElement<BooleanConfigurationElement>(
                LibraryFavoritesBehaviourConfiguration.SECTION,
                LibraryFavoritesBehaviourConfiguration.SHOW_FAVORITES_ELEMENT
            );
            base.InitializeComponent(core);
        }

        public IEnumerable<IInvocationComponent> Invocations
        {
            get
            {
                yield return new InvocationComponent(
                    InvocationComponent.CATEGORY_LIBRARY,
                    TOGGLE_FAVORITE,
                    "Favorite",
                    path: "Favorites",
                    attributes: (byte)(InvocationComponent.ATTRIBUTE_SEPARATOR | (this.IsFavorite ? InvocationComponent.ATTRIBUTE_SELECTED : InvocationComponent.ATTRIBUTE_NONE))
                );
                yield return new InvocationComponent(
                    InvocationComponent.CATEGORY_LIBRARY,
                    TOGGLE_SHOW_FAVORITES,
                    "Show Favorites",
                    path: "Favorites",
                    attributes: this.ShowFavorites.Value ? InvocationComponent.ATTRIBUTE_SELECTED : InvocationComponent.ATTRIBUTE_NONE
                );
            }
        }

        public async Task InvokeAsync(IInvocationComponent component)
        {
            switch (component.Id)
            {
                case TOGGLE_FAVORITE:
                    var libraryHierarchyNode = this.LibraryManager.SelectedItem;
                    if (libraryHierarchyNode != null)
                    {
                        await this.LibraryManager.SetIsFavorite(libraryHierarchyNode, !await this.LibraryManager.GetIsFavorite(libraryHierarchyNode));
                        if (this.ShowFavorites.Value)
                        {
                            await this.SignalEmitter.Send(new Signal(this, CommonSignals.HierarchiesUpdated));
                        }
                    }
                    break;
                case TOGGLE_SHOW_FAVORITES:
                    this.ShowFavorites.Toggle();
                    await this.SignalEmitter.Send(new Signal(this, CommonSignals.HierarchiesUpdated));
                    this.Configuration.Save();
                    break;
            }
        }

        public IEnumerable<ConfigurationSection> GetConfigurationSections()
        {
            return LibraryFavoritesBehaviourConfiguration.GetConfigurationSections();
        }
    }
}
