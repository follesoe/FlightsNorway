using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System;

namespace Phone.Controls.Samples.Build.Tasks
{
    public class MergeStylesTask : Task
    {
        private const string FilePath = "Themes";
        private const string FileName = "generic.xaml";
        private const string XmlRoot = "ResourceDictionary";
        private const string XmlNS = "xmlns";
        private const string XmlStyle = "Style";
        private const string XmlTarget = "TargetType";

        private SortedDictionary<string, string> Namespaces = new SortedDictionary<string, string>();
        private SortedDictionary<string, string> TargetFiles = new SortedDictionary<string, string>();
        private SortedDictionary<string, XElement> TargetStyles = new SortedDictionary<string, XElement>();

        public MergeStylesTask()
        {
        }

        [Required]
        public string ProjectDirectory { get; set; }

        [Required]
        public ITaskItem[] Styles { get; set; }

        public override bool Execute()
        {
            string targetPath = Path.Combine(ProjectDirectory, FilePath);
            string targetName = Path.Combine(targetPath, FileName);

            foreach (var item in Styles)
            {
                XElement root = XElement.Load(item.ItemSpec, LoadOptions.PreserveWhitespace);
                if (root.Name.LocalName != XmlRoot)
                    throw new InvalidDataException(
                        string.Format("Unknown root element name '{0}' in {1}",
                        root.Name.LocalName,
                        item.ItemSpec));

                // namespaces
                foreach (var attribute in root.Attributes())
                {
                    string ns = attribute.Name.LocalName;
                    string val = attribute.Value;
                    if (ns == XmlNS) ns = "";

                    // already in dictionary ?
                    if (Namespaces.ContainsKey(ns))
                    {
                        // check for mismatch namespace
                        if (Namespaces[ns] != val)
                            throw new InvalidDataException(
                                string.Format("Namespace '{0}' in {1} already exists but is different",
                                attribute.Name.LocalName,
                                item.ItemSpec));
                        continue;
                    }
                    // add to namespace dictionary
                    Namespaces.Add(ns, val);
                }

                // styles
                foreach (var element in root.Elements())
                {
                    string style = element.Name.LocalName;
                    string type = element.Attribute(XmlTarget).Value;

                    if (element.Name.LocalName != XmlStyle)
                        throw new InvalidDataException(
                            string.Format("Unknown element name '{0}' in {1}",
                            element.Name.LocalName,
                            item.ItemSpec));

                    // already in dictionary ?
                    if (TargetStyles.ContainsKey(style))
                        throw new InvalidDataException(
                            string.Format("Duplicate style '{0}' in {1}",
                            type,
                            item.ItemSpec));

                    TargetFiles.Add(type, item.ItemSpec);
                    TargetStyles.Add(type, element);
                }
            }

            // merge styles into Themes\generic.xaml
            string defaultNamespace = XNamespace.Xml.NamespaceName;
            Namespaces.TryGetValue("", out defaultNamespace);
            XElement resources = new XElement(XName.Get(XmlRoot, defaultNamespace));
            foreach (var ns in Namespaces)
            {
                // default namespace is added automatically
                if (string.IsNullOrEmpty(ns.Key)) continue;
                resources.Add(new XAttribute(XNamespace.Xmlns + ns.Key, ns.Value));
            }

            resources.Add(new XText(Environment.NewLine));
            foreach (var target in TargetStyles)
            {
                string style = target.Key;
                string file = TargetFiles[style];
                XElement element = target.Value;

                resources.Add(
                    new XText(Environment.NewLine),
                    new XComment(Environment.NewLine +
                        string.Format("  Style : {0}", style) + Environment.NewLine +
                        string.Format("  File : {0}", file) + Environment.NewLine),
                    new XText(Environment.NewLine),
                    new XText("    "),
                    element,
                    new XText(Environment.NewLine));
            }

            // create the document
            XDocument document = new XDocument(
                new XComment(Environment.NewLine +
                    "  WARNING : This file was auto-generated." + Environment.NewLine +
                    "  DO NOT MODIFY" + Environment.NewLine),
                new XText(Environment.NewLine),
                new XText(Environment.NewLine),
                resources);

            // create directory and unprotect file
            try
            {
                Directory.CreateDirectory(targetPath);
                File.SetAttributes(targetName, FileAttributes.Normal);
            }
            catch { };

            // flush xaml to file
            File.WriteAllText(targetName, document.ToString());

            return true;
        }
    }
}
