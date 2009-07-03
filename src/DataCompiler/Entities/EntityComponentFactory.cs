using System;
using System.IO;
using System.Xml;
using DataCompiler.System;

namespace DataCompiler.Entities
{
    public class EntityComponentFactory
    {
        public static EntityComponent CreateComponent( string systemType, XmlNode xmlNode )
        {
            switch( systemType )
            {
                case SystemTypes.Graphics:
                    return EntityComponentFactory.CreateGraphicsComponent( xmlNode );

                case SystemTypes.Physics:
                    return EntityComponentFactory.CreatePhysicsComponent( xmlNode );

                case SystemTypes.Animation:
                    return EntityComponentFactory.CreateAnimationComponent( xmlNode );

                case SystemTypes.Input:
                    return EntityComponentFactory.CreateInputComponent( xmlNode );

                case SystemTypes.Sound:
                    return EntityComponentFactory.CreateSoundComponent( xmlNode );

                case SystemTypes.Script:
                    return EntityComponentFactory.CreateScriptComponent( xmlNode );

                case SystemTypes.AI:
                    return EntityComponentFactory.CreateAIComponent( xmlNode );

                case SystemTypes.Geometry:
                    return EntityComponentFactory.CreateGeometryComponent( xmlNode );

                    default:
                    throw new Exception( string.Format( "unable to create component of type {0}", systemType ) );
            }
        }

        private static EntityComponent CreateGeometryComponent( XmlNode xmlNode )
        {
            var component = new EntityComponent( SystemTypes.Geometry );

            component.Attributes.Add( new EntityAttribute( "scale" )
                                          {
                                              V1 = xmlNode.SelectSingleNode( "./scale" ).Attributes[ "x" ].Value,
                                              V2 = xmlNode.SelectSingleNode( "./scale" ).Attributes[ "y" ].Value,
                                              V3 = xmlNode.SelectSingleNode( "./scale" ).Attributes[ "z" ].Value
                                          }
                );

            component.Attributes.Add( new EntityAttribute( "position" )
                                          {
                                              V1 = xmlNode.SelectSingleNode( "./position" ).Attributes[ "x" ].Value,
                                              V2 = xmlNode.SelectSingleNode( "./position" ).Attributes[ "y" ].Value,
                                              V3 = xmlNode.SelectSingleNode( "./position" ).Attributes[ "z" ].Value
                                          }
                );

            component.Attributes.Add( new EntityAttribute( "orientation" )
                                          {
                                              V1 = xmlNode.SelectSingleNode( "./rotation" ).Attributes[ "qx" ].Value,
                                              V2 = xmlNode.SelectSingleNode( "./rotation" ).Attributes[ "qy" ].Value,
                                              V3 = xmlNode.SelectSingleNode( "./rotation" ).Attributes[ "qz" ].Value,
                                              V4 = xmlNode.SelectSingleNode( "./rotation" ).Attributes[ "qw" ].Value
                                          }
                );

            return component;
        }

        private static EntityComponent CreateAIComponent( XmlNode xmlNode )
        {
            var component = new EntityComponent( SystemTypes.AI );

            XmlNode aiTypeNode = xmlNode.SelectSingleNode( ".//aiType" );
            component.Type = aiTypeNode.InnerText;

            XmlNode aiFilePathNode = xmlNode.SelectSingleNode( ".//aifilePath" );

            if ( aiFilePathNode != null )
            {
                if ( aiFilePathNode.InnerText.Length > 0 )
                {
                    component.Attributes.Add( new EntityAttribute( "scriptPath" ) { V1 = string.Format( "{0}/{1}", System.Paths.BaseGamePath, aiFilePathNode.InnerText ) } );
                }
            }

            XmlNode ainavMeshNode = xmlNode.SelectSingleNode( ".//ainavMesh" );

            if ( ainavMeshNode != null )
            {
                if ( ainavMeshNode.InnerText.Length > 0 )
                {
                    component.Attributes.Add( new EntityAttribute( "navMesh" ) { V1 = string.Format( "{0}/{1}", System.Paths.BaseGamePath, ainavMeshNode.InnerText ) } );
                }
            }

            return component;
        }

        private static EntityComponent CreateScriptComponent( XmlNode xmlNode )
        {
            var component = new EntityComponent( SystemTypes.Script );

            XmlNode scriptNameNode = xmlNode.SelectSingleNode( ".//scriptName" );

            if ( scriptNameNode != null )
            {
                component.Attributes.Add( new EntityAttribute( "scriptPath" ) { V1 = string.Format( "{0}/{1}", System.Paths.ScriptsGamePath, scriptNameNode.InnerText ) } );
            }

            return component;
        }

