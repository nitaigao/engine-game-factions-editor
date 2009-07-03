using System.Collections.Generic;
using System.Xml;

namespace DataCompiler
{
    public class ModelCompiler
    {
        public IList<XmlNode> CompileModelsFromScene( XmlNode xmlNode )
        {
            IList< XmlNode > models = new List<XmlNode>( );

            var entitiesNode = xmlNode.SelectSingleNode( "/scene/nodes" );

            if ( entitiesNode != null )
            {
                foreach ( XmlNode entity in entitiesNode.ChildNodes )
                {
                    var userDataNode = xmlNode.SelectSingleNode( ".//userData" );

                    if( userDataNode != null )
                    {
                        userDataNode.ParentNode.RemoveChild( userDataNode );
                    }

                    var modelFile = new XmlDocument( );
                    var modelNode = modelFile.ImportNode( entity, true );
                    modelFile.AppendChild( modelNode );

                    models.Add( modelFile );
                }
            }

            return models;
        }
    }
}