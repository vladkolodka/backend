using System.Collections.Generic;

namespace Menchul.Mongo.Common
{
    /// <summary>
    /// Hierarchy of nested documents
    /// </summary>
    /// <typeparam name="T">Type of nested documents</typeparam>
    public interface IHierarchy<T>
    {
        public List<T> Hierarchy { get; set; }
    }
}