using System;
using System.Xml;
using BlackBox.Testing;
using DataCompiler.Entities;
using NUnit.Framework;

namespace specs_for_EntityParser
{
    public abstract class base_context : Specification<EntityParser>
    {
        protected XmlNode _levelXml;
        protected XmlDocument _inputXml;

        protected override void EstablishContext( )
        {
            base.EstablishContext( );

            _levelXml = new XmlDocument( );
            _inputXml = new XmlDocument( );
            _inputXml.LoadXml( "<scene formatVersion='1.0' upAxis='y' unitsPerMeter='1' minOgreVersion='1.4' author='OgreMax Scene Exporter by Derek Nedelman (www.ogremax.com)'><environment><colourAmbient r='1' g='1' b='1' /><colourBackground r='0' g='0' b='0' /></environment><nodes>" +
                "<node name='test'><userData>" +
                "<components><attachAI>true</attachAI><aifilePath></aifilePath><aiType>waypoint</aiType><attachGraphics>true</attachGraphics><overrideGraphicsModel></overrideGraphicsModel><graphicsType>default</graphicsType><attachInput>false</attachInput><attachPhysics>true</attachPhysics><physicsType>default</physicsType><attachScript>false</attachScript><scriptName></scriptName><attachSound>false</attachSound></components>" +
                "</userData></node>" +
                "</nodes></scene>" );
        }

        protected override EntityParser CreateSubject( )
        {
            return new EntityParser( );
        }

        [TestFixture]
        public class when_given_xml : base_context
        {
            protected override void When( )
            {
                _levelXml = this.Subject.Serialize( _inputXml );
            }

            [Test]
            public void if_the_xml_contains_entity_data_then_parse_it_and_output_level_xml( )
            {

                Assert.That( _levelXml.SelectSingleNode( "/entities/entity" ), Is.Not.Null );
                Assert.That( _levelXml.HasChildNodes, Is.True );

                XmlWriter outputWriter = new XmlTextWriter( Console.Out );
                _levelXml.WriteTo( outputWriter );
            }
        }
    }
}