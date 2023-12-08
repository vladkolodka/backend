using Menchul.Core.ResourceProcessing.Entities;
using Menchul.Mongo.Resources;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.Mongo.Common
{
    /// <summary>
    /// Reference to other Mongo document
    /// </summary>
    public interface IDocumentRef : IDocumentResourceContainer
    {
    }

    /// <inheritdoc />
    public class DocumentRef<TDocument, TId> : IDocumentRef where TDocument : IDocumentRoot<TId>
    {
        private TDocument _document;

        /// <summary>
        /// The property name should be equal to <see cref="Constants.Properties.RefId"/>
        /// </summary>
        public TId RefId { get; set; }

        [BsonIgnore]
        public TDocument Document
        {
            get => _document;
            set
            {
                _document = value;
                RefId = value.Id;
            }
        }

        public bool ProcessValue => true;

        public IEnumerable<IResource> GetResources()
        {
            yield return Document;
        }

        private bool Equals(DocumentRef<TDocument, TId> other)
        {
            return EqualityComparer<TId>.Default.Equals(RefId, other.RefId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((DocumentRef<TDocument, TId>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TId>.Default.GetHashCode(RefId);
        }

        /// <summary>
        /// Cast DocumentReference &lt;TDocument, TId&gt; to TId
        /// </summary>
        /// <param name="ref"></param>
        /// <returns>TId</returns>
        public static implicit operator TId(DocumentRef<TDocument, TId> @ref) => @ref.RefId;

        /// <summary>
        /// Cast TId to DocumentReference&lt;TDocument, TId&gt;
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DocumentReference&lt;TDocument, TId&gt;</returns>
        public static implicit operator DocumentRef<TDocument, TId>(TId id) => new() {RefId = id};

        /// <summary>
        /// Cast DocumentReference &lt;TDocument, TId&gt; to TDocument
        /// </summary>
        /// <param name="ref"></param>
        /// <returns>TDocument</returns>
        public static implicit operator TDocument(DocumentRef<TDocument, TId> @ref) => @ref.Document;

        /// <summary>
        /// Cast TDocument to DocumentReference&lt;TDocument, TId&gt;
        /// </summary>
        /// <param name="document"></param>
        /// <returns>DocumentReference&lt;TDocument, TId&gt;</returns>
        public static implicit operator DocumentRef<TDocument, TId>(TDocument document) => new()
        {
            RefId = document.Id, Document = document
        };
    }

    public class DocumentReferenceList<TDocument, TId> : List<DocumentRef<TDocument, TId>>,
        IDocumentResourceContainer
        where TDocument : IDocumentRoot<TId>
    {
        public IEnumerable<IResource> GetResources() => this.ToList();

        public DocumentReferenceList(IEnumerable<DocumentRef<TDocument, TId>> collection) : base(collection)
        {
        }

        public DocumentReferenceList()
        {
        }

        /// <summary>
        /// Cast DocumentReferenceList&lt;TDocument, TId&gt; to List&lt;TId&gt;
        /// </summary>
        /// <param name="list"></param>
        /// <returns>List&lt;TId&gt;</returns>
        public static implicit operator List<TId>(DocumentReferenceList<TDocument, TId> list) =>
            list.Select(r => (TId)r).ToList();

        /// <summary>
        /// Cast List&lt;TId&gt; to DocumentReferenceList&lt;TDocument, TId&gt;
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>DocumentReferenceList&lt;TDocument, TId&gt;</returns>
        public static implicit operator DocumentReferenceList<TDocument, TId>(List<TId> ids) =>
            new(ids.Select(id => (DocumentRef<TDocument, TId>)id));

        /// <summary>
        /// Cast DocumentReferenceList&lt;TDocument, TId&gt; to List&lt;TDocument&gt;
        /// </summary>
        /// <param name="list"></param>
        /// <returns>List&lt;TDocument&gt;</returns>
        public static implicit operator List<TDocument>(DocumentReferenceList<TDocument, TId> list) =>
            list.Select(r => (TDocument)r).ToList();

        /// <summary>
        /// Cast List&lt;TDocument&gt; to DocumentReferenceList&lt;TDocument, TId&gt;
        /// </summary>
        /// <param name="documents"></param>
        /// <returns>DocumentReferenceList&lt;TDocument, TId&gt;</returns>
        public static implicit operator DocumentReferenceList<TDocument, TId>(List<TDocument> documents) =>
            new(documents.Select(doc => (DocumentRef<TDocument, TId>)doc));
    }
}