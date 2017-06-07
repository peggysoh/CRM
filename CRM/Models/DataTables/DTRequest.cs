namespace OgsysCRM.Models.DataTables
{
    public class DTRequest : IDataTableRequest
    {
        /// <summary>
        /// Gets/Sets the draw counter from DataTables.
        /// </summary>
        public virtual int Draw { get; set; }
        /// <summary>
        /// Gets/Sets the start record number (jump) for paging.
        /// </summary>
        public virtual int Start { get; set; }
        /// <summary>
        /// Gets/Sets the length of the page (paging).
        /// </summary>
        public virtual int Length { get; set; }
        /// <summary>
        /// Gets/Sets the global search term.
        /// </summary>
        public virtual SearchViewModel Search { get; set; }
        /// <summary>
        /// Gets/Sets the column collection.
        /// </summary>
        public virtual ColumnCollection Columns { get; set; }

    }
}