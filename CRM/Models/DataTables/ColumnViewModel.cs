using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OgsysCRM.Models.DataTables
{
    public class ColumnViewModel
    {
        /// <summary>
        /// Gets the data component (bind property name).
        /// </summary>
        public string Data { get; private set; }
        /// <summary>
        /// Get's the name component (if any provided on client-side script).
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Indicates if the column is searchable or not.
        /// </summary>
        public bool Searchable { get; private set; }
        /// <summary>
        /// Indicates if the column is orderable or not.
        /// </summary>
        public bool Orderable { get; private set; }
        /// <summary>
        /// Gets the search component for the column.
        /// </summary>
        public SearchViewModel Search { get; private set; }
        /// <summary>
        /// Indicates if the current column should be ordered on server-side or not.
        /// </summary>
        public bool IsOrdered { get { return OrderNumber != -1; } }
        /// <summary>
        /// Indicates the column' position on the ordering (multi-column ordering).
        /// </summary>
        public int OrderNumber { get; private set; }
        /// <summary>
        /// Indicates the column's sort direction.
        /// </summary>
        public OrderDirection SortDirection { get; private set; }
        /// <summary>
        /// Sets the columns ordering.
        /// </summary>
        /// <param name="orderNumber">The column's position on the ordering (multi-column ordering).</param>
        /// <param name="orderDirection">The column's sort direction.</param>
        /// <exception cref="System.ArgumentException">Thrown if the provided orderDirection is not valid.</exception>
        public void SetColumnOrdering(int orderNumber, string orderDirection)
        {
            this.OrderNumber = orderNumber;

            if (orderDirection.ToLower().Equals("asc")) this.SortDirection = ColumnViewModel.OrderDirection.Ascendant;
            else if (orderDirection.ToLower().Equals("desc")) this.SortDirection = ColumnViewModel.OrderDirection.Descendant;
            else throw new ArgumentException("The provided ordering direction was not valid. Valid values must be 'asc' or 'desc' only.");
        }
        /// <summary>
        /// Creates a new DataTables column.
        /// </summary>
        /// <param name="data">The data component (bind property name).</param>
        /// <param name="name">The name of the column (if provided).</param>
        /// <param name="searchable">True if the column allows searching, false otherwise.</param>
        /// <param name="orderable">True if the column allows ordering, false otherwise.</param>
        /// <param name="searchValue">The searched value for the column, or an empty string.</param>
        /// <param name="isRegexValue">True if the search value is a regex value, false otherwise.</param>
        public ColumnViewModel(string data, string name, bool searchable, bool orderable, string searchValue, bool isRegexValue)
        {
            this.Data = data;
            this.Name = name;
            this.Searchable = searchable;
            this.Orderable = orderable;
            this.Search = new SearchViewModel(searchValue, isRegexValue);

            // Default - indicates that the column should not be ordered on server-side.
            this.OrderNumber = -1;
        }
        /// <summary>
        /// Defines order directions for proper use.
        /// </summary>
        public enum OrderDirection
        {
            /// <summary>
            /// Represents an ascendant (A-Z) ordering.
            /// </summary>
            Ascendant = 0,
            /// <summary>
            /// Represents a descendant (Z-A) ordering.
            /// </summary>
            Descendant = 1
        }
    }

    /// <summary>
    /// Represents a read-only DataTables column collection.
    /// </summary>
    public class ColumnCollection : IEnumerable<ColumnViewModel>
    {
        /// <summary>
        /// For internal use only.
        /// Stores data.
        /// </summary>
        private IReadOnlyList<ColumnViewModel> Data;
        /// <summary>
        /// Created a new ReadOnlyColumnCollection with predefined data.
        /// </summary>
        /// <param name="columns">The column collection from DataTables.</param>
        public ColumnCollection(IEnumerable<ColumnViewModel> columns)
        {
            if (columns == null) throw new ArgumentNullException("The provided column collection cannot be null", "columns");
            Data = columns.ToList().AsReadOnly();
        }
        /// <summary>
        /// Get sorted columns on client-side already on the same order as the client requested.
        /// The method checks if the column is bound and if it's ordered on client-side.
        /// </summary>
        /// <returns>The ordered enumeration of sorted columns.</returns>
        public IOrderedEnumerable<ColumnViewModel> GetSortedColumns()
        {
            return Data
                .Where(_column => !String.IsNullOrWhiteSpace(_column.Data) && _column.IsOrdered)
                .OrderBy(_c => _c.OrderNumber);
        }
        /// <summary>
        /// Get filtered columns on client-side.
        /// The method checks if the column is bound and if the search has a value.
        /// </summary>
        /// <returns>The enumeration of filtered columns.</returns>
        public IEnumerable<ColumnViewModel> GetFilteredColumns()
        {
            return Data
                .Where(_column => !String.IsNullOrWhiteSpace(_column.Data) && _column.Searchable && !String.IsNullOrWhiteSpace(_column.Search.Value));
        }
        /// <summary>
        /// Returns the enumerable element as defined on IEnumerable.
        /// </summary>
        /// <returns>The enumerable elemento to iterate through data.</returns>
        public IEnumerator<ColumnViewModel> GetEnumerator()
        {
            return Data.GetEnumerator();
        }
        /// <summary>
        /// Returns the enumerable element as defined on IEnumerable.
        /// </summary>
        /// <returns>The enumerable element to iterate through data.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)Data).GetEnumerator();
        }
    }
}