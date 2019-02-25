using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/********************************************************************/
/*  Reference: https://github.com/Hoangpnse62077/CasptoneProject    */
/*  Gmail: hoangpnse62077@fpt.edu.vn                                */
/********************************************************************/

namespace eLTMS.DataAccess.Infrastructure
{
    [System.Serializable]
    public class ConcurrencyException : System.Exception
    {
        /// <summary>
        /// The current entity data
        /// </summary>
        public object DatabaseData { get; set; }

        public ConcurrencyException() { }
        public ConcurrencyException(string message) : base(message) { }
        public ConcurrencyException(string message, System.Exception inner) : base(message, inner) { }
        public ConcurrencyException(string message, object databaseData) : base(message)
        {
            DatabaseData = databaseData ?? throw new ArgumentNullException("databaseData");
        }
        public ConcurrencyException(string message, object databaseData, System.Exception inner) : base(message, inner)
        {
            DatabaseData = databaseData ?? throw new ArgumentNullException("databaseData");
        }
        protected ConcurrencyException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class DataException : System.Exception
    {
        public DataException() { }
        public DataException(string message) : base(message) { }
        public DataException(string message, System.Exception inner) : base(message, inner) { }
        protected DataException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
