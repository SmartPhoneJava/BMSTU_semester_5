using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;


    /// <summary>
    /// Tests in this class are only used to analyse and research code.
    /// It is not intended to contain real integration tests.
    /// </summary>

public class RliResearchTester
{
    static public void Main()
    {
        ValidateXml(@"D:\GitHub\basedate\5\titles.xml",
            @"D:\GitHub\basedate\5\titles.xsd");
;    }

    /// <summary>
    /// Test xml validation against a XSD file.
    /// </summary>
   

    /// <summary>
    /// Validates a xml file (found at the given xmlFilePath) to a XSD file (found at the given xsdFilePath).
    /// </summary>
    /// <param name="xmlFilePath">The xml file to validate.</param>
    /// <param name="xsdFilePath">The xsd file used in the validation.</param>
    /// <exception cref="ArgumentNullException">
    /// Throws an ArgumentNullException, when xmlFilePath is null, empty or contains only white spaces.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Throws an ArgumentNullException, when xsdFilePath is null, empty or contains only white spaces.
    /// </exception>
    /// <exception cref="ArgumentException">Throws an ArgumentException, when xmlFilePath does not exist.</exception>
    /// <exception cref="ArgumentException">Throws an ArgumentException, when xsdFilePath does not exist.</exception>
    /// <remarks>
    /// - Includes schemas referenced by the given xsd file (recursively).
    /// </remarks>
    static public void ValidateXml(string xmlFilePath, string xsdFilePath)
    {
        if (string.IsNullOrWhiteSpace(xmlFilePath)) { throw new ArgumentNullException("xmlFilePath"); }
        if (string.IsNullOrWhiteSpace(xsdFilePath)) { throw new ArgumentNullException("xsdFilePath"); }
        if (!File.Exists(xmlFilePath))
        {
            throw new ArgumentException(string.Format("File [{0}] not found.", xmlFilePath));
        }
        if (!File.Exists(xsdFilePath))
        {
            throw new ArgumentException(string.Format("File [{0}] not found.", xsdFilePath));
        }

        var schemas = new XmlSchemaSet();

        // Use the target namespace specified in the XSD file. 
        schemas.Add(null, xsdFilePath);

        var readerSettings = new XmlReaderSettings();

        // Validate the xml file to an XSD file not an DTD or XDR.
        readerSettings.ValidationType = ValidationType.Schema;

        // Include schemas referenced by the given xsd file (recursively) in the validation proces.
        readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;

        // Warnings will fire the ValidationEventHandler function.
        readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
        readerSettings.Schemas.Add(schemas);

        // The function [ValidationEventHandler] will be used to handle all validation errors / warnings.
        readerSettings.ValidationEventHandler += ValidationEventHandler;
       
        using (var xmlReader = XmlReader.Create(xmlFilePath, readerSettings))
        {
            while (xmlReader.Read()) {
                
            }    // Validate XML file.


        }
        Console.Read();
    }
    /// <summary>
    /// This event will fire on every XML validation error / warning.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    static public void ValidationEventHandler(object sender, ValidationEventArgs args)
    {
        //if (System.Xml.Schema.XmlSchemaValidationException.Equals(args.Exception.Data))
        System.Collections.IDictionary a = args.Exception.Data;
        Console.WriteLine("{0}", a.ToString());
        Console.WriteLine("{0}  {1}  {2}", args.Exception, args.Exception, args.Exception.LinePosition);
        Console.Read();
    }
}

