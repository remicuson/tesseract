using System;
using System.Collections.Generic;
using System.Text;
using Tesseract.Backends;
using Tesseract.Controls;
using System.Xml;
using System.IO;
using System.Reflection;
using Tesseract.Graphics;
using Tesseract.Geometry;

namespace Tesseract.Theming
{
    public class DefaultThemer: ThemerBase
    {
        List<string> styleNames = new List<string>();
        List<Type> styleTypes = new List<Type>();
        List<ControlState> styleStates = new List<ControlState>();
        List<PatternList> styleOperations = new List<PatternList>();
        List<List<Location>> styleLocations = new List<List<Location>>();
        List<List<Path>> stylePaths = new List<List<Path>>();

        public DefaultThemer()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("Tesseract.Theming.DefaultThemer.DefaultStyles.xml"));

            LoadStyles(xml.DocumentElement);
        }

        void LoadStyles(XmlElement theme)
        {
            foreach (XmlNode child in theme)
            {
                if (child.NodeType != XmlNodeType.Element)
                    continue;
                if (child.LocalName != "Style")
                    continue;

                LoadStyle((XmlElement)child);
            }
        }

        void LoadStyle(XmlElement style)
        {
            string name = style.GetAttribute("Name");
            Type type = TypeStore.Find(style.GetAttribute("Type"));
            ControlState state = (ControlState)Enum.Parse(typeof(ControlState), style.GetAttribute("State"));

            PatternList plist = new PatternList();
            List<Location> locations = new List<Location>();
            List<Path> paths = new List<Path>();

            foreach (XmlNode child in style)
            {
                Pattern ptn = null;
                Path pth = null;
                Location loc = null;

                foreach (XmlNode child2 in child)
                {
                    if (child2.NodeType != XmlNodeType.Element)
                        continue;

                    if (ptn == null)
                        ptn = LoadPattern((XmlElement)child2);
                    if (loc == null)
                        loc = LoadLocation((XmlElement)child2);
                    if (pth == null)
                        pth = LoadPath((XmlElement)child2);
                }

                if (ptn == null)
                    continue;

                ptn.Type = (PatternType)Enum.Parse(typeof(PatternType), child.LocalName);

                plist.Add(ptn);
                paths.Add(pth);
                locations.Add(loc);
            }

            styleNames.Add(name);
            styleTypes.Add(type);
            styleStates.Add(state);
            styleOperations.Add(plist);
            styleLocations.Add(locations);
            stylePaths.Add(paths);
        }

        Location LoadLocation(XmlElement n)
        {
            if (n.LocalName == "Location")
                return new Location(null,
                    Distance.FromString(n.GetAttribute("L")),
                    Distance.FromString(n.GetAttribute("T")),
                    Distance.FromString(n.GetAttribute("R")),
                    Distance.FromString(n.GetAttribute("B")));

            return null;
        }

        Path LoadPath(XmlElement n)
        {
            if (n.LocalName == "Rectangle")
                return new Rectangle(Distance.FromString(n.GetAttribute("W")), Distance.FromString(n.GetAttribute("H")));

            return null;
        }

        Pattern LoadPattern(XmlElement n)
        {
            if (n.LocalName == "Solid")
                return new Solid(Color.FromString(n.GetAttribute("Color")));

            if (n.LocalName == "LinearGradient")
            {
                LinearGradient lg = new LinearGradient((LinearGradientOrientation)Enum.Parse(typeof(LinearGradientOrientation), n.GetAttribute("Orientation")));

                foreach (XmlNode child in n)
                {
                    if (child.LocalName != "GradientStop")
                        continue;

                    GradientStop stop = new GradientStop(
                        double.Parse(((XmlElement)child).GetAttribute("Stop")),
                        Color.FromString(((XmlElement)child).GetAttribute("Color")));

                    lg.Add(stop);
                }

                return lg;
            }

            return null;
        }

        public override void InitControl(Control c)
        {
            c.ThemerData = ""; // Needs to be non-null to know its been initialized
        }

        public override string[] GetStyles(Control c)
        {
            List<string> styles = new List<string>();

            for (int i = 0; i < styleNames.Count; i++)
            {
                if (styleTypes[i] == c.GetType())
                    styles.Add(styleNames[i]);

                if ((c.GetType().Assembly != Assembly.GetExecutingAssembly()) && c.GetType().IsSubclassOf(styleTypes[i]))
                    styles.Add(styleNames[i]);
            }

            return styles.ToArray();
        }

        public override void SetStyle(Control c, string style)
        {
            c.ThemerData = style;
        }

        public override void RenderControl(Control c, IGraphics g)
        {
            for (int i = 0; i < styleNames.Count; i++)
            {
                List<Location> locs = styleLocations[i];
                List<Path> paths = stylePaths[i];

                if (styleNames[i] != (string)c.ThemerData)
                    continue;
                if (styleTypes[i] != c.GetType())
                {
                    if ((c.GetType().Assembly == Assembly.GetExecutingAssembly()) || !c.GetType().IsSubclassOf(styleTypes[i]))
                        continue;
                }
                if ((styleStates[i] & c.State) != styleStates[i])
                    continue;

                PatternList plist = styleOperations[i];

                for (int j = 0; j < plist.Count; j++)
                {
                    g.Save();

                    Path path = (paths[j] != null) ? paths[j].Clone() : c.Path.Clone();
                    path.Control = c;

                    if (locs[j] != null)
                    {
                        // This is a little hackish, but I can't think of a better way
                        // Without reworking major chunks of the geometry classes

                        Control tmp = new Control();
                        tmp.Path = path;
                        tmp.Parent = c;
                        path.Control = tmp;

                        Location loc = locs[j].Clone();
                        loc.Control = tmp;

                        g.Translate(loc.RealL, loc.RealT);
                        loc.HandleRel();

                        path = tmp.Path.ClonePixels();
                        tmp.Parent = null;
                    }

                    path.Apply(g);

                    plist[j].Apply(g, path.W, path.H);

                    if (plist[j].Type == PatternType.Fill)
                        g.Fill();
                    else
                        g.Stroke();

                    g.Restore();
                }
            }
        }
    }
}
