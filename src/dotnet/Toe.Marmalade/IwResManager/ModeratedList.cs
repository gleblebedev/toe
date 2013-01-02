using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Toe.Utils.Marmalade.IwResManager
{
	//public class ModeratedList<T>: IList<T>, INotifyCollectionChanged,INotifyPropertyChanged, IBindingList
	//{
	//    private static PropertyChangedEventArgs CountChanged = new PropertyChangedEventArgs ( "Count" );
	//    private static PropertyChangedEventArgs IndexerChanged = new PropertyChangedEventArgs ("Item[]" );

	//    readonly List<T> impl = new List<T>();

	//    private bool isReadOnly;

	//    #region Implementation of IEnumerable

	//    /// <summary>
	//    /// Returns an enumerator that iterates through the collection.
	//    /// </summary>
	//    /// <returns>
	//    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
	//    /// </returns>
	//    /// <filterpriority>1</filterpriority>
	//    public IEnumerator<T> GetEnumerator()
	//    {
	//        return impl.GetEnumerator();
	//    }

	//    /// <summary>
	//    /// Returns an enumerator that iterates through a collection.
	//    /// </summary>
	//    /// <returns>
	//    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
	//    /// </returns>
	//    /// <filterpriority>2</filterpriority>
	//    IEnumerator IEnumerable.GetEnumerator()
	//    {
	//        return this.GetEnumerator();
	//    }

	//    #endregion

	//    #region Implementation of ICollection<T>

	//    /// <summary>
	//    /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	//    /// </summary>
	//    /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
	//    public void Add(T item)
	//    {
	//        this.AddItem(item);
	//    }

	//    /// <summary>
	//    /// Adds an item to the <see cref="T:System.Collections.IList"/>.
	//    /// </summary>
	//    /// <returns>
	//    /// The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection,
	//    /// </returns>
	//    /// <param name="value">The object to add to the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception><filterpriority>2</filterpriority>
	//    public int Add(object value)
	//    {
	//        this.AddItem((T)value);
	//        return this.Count - 1;
	//    }

	//    private void AddItem(T value)
	//    {
	//        var index = Count;

	//        this.impl.Add(value);

	//        OnPropertyChanged(CountChanged);
	//        OnPropertyChanged(IndexerChanged);
	//        OnCollectionChanged(NotifyCollectionChangedAction.Add, value, index);
	//        OnBindingCollectionChanged(ListChangedType.ItemAdded, index);
	//    }

	//    /// <summary>
	//    /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
	//    /// </summary>
	//    /// <returns>
	//    /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
	//    /// </returns>
	//    /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param><filterpriority>2</filterpriority>
	//    public bool Contains(object value)
	//    {
	//        return IndexOf(value) >= 0;
	//    }

	

	//    /// <summary>
	//    /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
	//    /// </summary>
	//    /// <returns>
	//    /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
	//    /// </returns>
	//    /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param><filterpriority>2</filterpriority>
	//    public int IndexOf(object value)
	//    {
	//        return ((IList)impl).IndexOf(value);
	//    }

	//    /// <summary>
	//    /// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
	//    /// </summary>
	//    /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted. </param><param name="value">The object to insert into the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception><exception cref="T:System.NullReferenceException"><paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception><filterpriority>2</filterpriority>
	//    public void Insert(int index, object value)
	//    {
	//        ((IList<T>)this).Insert(index,value);
	//    }

	//    /// <summary>
	//    /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
	//    /// </summary>
	//    /// <param name="value">The object to remove from the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception><filterpriority>2</filterpriority>
	//    public void Remove(object value)
	//    {
	//        int index = impl.IndexOf((T)value);
	//        if (index >= 0) RemoveAt(index);
	//    }


	//    /// <summary>
	//    /// Removes the <see cref="T:System.Collections.IList"/> item at the specified index.
	//    /// </summary>
	//    /// <param name="index">The zero-based index of the item to remove. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception><filterpriority>2</filterpriority>
	//    public void RemoveAt(int index)
	//    {
	//        T removedItem = this[index];

	//        this.impl.RemoveAt(index);

	//        OnPropertyChanged(CountChanged);
	//        OnPropertyChanged(IndexerChanged);
	//        OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
	//        OnBindingCollectionChanged(ListChangedType.ItemDeleted, index);
	//    }

	//    private void ReplaceItem(int index, object value)
	//    {
	//        T oldItem = this[index];
	//        var newValue = (T)value;
	//        impl[index] = newValue;
	//        OnPropertyChanged(IndexerChanged);
	//        OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, newValue, index);
	//        OnBindingCollectionChanged(ListChangedType.ItemChanged, index);
	//    }

	//    private void OnBindingCollectionChanged(ListChangedType type, int index)
	//    {
	//        if (ListChanged != null) ListChanged(this, new ListChangedEventArgs(type,index));
	//    }

	//    private void OnBindingCollectionChanged(ListChangedEventArgs listChangedEventArgs)
	//    {
	//        if (ListChanged != null) ListChanged(this, listChangedEventArgs);
	//    }

	//    private void OnCollectionChanged(NotifyCollectionChangedAction remove, T removedItem, int index)
	//    {
	//        if (this.CollectionChanged != null) this.CollectionChanged(this,new NotifyCollectionChangedEventArgs(remove,removedItem,index));
	//    }
	//    private void OnCollectionChanged(NotifyCollectionChangedAction remove, T newItem, T oldItem, int index)
	//    {
	//        if (this.CollectionChanged != null) this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(remove, newItem, oldItem, index));
	//    }

	//    /// <summary>
	//    /// Gets or sets the element at the specified index.
	//    /// </summary>
	//    /// <returns>
	//    /// The element at the specified index.
	//    /// </returns>
	//    /// <param name="index">The zero-based index of the element to get or set. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList"/> is read-only. </exception><filterpriority>2</filterpriority>
	//    object IList.this[int index]
	//    {
	//        get
	//        {
	//            return impl[index];
	//        }
	//        set
	//        {
	//            ReplaceItem(index, value);
	//        }
	//    }

		

	//    /// <summary>
	//    /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> is read-only.
	//    /// </summary>
	//    /// <returns>
	//    /// true if the <see cref="T:System.Collections.IList"/> is read-only; otherwise, false.
	//    /// </returns>
	//    /// <filterpriority>2</filterpriority>
	//    public bool IsReadOnly
	//    {
	//        get
	//        {
	//            return isReadOnly;
	//        }
	//    }

	//    /// <summary>
	//    /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
	//    /// </summary>
	//    /// <returns>
	//    /// true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.
	//    /// </returns>
	//    /// <filterpriority>2</filterpriority>
	//    public bool IsFixedSize
	//    {
	//        get
	//        {
	//            throw new NotImplementedException();
	//        }
	//    }

	//    /// <summary>
	//    /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	//    /// </summary>
	//    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
	//    public void Clear()
	//    {
	//    }

	

	//    /// <summary>
	//    /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
	//    /// </summary>
	//    /// <returns>
	//    /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
	//    /// </returns>
	//    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
	//    public bool Contains(T item)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
	//    /// </summary>
	//    /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
	//    public void CopyTo(T[] array, int arrayIndex)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	//    /// </summary>
	//    /// <returns>
	//    /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
	//    /// </returns>
	//    /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
	//    public bool Remove(T item)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
	//    /// </summary>
	//    /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing. </param><param name="index">The zero-based index in <paramref name="array"/> at which copying begins. </param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. </exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception><exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception><filterpriority>2</filterpriority>
	//    public void CopyTo(Array array, int index)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
	//    /// </summary>
	//    /// <returns>
	//    /// The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
	//    /// </returns>
	//    /// <filterpriority>2</filterpriority>
	//    public int Count
	//    {
	//        get
	//        {
	//            return impl.Count;
	//        }
	//    }

	//    /// <summary>
	//    /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
	//    /// </summary>
	//    /// <returns>
	//    /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
	//    /// </returns>
	//    /// <filterpriority>2</filterpriority>
	//    public object SyncRoot
	//    {
	//        get
	//        {
	//            throw new NotImplementedException();
	//        }
	//    }

	//    /// <summary>
	//    /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
	//    /// </summary>
	//    /// <returns>
	//    /// true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.
	//    /// </returns>
	//    /// <filterpriority>2</filterpriority>
	//    public bool IsSynchronized
	//    {
	//        get
	//        {
	//            throw new NotImplementedException();
	//        }
	//    }

	//    /// <summary>
	//    /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	//    /// </summary>
	//    /// <returns>
	//    /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
	//    /// </returns>
	//    int ICollection<T>.Count
	//    {
	//        get
	//        {
	//            return Count;
	//        }
	//    }

	//    /// <summary>
	//    /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
	//    /// </summary>
	//    /// <returns>
	//    /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
	//    /// </returns>
	//    bool ICollection<T>.IsReadOnly
	//    {
	//        get
	//        {
	//            return IsReadOnly;
	//        }
	//    }

	//    #endregion

	//    #region Implementation of IList<T>

	//    /// <summary>
	//    /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
	//    /// </summary>
	//    /// <returns>
	//    /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
	//    /// </returns>
	//    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
	//    public int IndexOf(T item)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
	//    /// </summary>
	//    /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
	//    public void Insert(int index, T item)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
	//    /// </summary>
	//    /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
	//    void IList<T>.RemoveAt(int index)
	//    {
	//        RemoveAt(index);
	//    }

	//    /// <summary>
	//    /// Gets or sets the element at the specified index.
	//    /// </summary>
	//    /// <returns>
	//    /// The element at the specified index.
	//    /// </returns>
	//    /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
	//    public T this[int index]
	//    {
	//        get
	//        {
	//            throw new NotImplementedException();
	//        }
	//        set
	//        {
	//            throw new NotImplementedException();
	//        }
	//    }

	//    #endregion

	//    #region Implementation of IBindingList

	//    /// <summary>
	//    /// Adds a new item to the list.
	//    /// </summary>
	//    /// <returns>
	//    /// The item added to the list.
	//    /// </returns>
	//    /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.AllowNew"/> is false. </exception>
	//    public object AddNew()
	//    {
	//        var value = default(T);
	//        Add(value);
	//        return value;
	//    }

	//    /// <summary>
	//    /// Adds the <see cref="T:System.ComponentModel.PropertyDescriptor"/> to the indexes used for searching.
	//    /// </summary>
	//    /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to add to the indexes used for searching. </param>
	//    public void AddIndex(PropertyDescriptor property)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Sorts the list based on a <see cref="T:System.ComponentModel.PropertyDescriptor"/> and a <see cref="T:System.ComponentModel.ListSortDirection"/>.
	//    /// </summary>
	//    /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to sort by. </param><param name="direction">One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values. </param><exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
	//    public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Returns the index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor"/>.
	//    /// </summary>
	//    /// <returns>
	//    /// The index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor"/>.
	//    /// </returns>
	//    /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to search on. </param><param name="key">The value of the <paramref name="property"/> parameter to search for. </param><exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSearching"/> is false. </exception>
	//    public int Find(PropertyDescriptor property, object key)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Removes the <see cref="T:System.ComponentModel.PropertyDescriptor"/> from the indexes used for searching.
	//    /// </summary>
	//    /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to remove from the indexes used for searching. </param>
	//    public void RemoveIndex(PropertyDescriptor property)
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Removes any sort applied using <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"/>.
	//    /// </summary>
	//    /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
	//    public void RemoveSort()
	//    {
	//        throw new NotImplementedException();
	//    }

	//    /// <summary>
	//    /// Gets whether you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew"/>.
	//    /// </summary>
	//    /// <returns>
	//    /// true if you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew"/>; otherwise, false.
	//    /// </returns>
	//    public bool AllowNew
	//    {
	//        get
	//        {
	//            return !IsReadOnly;
	//        }
	//    }

	//    /// <summary>
	//    /// Gets whether you can update items in the list.
	//    /// </summary>
	//    /// <returns>
	//    /// true if you can update the items in the list; otherwise, false.
	//    /// </returns>
	//    public bool AllowEdit
	//    {
	//        get
	//        {
	//            return !IsReadOnly;
	//        }
	//    }

	//    /// <summary>
	//    /// Gets whether you can remove items from the list, using <see cref="M:System.Collections.IList.Remove(System.Object)"/> or <see cref="M:System.Collections.IList.RemoveAt(System.Int32)"/>.
	//    /// </summary>
	//    /// <returns>
	//    /// true if you can remove items from the list; otherwise, false.
	//    /// </returns>
	//    public bool AllowRemove
	//    {
	//        get
	//        {
	//            return !IsReadOnly;
	//        }
	//    }

	//    /// <summary>
	//    /// Gets whether a <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> event is raised when the list changes or an item in the list changes.
	//    /// </summary>
	//    /// <returns>
	//    /// true if a <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> event is raised when the list changes or when an item changes; otherwise, false.
	//    /// </returns>
	//    public bool SupportsChangeNotification
	//    {
	//        get
	//        {
	//            return true;
	//        }
	//    }

	//    /// <summary>
	//    /// Gets whether the list supports searching using the <see cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)"/> method.
	//    /// </summary>
	//    /// <returns>
	//    /// true if the list supports searching using the <see cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)"/> method; otherwise, false.
	//    /// </returns>
	//    public bool SupportsSearching
	//    {
	//        get
	//        {
	//            return false;
	//        }
	//    }

	//    /// <summary>
	//    /// Gets whether the list supports sorting.
	//    /// </summary>
	//    /// <returns>
	//    /// true if the list supports sorting; otherwise, false.
	//    /// </returns>
	//    public bool SupportsSorting
	//    {
	//        get
	//        {
	//            return false;
	//        }
	//    }

	//    /// <summary>
	//    /// Gets whether the items in the list are sorted.
	//    /// </summary>
	//    /// <returns>
	//    /// true if <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"/> has been called and <see cref="M:System.ComponentModel.IBindingList.RemoveSort"/> has not been called; otherwise, false.
	//    /// </returns>
	//    /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
	//    public bool IsSorted
	//    {
	//        get
	//        {
	//            return false;
	//        }
	//    }

	//    /// <summary>
	//    /// Gets the <see cref="T:System.ComponentModel.PropertyDescriptor"/> that is being used for sorting.
	//    /// </summary>
	//    /// <returns>
	//    /// The <see cref="T:System.ComponentModel.PropertyDescriptor"/> that is being used for sorting.
	//    /// </returns>
	//    /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
	//    public PropertyDescriptor SortProperty
	//    {
	//        get
	//        {
	//            throw new NotImplementedException();
	//        }
	//    }

	//    /// <summary>
	//    /// Gets the direction of the sort.
	//    /// </summary>
	//    /// <returns>
	//    /// One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values.
	//    /// </returns>
	//    /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
	//    public ListSortDirection SortDirection
	//    {
	//        get
	//        {
	//            throw new NotImplementedException();
	//        }
	//    }

	//    public event ListChangedEventHandler ListChanged;
	//    /// <summary>
	//    /// Raises a PropertyChanged event (per <see cref="INotifyPropertyChanged" />). 
	//    /// </summary> 
	//    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	//    {
	//        if (PropertyChanged != null)
	//        {
	//            PropertyChanged(this, e);
	//        }
	//    }

	//    /// <summary> 
	//    /// Raise CollectionChanged event to any listeners.
	//    /// Properties/methods modifying this collection will raise 
	//    /// a collection changed event through this virtual method. 
	//    /// </summary>
	//    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	//    {
	//        if (CollectionChanged != null)
	//        {
	//            CollectionChanged(this, e);
	//        }
	//    } 

	//    #endregion

	//    #region Implementation of INotifyCollectionChanged

	//    public event NotifyCollectionChangedEventHandler CollectionChanged;

	//    #endregion

	//    #region Implementation of INotifyPropertyChanged

	//    public event PropertyChangedEventHandler PropertyChanged;

	//    #endregion
	//}
}