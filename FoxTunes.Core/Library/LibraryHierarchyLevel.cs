﻿using FoxTunes.Interfaces;
using System;

namespace FoxTunes
{
    public class LibraryHierarchyLevel : PersistableComponent, ISequenceableComponent
    {
        public LibraryHierarchyLevel()
        {

        }

        public int LibraryHierarchyId { get; set; }

        private int _Sequence { get; set; }

        public int Sequence
        {
            get
            {
                return this._Sequence;
            }
            set
            {
                this._Sequence = value;
                this.OnSequenceChanged();
            }
        }

        protected virtual void OnSequenceChanged()
        {
            if (this.SequenceChanged != null)
            {
                this.SequenceChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("Sequence");
        }

        public event EventHandler SequenceChanged;

        private string _Name { get; set; }

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
                this.OnNameChanged();
            }
        }

        protected virtual void OnNameChanged()
        {
            if (this.NameChanged != null)
            {
                this.NameChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("Name");
        }

        public event EventHandler NameChanged;

        private string _Script { get; set; }

        public string Script
        {
            get
            {
                return this._Script;
            }
            set
            {
                this._Script = value;
                this.OnScriptChanged();
            }
        }

        protected virtual void OnScriptChanged()
        {
            if (this.ScriptChanged != null)
            {
                this.ScriptChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("Script");
        }

        public event EventHandler ScriptChanged;
    }
}
