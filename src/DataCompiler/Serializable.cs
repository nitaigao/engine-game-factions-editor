using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DataCompiler
{
    public abstract class Serializable
    {
        public XmlNode Serialize( )
        {
            var xmlnsEmpty = new XmlSerializerNamespaces( );
            xmlnsEmpty.Add( string.Empty, string.Empty );

            var memoryStream = new MemoryStream( );

            var serializer = new XmlSerializer( this.GetType( ) );
            serializer.Serialize( memoryStream, this, xmlnsEmpty );

            memoryStream.Position = 0;

            var xmlDocument = new XmlDocument( );
            xmlDocument.Load( memoryStream );

            return xmlDocument;
        }
    }
}
