using System;
using System.Xml;
using BlackBox.Testing;
using DataCompiler.Environment;
using NUnit.Framework;

namespace spect_for_EnvironmentParser
{
    public abstract class base_context : Specification<EnvironmentParser>
    {
        protected XmlDocument _inputXml;
        protected XmlNode _outputXml;

        protected override void EstablishContext( )
        {
            base.EstablishContext( );

            _inputXml = new XmlDocument( );
            _inputXml.LoadXml( "<scene formatVersion='1.0' upAxis='y' unitsPerMeter='1' minOgreVersion='1.4' author='OgreMax Scene Exporter by Derek Nedelman (www.ogremax.com)'><environment><colourAmbient r='1' g='1' b='1' /><colourBackground r='0' g='0' b='0' /></environment></scene>" );
        }

        protected override EnvironmentParser CreateSubject( )
        {
            return new EnvironmentParser( );
        }

        [TestFixture]
        public class when_given_some_xml : base_context
        {
            protected override void When( )
            {
                _outputXml = this.Subject.Serialize( _inputXml );
            }

            [Test]
            public void the_parser_should_output_xml( )
            {
                Assert.That( _outputXml.HasChildNodes );

                XmlWriter outputWriter = new XmlTextWriter( Console.Out );
                _outputXml.WriteTo( outputWriter );
            }
        }
    }
}