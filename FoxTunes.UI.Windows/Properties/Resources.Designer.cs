﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoxTunes.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FoxTunes.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to (function () {
        ///    if (item == null) {
        ///        return version();
        ///    }
        ///    var parts = [];
        ///    if (tag.disccount != 1 &amp;&amp; tag.disc) {
        ///        parts.push(tag.disc);
        ///    }
        ///    if (tag.track) {
        ///        parts.push(zeropad(tag.track, 2));
        ///    }
        ///    var artist = tag.firstalbumartist || tag.firstalbumartistsort || tag.firstartist;
        ///    if (artist) {
        ///        parts.push(artist);
        ///    }
        ///    if (tag.album) {
        ///        parts.push(tag.album);
        ///    }
        ///    if (tag.title) {
        ///        parts.push(tag.title);
        ///    } [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string NowPlaying {
            get {
                return ResourceManager.GetString("NowPlaying", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (function () {
        ///    if (item == null) {
        ///        return version();
        ///    }
        ///    var parts = [];
        ///    if (tag.disccount != 1 &amp;&amp; tag.disc) {
        ///        parts.push(tag.disc);
        ///    }
        ///    if (tag.track) {
        ///        parts.push(zeropad(tag.track, 2));
        ///    }
        ///    if (tag.title) {
        ///        parts.push(tag.title);
        ///    }
        ///    else {
        ///        parts.push(filename(item.FileName));
        ///    }
        ///    return parts.join(&quot; - &quot;);
        ///})().
        /// </summary>
        internal static string Playlist {
            get {
                return ResourceManager.GetString("Playlist", resourceCulture);
            }
        }
    }
}