        private static EntityComponent CreateSoundComponent( XmlNode xmlNode )
        {
            return new EntityComponent( SystemTypes.Sound );
        }

        private static EntityComponent CreateInputComponent( XmlNode xmlNode )
        {
            return new EntityComponent( SystemTypes.Input );
        }

        private static EntityComponent CreateAnimationComponent( XmlNode xmlNode )
        {
            var component = new EntityComponent( SystemTypes.Animation );

            XmlNode animationsPathNode = xmlNode.SelectSingleNode( ".//animationPath" );

            if ( animationsPathNode != null )
            {
                string fullPath = string.Format( "{0}/{1}", "../../shared/entities", animationsPathNode.InnerText );

                var searchPathInfo = new DirectoryInfo( string.Format( "{0}/{1}", System.Paths.WorkingDirectory, fullPath ) );

                foreach ( var file in searchPathInfo.GetFiles( "*.hkx" ) )
                {
                    int seperatorIndex = file.Name.IndexOf( "_" ) + 1;
                    int extensionIndex = file.Name.IndexOf( ".hkx" );

                    string animationName = file.Name.Substring( seperatorIndex, extensionIndex - seperatorIndex );
                    string animationPath = string.Format( "{0}/{1}/{2}", System.Paths.AnimationsGamePath, animationsPathNode.InnerText.Substring( animationsPathNode.InnerText.IndexOf( "/" ) + 1 ), file.Name );

                    if ( animationName != "bindPose" )
                    {
                        component.Attributes.Add( new EntityAttribute( SystemTypes.Animation ) { V1 = animationName, V2 = animationPath } );
                    }
                }

                XmlNode bindPosePathNode = xmlNode.SelectSingleNode( ".//bindPose" );

                if ( bindPosePathNode != null )
                {
                    string bindPosePath = string.Format( "{0}/{1}/{2}", System.Paths.AnimationsGamePath, animationsPathNode.InnerText.Substring( animationsPathNode.InnerText.IndexOf( "/" ) + 1 ), bindPosePathNode.InnerText );

                    component.Attributes.Add( new EntityAttribute( "bindPose" ) { V1 = bindPosePath } );
                }

                XmlNode defaultAnimatioNode = xmlNode.SelectSingleNode( ".//defaultAnimation" );

                if ( defaultAnimatioNode != null )
                {
                    string defaultAnimationPath = string.Format( "{0}/{1}/{2}", System.Paths.AnimationsGamePath, animationsPathNode.InnerText.Substring( animationsPathNode.InnerText.IndexOf( "/" ) + 1 ), defaultAnimatioNode.InnerText );

                    component.Attributes.Add( new EntityAttribute( "defaultAnimation" ) { V1 = defaultAnimationPath } );
                }
            }

            return component;
        }

        private static EntityComponent CreatePhysicsComponent( XmlNode xmlNode )
        {
            var component = new EntityComponent( SystemTypes.Physics );

            component.Attributes.Add( new EntityAttribute( "body" ) { V1 = string.Format( "/data/entities/bodies/{0}.hkx", xmlNode.Attributes[ "name" ].Value ) } );

            XmlNode physicsTypeNode = xmlNode.SelectSingleNode( ".//physicsType" );
            component.Type = physicsTypeNode.InnerText;

            return component;
        }

        private static EntityComponent CreateGraphicsComponent( XmlNode xmlNode )
        {
            var component = new EntityComponent( SystemTypes.Graphics );

            XmlNode overrideModelNode = xmlNode.SelectSingleNode( ".//overrideGraphicsModel" );

            if ( overrideModelNode != null )
            {
                if ( overrideModelNode.InnerText.Length > 0 )
                {
                    component.Attributes.Add( new EntityAttribute( "model" ) { V1 = string.Format( "{0}/{1}", System.Paths.ModelsGamePath, overrideModelNode.InnerText ) } );
                }
                else
                {
                    component.Attributes.Add( new EntityAttribute( "model" ) { V1 = string.Format( "{0}/{1}.model", System.Paths.ModelsGamePath, xmlNode.Attributes[ "name" ].Value ) } );
                }
            }

            XmlNode graphicsTypeNode = xmlNode.SelectSingleNode( ".//graphicsType" );
            component.Type = graphicsTypeNode.InnerText;

            return component;
        }
    }
}
