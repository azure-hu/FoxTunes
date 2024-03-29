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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FoxTunes.Scripting.JS.Resources", typeof(Resources).Assembly);
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
        ///    if (tag.__ft_variousartists) {
        ///        return &quot;Various Artists&quot;;
        ///    }
        ///    return tag.artist || &quot;No Artist&quot;;
        ///})().
        /// </summary>
        internal static string Artist {
            get {
                return ResourceManager.GetString("Artist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (function () {
        ///    var parts = [tag.artist || &quot;No Artist&quot;];
        ///    if (tag.album) {
        ///        parts.push(tag.album);
        ///    }
        ///    return parts.join(&quot; - &quot;);
        ///})().
        /// </summary>
        internal static string Artist_Album {
            get {
                return ResourceManager.GetString("Artist_Album", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (extension(item.FileName) || &quot;&quot;).toUpperCase();.
        /// </summary>
        internal static string Codec {
            get {
                return ResourceManager.GetString("Codec", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (function () {
        ///    if (tag.title) {
        ///        var parts = [];
        ///        if (parseInt(tag.disccount) != 1 &amp;&amp; parseInt(tag.disc)) {
        ///            parts.push(tag.disc);
        ///        }
        ///        if (tag.track) {
        ///            parts.push(zeropad(tag.track, 2));
        ///        }
        ///        parts.push(tag.title);
        ///        return parts.join(&quot; - &quot;);
        ///    } return filename(fileName);
        ///})().
        /// </summary>
        internal static string Disk_Track_Title {
            get {
                return ResourceManager.GetString("Disk_Track_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to timestamp(property.duration).
        /// </summary>
        internal static string Duration {
            get {
                return ResourceManager.GetString("Duration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ucfirst(tag.genre) || &quot;No Genre&quot;.
        /// </summary>
        internal static string Genre {
            get {
                return ResourceManager.GetString("Genre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to playing != null &amp;&amp; item.Id == playing.Id &amp;&amp; item.FileName == playing.FileName ? &quot;\u2022&quot; : &quot;&quot;.
        /// </summary>
        internal static string Playing {
            get {
                return ResourceManager.GetString("Playing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (function () {
        ///    var parts = [];
        ///    if (tag.title) {
        ///        parts.push(tag.title);
        ///    }
        ///    if (tag.performer &amp;&amp; tag.performer != tag.artist) {
        ///        parts.push(tag.performer);
        ///    }
        ///    if (parts.length) {
        ///        return parts.join(&quot; - &quot;);
        ///    }
        ///    else {
        ///        return filename(item.FileName);
        ///    }
        ///})().
        /// </summary>
        internal static string Title_Performer {
            get {
                return ResourceManager.GetString("Title_Performer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (function () {
        ///    var parts = [];
        ///    if (tag.disccount != 1 &amp;&amp; tag.disc) {
        ///        parts.push(tag.disc);
        ///    }
        ///    if (tag.track) {
        ///        parts.push(zeropad(tag.track, 2));
        ///    }
        ///    return parts.join(&quot; - &quot;);
        ///})().
        /// </summary>
        internal static string Track {
            get {
                return ResourceManager.GetString("Track", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to function version() {
        ///    return &quot;Fox Tunes 2.0.0&quot;;
        ///}
        ///
        ///function timestamp(value) {
        ///
        ///    if (!value) {
        ///        return value;
        ///    }
        ///
        ///    var s = parseInt((value / 1000) % 60);
        ///    var m = parseInt((value / (1000 * 60)) % 60);
        ///    var h = parseInt((value / (1000 * 60 * 60)) % 24);
        ///
        ///    var parts = [];
        ///
        ///    if (h &gt; 0) {
        ///        if (h &lt; 10) {
        ///            h = &quot;0&quot; + h;
        ///        }
        ///        parts.push(h);
        ///    }
        ///
        ///    if (m &lt; 10) {
        ///        m = &quot;0&quot; + m;
        ///    }
        ///    parts.push(m);
        ///    if (s &lt; 10) { [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string utils {
            get {
                return ResourceManager.GetString("utils", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (function () {
        ///    if (tag.album) {
        ///        var parts = [];
        ///        if (tag.year) {
        ///            parts.push(tag.year);
        ///        }
        ///        parts.push(tag.album);
        ///        return parts.join(&quot; - &quot;);
        ///    }
        ///    return &quot;No Album&quot;;
        ///})().
        /// </summary>
        internal static string Year_Album {
            get {
                return ResourceManager.GetString("Year_Album", resourceCulture);
            }
        }
    }
}
