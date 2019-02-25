using System;

/********************************************************************/
/*  Reference: https://github.com/Hoangpnse62077/CasptoneProject    */
/*  Gmail: hoangpnse62077@fpt.edu.vn                                */
/********************************************************************/

namespace eLTMS.DataAccess.Infrastructure
{
    public class Disposable : IDisposable
    {
        private bool isDisposed;

        Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                DisposeCore();
            }

            isDisposed = true;
        }

        // Ovveride this to dispose custom objects
        protected virtual void DisposeCore()
        {
        }
    }
}