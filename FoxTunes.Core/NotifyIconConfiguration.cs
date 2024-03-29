﻿using System.Collections.Generic;

namespace FoxTunes
{
    public static class NotifyIconConfiguration
    {
        public const string SECTION = "F9B3FAE5-87BD-486F-9C23-B8B11A8FDAA9";

        public const string ENABLED_ELEMENT = "AAAA1AC8-7D75-43C9-9E99-FF69EC5D8040";

        public const string MINIMIZE_TO_TRAY_ELEMENT = "BBBB2156-2E0E-4EA3-AFAA-6775CF3421F3";

        public const string CLOSE_TO_TRAY_ELEMENT = "CCCC012A-C958-4964-9574-D57196F36D21";

        public static IEnumerable<ConfigurationSection> GetConfigurationSections()
        {
            yield return new ConfigurationSection(SECTION, "Tray Icon")
                .WithElement(
                    new BooleanConfigurationElement(ENABLED_ELEMENT, "Enabled").WithValue(false))
                .WithElement(
                    new BooleanConfigurationElement(MINIMIZE_TO_TRAY_ELEMENT, "Minimize To Tray").WithValue(true))
                .WithElement(
                    new BooleanConfigurationElement(CLOSE_TO_TRAY_ELEMENT, "Close To Tray").WithValue(false)
            );
            StandardComponents.Instance.Configuration.GetElement<BooleanConfigurationElement>(SECTION, ENABLED_ELEMENT).ConnectValue(mode => UpdateConfiguration(mode));
        }

        private static void UpdateConfiguration(bool mode)
        {
            if (mode)
            {
                StandardComponents.Instance.Configuration.GetElement(SECTION, MINIMIZE_TO_TRAY_ELEMENT).Show();
                StandardComponents.Instance.Configuration.GetElement(SECTION, CLOSE_TO_TRAY_ELEMENT).Show();
            }
            else
            {
                StandardComponents.Instance.Configuration.GetElement(SECTION, MINIMIZE_TO_TRAY_ELEMENT).Hide();
                StandardComponents.Instance.Configuration.GetElement(SECTION, CLOSE_TO_TRAY_ELEMENT).Hide();
            }
        }
    }
}
