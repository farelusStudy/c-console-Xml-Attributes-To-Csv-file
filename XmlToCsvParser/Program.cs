using System;
using System.IO;
using System.Text;
using System.Xml;

namespace XmlToCsvParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter xmls directory or xml file path: ");
            string path = Console.ReadLine();
            Console.WriteLine("Enter output csv file path: ");
            string csvPath = Console.ReadLine();

            string[] attrs = { "last_name", "first_name", "middle_name", "birthdate" };

            File.WriteAllText(csvPath, String.Join(';', attrs) + '\n', Encoding.UTF8);

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string s in files)
                {
                    if (s.Contains(".xml"))
                        XmlAttrsToCsv(s, csvPath, attrs);
                }
            }
            else if (File.Exists(path))
            {
                XmlAttrsToCsv(path, csvPath, attrs);
            }

            Console.WriteLine("Done!");
        }

        static void XmlAttrsToCsv(string xmlFileName, string csvFileName, string[] attrs)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xmlFileName);

                XmlElement xRoot = xDoc.DocumentElement;

                StringBuilder csv = new StringBuilder();

                foreach (XmlNode xnode in xRoot)
                {
                    if (xnode.Attributes.Count > 0)
                    {
                        for (int i = 0; i < attrs.Length; i++)
                        {
                            XmlNode node = xnode.Attributes.GetNamedItem(attrs[i]);
                            if (node != null)
                            {
                                csv.Append(node.Value);
                                if (i != attrs.Length - 1)
                                    csv.Append(';');
                            }
                        }
                        csv.Append('\n');
                    }
                }

                File.AppendAllText(csvFileName, csv.ToString(), Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"File: {xmlFileName}");
                Console.WriteLine(e);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
