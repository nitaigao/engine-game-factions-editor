using System.IO;
using System.Xml;
using BlackBox.Testing;
using DataCompiler.Environment;
using NUnit.Framework;

namespace specs_for_EnvironmentColor
{
    public abstract class base_context : Specification<EnvironmentColor>
    {
        protected EnvironmentColor _outputColor;
        protected XmlNode _inputXml;

        protected override void EstablishContext( )
        {
            base.EstablishContext( );

            var elementXml = new XmlDocument( );
            var stringReader = new StringReader( "<colourAmbient r='1' g='1' b='1' />" );
            var xmlReader = new XmlTextReader( stringReader );
            _inputXml = elementXml.ReadNode( xmlReader );
        }

        protected override EnvironmentColor CreateSubject( )
        {
            return new EnvironmentColor( );
        }

        [TestFixture]
        public class when_given_xml : base_context
        {
            protected override void When( )
            {
                _outputColor = EnvironmentColor.FromXML( _inputXml );
            }

            [Test]
            public void should_deserialize_into_a_color( )
            {
                Assert.That( _outputColor.R != 0 );
                Assert.That( _outputColor.G != 0 );
                Assert.That( _outputColor.B != 0 );
            }
        }

        [TestFixture]
        public class when_asked_to_serialize : base_context
        {
            private XmlNode _outputXml;

            protected override void When( )
            {
                _outputXml = this.Subject.Serialize( );
            }

            [Test]
            public void should_serialize_object_to_xml( )
            {
                Assert.That( _outputXml.HasChildNodes );
            }
        }

    }
}