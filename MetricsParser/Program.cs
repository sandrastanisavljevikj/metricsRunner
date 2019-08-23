using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

public class Program
{
    public static void Main()
    {
        Console.Write("Put the fullpath location of the file...");

        var fullpath = Console.ReadLine();
        Console.WriteLine(File.Exists(fullpath) ? "File exists." : "File does not exist.");

        if (File.Exists(fullpath))
        {
            var directory = Path.GetDirectoryName(fullpath);
            var xDocument = XDocument.Load(fullpath);
            string xml = xDocument.ToString();
            XDocument doc = XDocument.Parse(xml);
            var allMethods = doc.Descendants("Method");

            List<XElement> elementsWithBadMetrics = new List<XElement>();
            foreach (var method in allMethods)
            {
                if (method.HasElements)
                {
                    var metricElements = method.Element("Metrics").Descendants();
                    if(CheckElements(metricElements))
                    {
                        using (StreamWriter file =
                            new StreamWriter($"{directory}\\output.xml", true))
                        {
                            file.WriteLine(method);
                        }
                        Console.WriteLine("{0}", method);
                    }
                }
            }
        }
        else
            return;


        Console.ReadKey();
    }

    private static bool CheckElements(IEnumerable<XElement> elements)
    {
        foreach(var element in elements)
        {
            if(element.Attribute("Name").Value == "MaintainabilityIndex")
            {
                if (int.Parse(element.Attribute("Value").Value) < 50) return true;  
            }
            if (element.Attribute("Name").Value == "CyclomaticComplexity")
            {
                if (int.Parse(element.Attribute("Value").Value) > 10) return true;
            }
            if (element.Attribute("Name").Value == "ClassCoupling")
            {
                if (int.Parse(element.Attribute("Value").Value) > 32) return true;
            }
            if (element.Attribute("Name").Value == "LinesOfCode")
            {
                if (int.Parse(element.Attribute("Value").Value) > 40) return true;
            }
        }
        return false;
    }
}