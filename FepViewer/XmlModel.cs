﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace FepViewer
{
    [Serializable]
    [XmlRoot(ElementName = "root")]
    public class XmlRootItem
    {
        [XmlElement(ElementName = "item")]
        public XmlChildItem Item { get; set; }
    }

    [Serializable]
    public class XmlChildItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [XmlElement(ElementName = "duration")]
        public double Duration { get; set; }

        [XmlElement(ElementName = "calls")]
        public double Calls { get; set; }

        [XmlElement(ElementName = "kilobytes")]
        public double Kilobytes { get; set; }

        [XmlElement(ElementName = "expression")]
        public string Expression { get; set; }

        [XmlArray("children")]
        [XmlArrayItem("item")]
        public XmlChildItem[] Children { get; set; }

        public bool IsExpanded { get; set; } = false;

        public bool IsSelected { get; set; } = false;

        public string DurationFormated => TimeSpan.FromMilliseconds(Duration).ToString("mm':'ss'.'fff");

        public string CallsFormated => $"{Calls:N0} x";

        public string BytesFormated
        {
            get
            {
                if (Kilobytes < 0)
                {
                    return $"- kB";
                }

                string[] sizes = { "kB", "MB", "GB", "TB" };
                double len = Kilobytes;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }
                return $"{len:0.##} {sizes[order]}";
            }
        }

        public void ExpandAllFirst()
        {
            IsExpanded = true;
            if (Children.Length > 0)
            {
                Children[0].ExpandAllFirst();
            }
        }
    }
}