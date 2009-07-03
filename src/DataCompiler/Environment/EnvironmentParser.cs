using System.Xml;

namespace DataCompiler.Environment
{
    public interface IEnvironmentParser
    {
        XmlNode Serialize( XmlNode xmlRoot );
    }

    public class EnvironmentParser : IEnvironmentParser
    {
        public XmlNode Serialize( XmlNode xmlDocument )
        {
            var environment = new Environment( );

            var colourAmbient = xmlDocument.SelectSingleNode( "//colourAmbient" );

            if ( colourAmbient != null )
            {
                environment.Colors.Add( EnvironmentColor.FromXML( colourAmbient ) );
            }

            var colourBackground = xmlDocument.SelectSingleNode( "//colourBackground" );

            if ( colourBackground != null )
            {
                environment.Colors.Add( EnvironmentColor.FromXML( colourBackground ) );
            }

            var outputXml = new XmlDocument( );
            XmlNode environmentNode = outputXml.ImportNode( environment.Serialize( ).LastChild, true );
            outputXml.AppendChild( environmentNode );
            return outputXml;
        }
    }
}