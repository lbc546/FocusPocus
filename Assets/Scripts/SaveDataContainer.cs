using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

// File used to save data in XML format. Don't worry too much about it.
[XmlRoot("ResponseData")]
public class SaveDataContainer {

    // Define that we're saving an array (in this case list) of items
    [XmlArray("Responses"), XmlArrayItem("ResponseData")]
    public List<ResponseData> Responses = new List<ResponseData>();

    // Save the array in XML format.
    public void Save(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveDataContainer));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    // Load the XML file in as a SaveDataContainer object with all the responses.
    public static SaveDataContainer Load(string path)
    {
        if (File.Exists(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SaveDataContainer));
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as SaveDataContainer;
            }
        }
        else
        {
            return new SaveDataContainer();
        }
    }

    // Load in by text if we want to create data or something. String
    // has to be in XML format with appropriate tags and such.
    public static SaveDataContainer LoadFromText(string text)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveDataContainer));
        return serializer.Deserialize(new StringReader(text)) as SaveDataContainer;
    }
}
