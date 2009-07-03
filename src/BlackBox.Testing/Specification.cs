
using System;
using NUnit.Framework;

namespace BlackBox.Testing
{
    public abstract class Specification
    {
        protected virtual bool RethrowExceptionsThrownDuringWhen
        {
            get { return true; }
        }

        protected Exception ExceptionThrownDuringWhen
        {
            get;
            set;
        }

        [SetUp]
        public virtual void SetUp( )
        {
            EstablishContext( );

            try
            {
                When( );
            }
            catch ( Exception ex )
            {
                ExceptionThrownDuringWhen = ex;

                if ( RethrowExceptionsThrownDuringWhen )
                {
                    throw;
                }
            }
        }

        protected virtual void EstablishContext( )
        {
        }

        protected abstract void When( );

        [TearDown]
        public virtual void AfterEachSpec( )
        {
        }
    }

    public abstract class Specification<TSubject> : Specification
    {
        [SetUp]
        public override void SetUp( )
        {
            EstablishContext( );
            Subject = CreateSubject( );

            try
            {
                When( );
            }
            catch ( Exception ex )
            {
                ExceptionThrownDuringWhen = ex;

                if ( RethrowExceptionsThrownDuringWhen )
                {
                    throw;
                }
            }
        }

        protected TSubject Subject { get; private set; }

        protected abstract TSubject CreateSubject( );
    }
}