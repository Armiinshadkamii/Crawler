using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using crawler.DataSets;

namespace crawler
{
    public class XOutput
    {
        public static void TreeXmlOutPut(HashSet<Tree> linksTree, XmlWriter xM)
        {
            foreach (var link in linksTree)
            {
                xM.WriteStartElement("link");

                if (link.Node.Contains('?'))
                {
                    string[] nodeUriParts = link.Node.Split('?');

                    xM.WriteElementString("parent", link.Parent);
                    xM.WriteElementString("name", nodeUriParts[0]);
                    xM.WriteElementString("query", nodeUriParts[1]);
                }
                else
                {
                    xM.WriteElementString("parent", link.Parent);
                    xM.WriteElementString("name", link.Node);
                }

                xM.WriteStartElement("children");
                foreach (var childLink in link.trees.ToHashSet())
                {
                    xM.WriteStartElement("link");

                    if (childLink.Node.Contains('?'))
                    {
                        string[] childNodeUriParts = childLink.Node.Split('?');

                        xM.WriteElementString("parent", link.Parent);
                        xM.WriteElementString("name", childNodeUriParts[0]);
                        xM.WriteElementString("query", childNodeUriParts[1]);
                    }
                    else
                    {
                        xM.WriteElementString("parent", childLink.Parent);
                        xM.WriteElementString("name", childLink.Node);
                    }

                    xM.WriteStartElement("children");
                    TreeXmlOutPut(childLink.trees, xM);
                    xM.WriteEndElement();

                    xM.WriteEndElement();
                }
                xM.WriteEndElement();

                xM.WriteEndElement();
            }
        }

        public static void ListXmlOutPut(HashSet<SingleList> list, XmlWriter xM)
        {
            foreach (var link in list)
            {
                if (link.Node.Contains('?'))    // if a uri has queries, split them.
                {
                    string[] uriParts = link.Node.Split('?');

                    xM.WriteStartElement("link");

                    xM.WriteElementString("parent", link.Parent);
                    xM.WriteElementString("name", uriParts[0]);
                    xM.WriteElementString("query", uriParts[1]);

                    xM.WriteEndElement();
                }
                else
                {
                    xM.WriteStartElement("link");

                    xM.WriteElementString("parent", link.Parent);
                    xM.WriteElementString("name", link.Node);

                    xM.WriteEndElement();
                }
            }
        }

        public static void MakeXmlTree(string uri ,HashSet<Tree> trees, string path, string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            //XmlWriter xWriter2 = XmlWriter.Create("F:\\Courses\\C#\\Crawler\\crawler\\result.xml", settings);

            XmlWriter xWriter2 = XmlWriter.Create($"{path}\\{name}", settings);
            xWriter2.WriteStartDocument();

            xWriter2.WriteStartElement("rootlink");
            xWriter2.WriteElementString("name", uri);

            xWriter2.WriteStartElement("children");

            XOutput.TreeXmlOutPut(trees, xWriter2);

            xWriter2.WriteEndElement();
            xWriter2.WriteEndElement();

            xWriter2.WriteEndDocument();
            xWriter2.Close();
        }

        public static void MakeXmlList(string uri, HashSet<SingleList> list, string path, string name)
        {
            XmlWriterSettings settings1 = new XmlWriterSettings();
            settings1.Indent = true;

            XmlWriter xWriter1 = XmlWriter.Create($"{path}\\{name}", settings1);
            xWriter1.WriteStartDocument();

            xWriter1.WriteStartElement("rootlink");
            xWriter1.WriteElementString("name", uri);
            xWriter1.WriteStartElement("children");

            XOutput.ListXmlOutPut(list, xWriter1);

            xWriter1.WriteEndElement();
            xWriter1.WriteEndElement();

            xWriter1.WriteEndDocument();
            xWriter1.Close();
        }
    }
}
