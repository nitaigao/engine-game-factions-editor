using System.Xml;
using BlackBox.Testing;
using DataCompiler;
using DataCompiler.Entities;
using DataCompiler.Environment;
using NUnit.Framework;
using Rhino.Mocks;

namespace specs_for_LevelCompiler
{
    public abstract class base_context : Specification<LevelCompiler>
    {
        protected XmlDocument _resultXml;
        protected XmlDocument _environmentOutputXml;
        protected XmlDocument _entitiesOutputXml;

        protected XmlDocument _inputXml;
        protected IEnvironmentParser _environmentParser;
        protected IEntityParser _entityParser;

        protected override void EstablishContext( )
        {
            base.EstablishContext( );

            _inputXml = new XmlDocument( );
            _inputXml.LoadXml( "<scene formatVersion='1.0' upAxis='y' unitsPerMeter='1' minOgreVersion='1.4' author='OgreMax Scene Exporter by Derek Nedelman (www.ogremax.com)'><environment><colourAmbient r='1' g='1' b='1' /><colourBackground r='0' g='0' b='0' /></environment><nodes><node Name='test' /></nodes></scene>" );

            _environmentOutputXml = new XmlDocument( );
            _environmentOutputXml.LoadXml( "<environment><color Type='colourAmbient' r='1' g='1' b='1' /></environment>" );

            _entitiesOutputXml = new XmlDocument( );
            _entitiesOutputXml.LoadXml( "<entities><entity Name='testentity' /></entities>" );

            _environmentParser = MockRepository.GenerateMock<IEnvironmentParser>( );
            _entityParser = MockRepository.GenerateMock<IEntityParser>( );
        }

        protected override LevelCompiler CreateSubject( )
        {
            return new LevelCompiler( _environmentParser, _entityParser );
        }

        [TestFixture]
        public class when_the_compiler_is_supplied_xml : base_context
        {
            protected override void EstablishContext( )
            {
                base.EstablishContext( );

                _environmentParser.Expect( o => o.Serialize( _inputXml ) ).Return( _environmentOutputXml );
                _entityParser.Expect( o => o.Serialize( _inputXml ) ).Return( _entitiesOutputXml );

            }

            protected override void When( )
            {
                _resultXml = this.Subject.CompileLevel( _inputXml );
            }

            [Test]
            public void it_should_extract_the_node_data_and_compile_it_into_entities( )
            {
                var entitiesNode = _resultXml.SelectSingleNode( "/level/entities" );
                Assert.That( entitiesNode, Is.Not.Null );
                Assert.That( entitiesNode.HasChildNodes, Is.True );
            }

            [Test]
            public void it_should_extract_the_environment_data_and_compile_it_into_the_level_xml( )
            {
                var environmentNode = _resultXml.SelectSingleNode( "/level/environment" );
                Assert.That( environmentNode, Is.Not.Null );
                Assert.That( environmentNode.HasChildNodes, Is.True );
            }
        }
    }
}