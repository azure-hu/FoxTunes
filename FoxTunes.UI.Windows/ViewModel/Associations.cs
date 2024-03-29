﻿using FoxTunes.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FoxTunes.ViewModel
{
    public class Associations : ViewModelBase
    {
        public Associations()
        {
            this.FileAssociations = new ObservableCollection<Association>();
        }

        public IOutput Output { get; private set; }

        public ObservableCollection<Association> FileAssociations { get; private set; }

        public IEnumerable<IFileAssociation> Enabled
        {
            get
            {
                return this.FileAssociations
                    .Where(association => association.FileAssociation != null && association.IsSelected)
                    .Select(association => association.FileAssociation);
            }
        }

        public IEnumerable<IFileAssociation> Disabled
        {
            get
            {
                return this.FileAssociations
                    .Where(association => association.FileAssociation != null && !association.IsSelected)
                    .Select(association => association.FileAssociation);
            }
        }

        public override void InitializeComponent(ICore core)
        {
            this.Output = this.Core.Components.Output;
            this.Refresh();
            base.InitializeComponent(core);
        }

        protected virtual void Refresh()
        {
            var fileAssociations = ComponentRegistry.Instance.GetComponent<IFileAssociations>();
            if (fileAssociations == null)
            {
                return;
            }
            var extensions = fileAssociations.Associations.Select(
                association => association.Extension.TrimStart('.')
            ).ToArray();
            this.FileAssociations.Clear();
            this.FileAssociations.AddRange(
                this.Output.SupportedExtensions.Select(extension => new Association(
                    fileAssociations.Create(extension),
                    extensions.Contains(extension)
                ))
            );
            this.FileAssociations.Add(new Association(
                null,
                this.FileAssociations.Count == extensions.Length
            ));
        }

        public ICommand SelectAllCommand
        {
            get
            {
                return new Command<bool>(selectAll => this.SelectAll(selectAll));
            }
        }

        public void SelectAll(bool selectAll)
        {
            if (selectAll)
            {
                foreach (var association in this.FileAssociations)
                {
                    association.IsSelected = true;
                }
            }
            else
            {
                this.Refresh();
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new Command(this.Save)
                {
                    Tag = CommandHints.DISMISS
                };
            }
        }

        public void Save()
        {
            var fileAssociations = ComponentRegistry.Instance.GetComponent<IFileAssociations>();
            if (fileAssociations == null)
            {
                return;
            }
            if (this.Enabled.Any())
            {
                fileAssociations.Enable();
            }
            else
            {
                fileAssociations.Disable();
            }
            fileAssociations.Disable(this.Disabled);
            fileAssociations.Enable(this.Enabled);
            this.Refresh();
        }

        public ICommand CancelCommand
        {
            get
            {
                return new Command(this.Cancel)
                {
                    Tag = CommandHints.DISMISS
                };
            }
        }

        public void Cancel()
        {
            this.Refresh();
        }

        protected override Freezable CreateInstanceCore()
        {
            return new Associations();
        }
    }

    public class Association : ViewModelBase
    {
        public Association(IFileAssociation fileAssociation, bool isSelected)
        {
            this.FileAssociation = fileAssociation;
            this.IsSelected = isSelected;
        }

        public IFileAssociation FileAssociation { get; private set; }

        private bool _IsSelected { get; set; }

        public bool IsSelected
        {
            get
            {
                return this._IsSelected;
            }
            set
            {
                this._IsSelected = value;
                this.OnIsSelectedChanged();
            }
        }

        protected virtual void OnIsSelectedChanged()
        {
            if (this.IsSelectedChanged != null)
            {
                this.IsSelectedChanged(this, EventArgs.Empty);
            }
            this.OnPropertyChanged("IsSelected");
        }

        public event EventHandler IsSelectedChanged;

        protected override Freezable CreateInstanceCore()
        {
            return new Association(null, false);
        }
    }
}
