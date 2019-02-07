﻿using System;
using System.IO;
using System.Windows;

namespace FoxTunes
{
    public class ExpressionDarkTheme : ThemeBase
    {
        public ExpressionDarkTheme()
            : base("3E9EFE8C-5245-4F8B-97D1-EB47CC70E373", "ExpressionDark")
        {
            this.ResourceDictionary = new ResourceDictionary()
            {
                Source = new Uri("/FoxTunes.UI.Windows.Themes;component/Themes/ExpressionDark.xaml", UriKind.Relative)
            };
        }

        public ResourceDictionary ResourceDictionary { get; private set; }

        public override Stream ArtworkPlaceholder
        {
            get
            {
                return typeof(ExpressionDarkTheme).Assembly.GetManifestResourceStream("FoxTunes.UI.Windows.Themes.Images.ExpressionDark_Artwork.png");
            }
        }

        public override void Enable()
        {
            Application.Current.Resources.MergedDictionaries.Add(this.ResourceDictionary);
        }

        public override void Disable()
        {
            Application.Current.Resources.MergedDictionaries.Remove(this.ResourceDictionary);
        }
    }
}
