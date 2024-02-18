using System;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    private int itemCount;

    public Heap(int _maxHeapSize) { items = new T[_maxHeapSize]; }

    public void Add(T _item)
    {
        _item.HeapIndex = itemCount;
        items[itemCount] = _item;
        SortUp(_item);
        itemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        items[0] = items[--itemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    private void SortUp(T _item)
    {
        int parentIndex = (_item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (_item.CompareTo(parentItem) > 0)
                Swap(_item, parentItem);
            else break;
            parentIndex = (_item.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T _item)
    {
        while (true)
        {
            int leftChild = _item.HeapIndex * 2 + 1;
            int rightChild = _item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            if (leftChild < itemCount)
            {
                swapIndex = leftChild;
                if (rightChild < itemCount && items[leftChild].CompareTo(items[rightChild]) < 0)
                    swapIndex = rightChild;
                if (_item.CompareTo(items[swapIndex]) < 0)
                    Swap(_item, items[swapIndex]);
                else return;
            }
            else return;
        }
    }

    private void Swap(T _first, T _second)
    {
        items[_first.HeapIndex] = _second;
        items[_second.HeapIndex] = _first;
        int temp = _first.HeapIndex;
        _first.HeapIndex = _second.HeapIndex;
        _second.HeapIndex = temp;
    }

    public bool Contains(T _item) => Equals(items[_item.HeapIndex], _item);

    public int Count { get => itemCount; }

    public void UpdateItem(T _item) => SortUp(_item);
}

public interface IHeapItem<T> : IComparable<T> { int HeapIndex { get; set; } }