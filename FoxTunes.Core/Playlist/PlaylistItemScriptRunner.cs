﻿using FoxTunes.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace FoxTunes
{
    public class PlaylistItemScriptRunner
    {
        public PlaylistItemScriptRunner(IScriptingContext scriptingContext, PlaylistItem playlistItem, string script)
            : this(null, scriptingContext, playlistItem, script)
        {

        }

        public PlaylistItemScriptRunner(IPlaybackManager playbackManager, IScriptingContext scriptingContext, PlaylistItem playlistItem, string script)
        {
            this.PlaybackManager = playbackManager;
            this.ScriptingContext = scriptingContext;
            this.PlaylistItem = playlistItem;
            this.Script = script;
        }

        public IPlaybackManager PlaybackManager { get; private set; }

        public IScriptingContext ScriptingContext { get; private set; }

        public PlaylistItem PlaylistItem { get; private set; }

        public string Script { get; private set; }

        public void Prepare()
        {
            var collections = new Dictionary<MetaDataItemType, Dictionary<string, object>>()
            {
                { MetaDataItemType.Tag, new Dictionary<string, object>() },
                { MetaDataItemType.Property, new Dictionary<string, object>() }
            };
            if (this.PlaylistItem != null)
            {
                if (this.PlaylistItem.MetaDatas != null)
                {
                    foreach (var item in this.PlaylistItem.MetaDatas)
                    {
                        if (!collections.ContainsKey(item.Type))
                        {
                            continue;
                        }
                        var key = item.Name.ToLower();
                        if (collections[item.Type].ContainsKey(key))
                        {
                            //Not sure what to do. Doesn't happen often.
                            continue;
                        }
                        collections[item.Type].Add(key, item.Value);
                    }
                }
            }
            if (this.PlaybackManager != null)
            {
                this.ScriptingContext.SetValue("playing", this.PlaybackManager.CurrentStream);
            }
            else
            {
                this.ScriptingContext.SetValue("playing", null);
            }
            this.ScriptingContext.SetValue("item", this.PlaylistItem);
            this.ScriptingContext.SetValue("tag", collections[MetaDataItemType.Tag]);
            this.ScriptingContext.SetValue("property", collections[MetaDataItemType.Property]);
        }

        [DebuggerNonUserCode]
        public object Run()
        {
            const string RESULT = "__result";
            try
            {
                this.ScriptingContext.Run(string.Concat("var ", RESULT, " = ", this.Script, ";"));
            }
            catch (ScriptingException e)
            {
                return e.Message;
            }
            return this.ScriptingContext.GetValue(RESULT);
        }
    }
}
