using System.Xml;
using DataCompiler.Entities;
using DataCompiler.Environment;

namespace DataCompiler
{
    public class LevelCompiler
    {
        private readonly IEnvironmentParser _environmentParser;
        private readonly IEntityParser _entityParser;

        public LevelCompiler( )
            : this( new EnvironmentParser( ), new EntityParser( ) )
        {
            
        }

        public LevelCompiler( IEnvironmentParser environmentParser, IEntityParser entityParser )
        {
            _environmentParser = environmentParser;
            _entityParser = entityParser;
        }

        public XmlDocument CompileLevel( XmlDocument xmlDocument )
        {
            var result = new XmlDocument( );
            var rootElement = result.CreateNode( XmlNodeType.Element, "level", string.Empty );

            var environmentData = xmlDocument.SelectSingleNode( "//environment" );

            if ( environmentData != null )
            {
                XmlNode environmentResult = _environmentParser.Serialize( xmlDocument );
                XmlNode environmentNode = result.ImportNode( environmentResult.LastChild, true );
                rootElement.AppendChild( environmentNode );
            }

            var entityData = xmlDocument.SelectSingleNode( "//nodes" );

            if ( entityData != null )
            {
                XmlNode entitiesResult = _entityParser.Serialize( xmlDocument );
                XmlNode entitiesNode = result.ImportNode( entitiesResult.LastChild, true );
                rootElement.AppendChild( entitiesNode );
            }

            result.AppendChild( rootElement );
            return result;
        }
    }
}
