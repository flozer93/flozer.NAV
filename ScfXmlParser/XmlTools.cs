using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace flozer.NAV.XmlTools
{
    public class XmlTools
    {
        public static Boolean EscapeXmlFileContent_b(string filePath)
        {
            /// <summary>
            /// Runs EscapeXmlFileContent but with a boolean return
            /// </summary>
            /// <returns>a boolean</returns>
            try
            {
                EscapeXmlFileContent(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void EscapeXmlFileContent(string filePath)
        {
            /// <summary>
            /// Escapes Characters wich are not allowed in XML.
            /// The XML-File will be processed and saved.
            /// </summary>
            /// <param name="filePath">FilePath and Filename of the XML</param>
            try
            {
                string rawXml = File.ReadAllText(filePath);

                // Escape ampersands that are NOT part of valid entities
                string safeXml = Regex.Replace(rawXml, @"&(?!amp;|lt;|gt;|quot;|apos;)", "&amp;");

                // Remove Control Characters
                //safeXml = Regex.Replace(safeXml, @"[\x00-\x1F]", string.Empty);
                safeXml = Regex.Replace(safeXml, @"[\x00-\x08]", string.Empty);
                safeXml = Regex.Replace(safeXml, @"[\x0B-\x0C]", string.Empty);
                safeXml = Regex.Replace(safeXml, @"[\x0E-\x19]", string.Empty);
                //safeXml = Regex.Replace(safeXml, @"[\x7F-\x1F]", string.Empty);

                File.WriteAllText(filePath, safeXml);
            }
            catch (Exception ex)
            {
                // Log or wrap the exception instead of rethrowing raw
                throw new ApplicationException($"Error processing file: {filePath}", ex);
            }
        }
        public static void RenameInnerItems(string filePath, string UpperElements, string UpperElement, string renameFrom, string renameTo)
        {
            /// <summary>
            /// The rename Elements "item" under "/items/item" will be renamed to "textItem"
            /// </summary>
            /// <param name="filePath">FilePath and Filename of the XML</param>
            /// <param name="UpperElements">i.E. "/po/items/item"</param>
            /// <param name="UpperElement">i.E. "text"</param>
            /// <param name="renameFrom">i.E. "item"</param>
            /// <param name="renameTo">i.E. "textItem"</param>
            try
            {
                var doc = new XmlDocument();
                doc.Load(filePath);

                // Alle äußeren <item> innerhalb von <items>
                XmlNodeList outerItems = doc.SelectNodes(UpperElements);

                foreach (XmlNode outerItem in outerItems)
                {
                    XmlNode textNode = outerItem.SelectSingleNode(UpperElement);
                    if (textNode != null)
                    {
                        List<XmlNode> nodesToReplace = new List<XmlNode>();

                        foreach (XmlNode innerItem in textNode.ChildNodes)
                        {
                            if (innerItem.Name == renameFrom)
                            {
                                XmlElement textItem = doc.CreateElement(renameTo);
                                textItem.InnerXml = innerItem.InnerXml;

                                nodesToReplace.Add(innerItem);
                                textNode.InsertBefore(textItem, innerItem);
                            }
                        }

                        // Entferne alte <item>-Knoten
                        foreach (var oldNode in nodesToReplace)
                        {
                            textNode.RemoveChild(oldNode);
                        }
                    }
                }
                //doc.Save(filePath);
                saveXmlWithStdEncoding(filePath, doc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void saveXmlWithStdEncoding(string filePath, XmlDocument doc)
        {
            // Speichern mit Encoding
            using (var writer = XmlWriter.Create(filePath, new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false),
                Indent = true
            }))
            {
                doc.Save(writer);
            }
        }
        /*
        public static Boolean ParseXmlFile_b(string file_par)
        {
            try
            {
                ParseXmlFile(file_par);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public static void ParseXmlFile(string file_par)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(file_par);

                // Loop through every element
                foreach (XmlElement element in doc.DocumentElement.GetElementsByTagName("*"))
                {
                    // Replace content as needed, and let XML handle escaping
                    element.InnerText = element.InnerText; // This triggers automatic escaping
                }
                // Save back to file
                doc.Save(file_par);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
        /*
        public static void RenameTextItems(string filePath)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList textItemNodes = doc.SelectNodes("/items/item/text/item");

            List<XmlNode> toReplace = new List<XmlNode>();

            foreach (XmlNode oldItem in textItemNodes)
            {
                XmlElement newTextItem = doc.CreateElement("textItem");
                newTextItem.InnerXml = oldItem.InnerXml;
                toReplace.Add(oldItem);
                oldItem.ParentNode.InsertBefore(newTextItem, oldItem);
            }

            foreach (var oldNode in toReplace)
            {
                oldNode.ParentNode.RemoveChild(oldNode);
            }
            //doc.Save(filePath);
            saveXmlWithStdEncoding(filePath, doc);
        }
        */
        /*
        public static void RenameInnerItems_ItemsItem_Item(string filePath)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);

            // Alle äußeren <item> innerhalb von <items>
            XmlNodeList outerItems = doc.SelectNodes("/po/items/item");

            foreach (XmlNode outerItem in outerItems)
            {
                XmlNode textNode = outerItem.SelectSingleNode("text");
                if (textNode != null)
                {
                    List<XmlNode> nodesToReplace = new List<XmlNode>();

                    foreach (XmlNode innerItem in textNode.ChildNodes)
                    {
                        if (innerItem.Name == "item")
                        {
                            XmlElement textItem = doc.CreateElement("textItem");
                            textItem.InnerXml = innerItem.InnerXml;

                            nodesToReplace.Add(innerItem);
                            textNode.InsertBefore(textItem, innerItem);
                        }
                    }

                    // Entferne alte <item>-Knoten
                    foreach (var oldNode in nodesToReplace)
                    {
                        textNode.RemoveChild(oldNode);
                    }
                }
            }
            //doc.Save(filePath);
            saveXmlWithStdEncoding(filePath, doc);
        }
        public static void RenameInnerItems_ItemsItem_Item_o(string filePath)
        {
            /// <summary>
            /// The Elements "item" under "/items/item" will be renamed to "textItem"
            /// </summary>
            /// <param name="filePath">FilePath and Filename of the XML</param>
            try
            {
                var doc = new XmlDocument();
                doc.Load(filePath);

                // Alle äußeren <item> innerhalb von <items>
                XmlNodeList outerItems = doc.SelectNodes("/items/item");

                foreach (XmlNode outerItem in outerItems)
                {
                    XmlNode textNode = outerItem.SelectSingleNode("text");
                    if (textNode != null)
                    {
                        List<XmlNode> nodesToReplace = new List<XmlNode>();

                        foreach (XmlNode innerItem in textNode.ChildNodes)
                        {
                            if (innerItem.Name == "item")
                            {
                                XmlElement textItem = doc.CreateElement("textItem");
                                textItem.InnerXml = innerItem.InnerXml;

                                nodesToReplace.Add(innerItem);
                                textNode.InsertBefore(textItem, innerItem);
                            }
                        }

                        // Entferne alte <item>-Knoten
                        foreach (var oldNode in nodesToReplace)
                        {
                            textNode.RemoveChild(oldNode);
                        }
                    }
                }
                //doc.Save(filePath);
                saveXmlWithStdEncoding(filePath, doc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
        /*
        public static void EscapeXmlFileContent(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filePath);

                // Use recursion to traverse and clean every element
                EscapeAllElements(doc.DocumentElement);

                // Save the cleaned XML
                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                // Log or wrap the exception instead of rethrowing raw
                throw new ApplicationException($"Error processing file: {filePath}", ex);
            }
        }
        private static void EscapeAllElements(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    // Recursively process child elements
                    EscapeAllElements(child);
                }
                else if (child.NodeType == XmlNodeType.Text || child.NodeType == XmlNodeType.CDATA)
                {
                    // Reassigning triggers escaping
                    child.InnerText = child.InnerText;
                }
            }
        }
        */
    }
}