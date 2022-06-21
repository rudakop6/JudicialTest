using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace JudicialTest
{
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        private readonly string _nameFile;
        
        public SerializableDictionary(string nameFile)
        {
            _nameFile = nameFile;
        }

        public System.Xml.Schema.XmlSchema GetSchema() => null;


        public void Reader()
        {
            using (FileStream fileStream = new FileStream(_nameFile, FileMode.Open))
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                ReadXml(System.Xml.XmlReader.Create(streamReader, new System.Xml.XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreWhitespace = true,
                    ConformanceLevel = System.Xml.ConformanceLevel.Fragment
                }));
            }
        }
        public void Writer()
        {
            using (FileStream fileStream = new FileStream(_nameFile, FileMode.Create))
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                WriteXml(System.Xml.XmlWriter.Create(streamWriter, new System.Xml.XmlWriterSettings
                {
                    ConformanceLevel = System.Xml.ConformanceLevel.Auto

                }));
            }
        }
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
                return;

            while (!reader.EOF)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadEndElement();

                this.Add(key, value);
            }
            reader.Dispose();
        }


        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteWhitespace("\n  ");

                writer.WriteStartElement("key");
                writer.WriteWhitespace("\n    ");
                keySerializer.Serialize(writer, key);
                writer.WriteWhitespace("\n  ");
                writer.WriteEndElement();

                writer.WriteWhitespace("\n  ");

                writer.WriteStartElement("value");
                TValue value = this[key];
                writer.WriteWhitespace("\n    ");
                valueSerializer.Serialize(writer, value);
                writer.WriteWhitespace("\n  ");
                writer.WriteEndElement();

                writer.WriteWhitespace("\n");
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");
            }
            writer.Dispose();
        }
    }
}
