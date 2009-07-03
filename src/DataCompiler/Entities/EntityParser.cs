using System.Xml;
using DataCompiler.System;

namespace DataCompiler.Entities
{
    public interface IEntityParser : IParser
    {

    }

    public interface IParser
    {
        XmlNode Serialize( XmlNode xmlRoot );
    }

    public class EntityParser : IEntityParser
    {
        public XmlNode Serialize( XmlNode xmlRoot )
        {
            var outputXml = new XmlDocument( );
            var entitiesNode = outputXml.CreateNode( XmlNodeType.Element, "entities", string.Empty );
            outputXml.AppendChild( entitiesNode );

            XmlNode nodesNode = xmlRoot.SelectSingleNode( "/scene/nodes" );

            if ( nodesNode != null )
            {
                foreach ( XmlNode childNode in nodesNode.ChildNodes )
                {
                    var entity = new Entity( childNode.Attributes[ "name" ].Value );

                    XmlNode externalNode = childNode.SelectSingleNode( ".//userData/external/externalSrc" );

                    if ( externalNode != null )
                    {
                        entity.Source = externalNode.InnerText;
                    }

                    XmlNode graphicsNode = childNode.SelectSingleNode( ".//userData/components/attachGraphics" );

                    if ( graphicsNode != null )
                    {
                        if ( bool.Parse( graphicsNode.InnerText ) )
                        {
                            entity.components.Add( EntityComponentFactory.CreateComponent( SystemTypes.Graphics, childNode ) );
                        }
                    }

                    XmlNode physicsNode = childNode.SelectSingleNode( ".//userData/components/attachPhysics" );

                    if ( physicsNode != null )
                    {
                        if ( bool.Parse( physicsNode.InnerText ) )
                        {
                            entity.components.Add( EntityComponentFactory.CreateComponent( SystemTypes.Physics, childNode ) );
                        }
                    }

                    XmlNode animationNode = childNode.SelectSingleNode( ".//userData/components/attachAnimation" );

                    if ( animationNode != null )
                    {
                        if ( bool.Parse( animationNode.InnerText ) )
                        {
                            entity.components.Add( EntityComponentFactory.CreateComponent( SystemTypes.Animation, childNode ) );
                        }
                    }

                    XmlNode inputNode = childNode.SelectSingleNode( ".//userData/components/attachInput" );

                    if ( inputNode != null )
                    {
                        if ( bool.Parse( inputNode.InnerText ) )
                        {
                            entity.components.Add( EntityComponentFactory.CreateComponent( SystemTypes.Input, childNode ) );
                        }
                    }

                    XmlNode soundNode = childNode.SelectSingleNode( ".//userData/components/attachSound" );

                    if ( soundNode != null )
                    {
                        if ( bool.Parse( soundNode.InnerText ) )
                        {
                            entity.components.Add( EntityComponentFactory.CreateComponent( SystemTypes.Sound, childNode ) );
                        }
                    }

                    XmlNode scriptNode = childNode.SelectSingleNode( ".//userData/components/attachScript" );

                    if ( scriptNode != null )
                    {
                        if ( bool.Parse( scriptNode.InnerText ) )
                        {
                            entity.components.Add( EntityComponentFactory.CreateComponent( SystemTypes.Script, childNode ) );
                        }
                    }

                    XmlNode aiNode = childNode.SelectSingleNode( ".//userData/components/attachAI" );

                    if ( aiNode != null )
                    {
                        if ( bool.Parse( aiNode.InnerText ) )
                        {
                            entity.components.Add( EntityComponentFactory.CreateComponent( SystemTypes.AI, childNode ) );
                        }
                    }

                    if ( childNode.SelectSingleNode( "./scale" ) != null && childNode.SelectSingleNode( "./position" ) != null && childNode.SelectSingleNode( "./rotation" ) != null )
                    {
                        entity.components.Add( EntityComponentFactory.CreateComponent( SystemTypes.Geometry, childNode ) );
                    }

                    XmlNode entityNode = outputXml.ImportNode( entity.Serialize( ).LastChild, true );
                    entitiesNode.AppendChild( entityNode );
                }
            }

            return outputXml;
        }
    }
}
