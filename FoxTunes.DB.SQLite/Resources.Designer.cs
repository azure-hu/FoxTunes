﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoxTunes {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FoxTunes.DB.SQLite.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT OR IGNORE INTO &quot;MetaDataItems&quot; (&quot;Name&quot;, &quot;Type&quot;, &quot;Value&quot;) 
        ///SELECT @name, @type, @value;
        ///
        ///INSERT OR IGNORE INTO &quot;LibraryItem_MetaDataItem&quot; (&quot;LibraryItem_Id&quot;, &quot;MetaDataItem_Id&quot;)
        ///SELECT @itemId, last_insert_rowid();.
        /// </summary>
        internal static string AddLibraryMetaDataItems {
            get {
                return ResourceManager.GetString("AddLibraryMetaDataItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [MetaDataItems](
        ///	[Id] INTEGER PRIMARY KEY NOT NULL, 
        ///	[Name] text NOT NULL COLLATE NOCASE, 
        ///	[Type] bigint NOT NULL,
        ///	[Value] text COLLATE NOCASE);
        ///
        ///CREATE TABLE [LibraryItems] (
        ///	[Id] INTEGER PRIMARY KEY NOT NULL, 
        ///	[DirectoryName] text NOT NULL COLLATE NOCASE, 
        ///	[FileName] text NOT NULL COLLATE NOCASE, 
        ///	[ImportDate] text NOT NULL,
        ///	[Favorite] bit NOT NULL,
        ///	[Status] INTEGER NOT NULL);
        ///
        ///CREATE TABLE [PlaylistItems](
        ///    [Id] INTEGER PRIMARY KEY NOT NULL, 
        ///	[LibraryItem_Id] INTEGER NULL REFER [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Database {
            get {
                return ResourceManager.GetString("Database", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT &quot;MetaDataItems&quot;.&quot;Value&quot;
        ///FROM &quot;LibraryHierarchyItems&quot;
        ///	JOIN &quot;LibraryHierarchyItem_LibraryItem&quot; ON &quot;LibraryHierarchyItems&quot;.&quot;Id&quot; = &quot;LibraryHierarchyItem_LibraryItem&quot;.&quot;LibraryHierarchyItem_Id&quot;
        ///	JOIN &quot;LibraryItem_MetaDataItem&quot; ON &quot;LibraryHierarchyItem_LibraryItem&quot;.&quot;LibraryItem_Id&quot; = &quot;LibraryItem_MetaDataItem&quot;.&quot;LibraryItem_Id&quot;
        ///	JOIN &quot;MetaDataItems&quot; ON &quot;MetaDataItems&quot;.&quot;Id&quot; = &quot;LibraryItem_MetaDataItem&quot;.&quot;MetaDataItem_Id&quot;
        ///WHERE &quot;LibraryHierarchyItems&quot;.&quot;Id&quot; = @libraryHierarchyItemId 
        ///	AND (@type &amp; &quot;MetaDa [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetLibraryHierarchyMetaData {
            get {
                return ResourceManager.GetString("GetLibraryHierarchyMetaData", resourceCulture);
            }
        }
    }
}
