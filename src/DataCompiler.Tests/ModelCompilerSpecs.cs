using System.Collections.Generic;
using System.Xml;
using BlackBox.Testing;
using DataCompiler;
using NUnit.Framework;

namespace specs_for_ModelCompiler
{
    public abstract class base_context : Specification< ModelCompiler >
    {
        protected XmlDocument _inputXml;

        protected override void EstablishContext( )
        {
            _inputXml = new XmlDocument( );
            _inputXml.LoadXml( "<scene><nodes><node name='test' /></nodes></scene>" );
            
            base.EstablishContext( );
        }

        protected override ModelCompiler CreateSubject( )
        {
            return new ModelCompiler( );
        }
    }

    [TestFixture]
    public class when_a_scene_file_is_passed_to_the_compiler : base_context
    {
        private IList< XmlNode > _models;

        protected override void EstablishContext( )
        {
            base.EstablishContext( );
        }

        protected override void When( )
        {
            _models = this.Subject.CompileModelsFromScene( _inputXml );
        }

        [Test]
        public void it_should_return_a_list_of_xml_models_ready_for_writing_to_disk( )
        {
            Assert.That( _models.Count, Is.GreaterThan( 0 ) );
        }
    }
}
