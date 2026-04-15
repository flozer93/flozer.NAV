using System;
using flozer.NAV;
using flozer.NAV.XmlTools;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Test");
                /*
                string name_loc = "C:\\NAV\\PO_B25-04979_808201_2025-07-10-13-05-51.xml";
                //XmlTools.ParseXmlFile(name_loc);
                //Console.WriteLine(XmlTools.ParseXmlFile_b(name_loc));
                */

                //XmlTools.EscapeXmlFileContent(name_loc);
                string name_loc2 = "C:\\NAV\\PO_B25-04979_808201_2025-07-10-13-05-51_copy.xml";
                //string name_loc2 = "C:\\NAV\\PO_B25-04251_803655_2025-07-07-10-58-09_copy.xml";
                XmlTools.EscapeXmlFileContent(name_loc2);
                //XmlTools.RenameTextItems(name_loc2);
                
                XmlTools.RenameInnerItems(name_loc2, "//head", "payTermFull", "item", "textItem");
                XmlTools.RenameInnerItems(name_loc2, "//items/item", "text", "item", "textItem");
                XmlTools.RenameInnerItems(name_loc2, "//items/item", "scheds", "item", "textItem");

                //XmlTools.RenameInnerItems_ItemsItem_Item(name_loc2);

                Console.WriteLine("Press any key to finish...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem...");
                throw ex;
            }
        }
    }
}
