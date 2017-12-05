using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace UniversalAnalyticsHttpWrapper.Objects
{
    /// <summary>
    /// Data object holding a GUID representing a client id (or cid).
    /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid
    /// </summary>
    public class ClientId
    {
        /// <summary>
        /// Creates a new client id with a random Guid as the id.
        /// </summary>
        public ClientId()
        {
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new client id with a guid generated from a seed string.
        /// </summary>
        public ClientId(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var md5Hasher = MD5.Create())
            {
                this.Id = new Guid(md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(value)));
            }
        }

        /// <summary>
        /// Creates a new client id with the supplied Guid 'id' as the client id.
        /// </summary>
        /// <param name="id">The Guid representing the client id.</param>
        /// <exception cref="ArgumentNullException">Thrown if 'id' is null.</exception>
        public ClientId(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
        }

        /// <summary>
        /// Value holder for the client id.
        /// </summary>
        public Guid Id { get; }
    }
}