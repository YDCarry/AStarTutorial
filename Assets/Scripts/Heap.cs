using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T>{
	T[] items;
	int current_item_count;

	public Heap(int max_heap_size)
	{
		items = new T[max_heap_size];
	}

	public void Add(T item)
	{
		item.HeapIndex = current_item_count;
		items [current_item_count] = item;
		SortUp (item);
		current_item_count++;
	}

	public T RemoveFirst()
	{
		T first_item = items[0];
		current_item_count--;
		items[0] = items[current_item_count];
		items[0].HeapIndex = 0;
		SortDown(items[0]);
		return first_item;
	}

	public void UpdateItem(T item)
	{
		SortUp (item);
	}

	public int Count
	{
		get{
			return current_item_count;
		}
	}

	public bool Contains(T item)
	{
		return Equals (items [item.HeapIndex], item);
	}

	void SortDown(T item)
	{
		while (true) {
			int child_index_left = item.HeapIndex * 2 + 1;
			int child_index_right = item.HeapIndex * 2 + 2;
			int swap_index = 0;

			if(child_index_left < current_item_count)
			{
				swap_index = child_index_left;

				if(child_index_right < current_item_count)
				{
					if(items[child_index_left].CompareTo(items[child_index_right]) < 0)
					{
						swap_index = child_index_right;
					}
				}

				if (item.CompareTo (items [swap_index]) < 0) {
					Swap (item, items [swap_index]);
				} 
				else
				{
					return;
				}
			}
			else
			{
				return;
			}
		}
	}

	void SortUp(T item)
	{
		int parent_index = (item.HeapIndex - 1) / 2;
		while (true) {
			T parent_item = items [parent_index];
			if (item.CompareTo (parent_item) > 0) {
				Swap (item, parent_item);
			} else {
				break;
			}
		}
	}

	void Swap(T item1, T item2)
	{
		items [item1.HeapIndex] = item2;
		items [item2.HeapIndex] = item1;
		int item1_index = item1.HeapIndex;
		item1.HeapIndex = item2.HeapIndex;
		item2.HeapIndex = item1_index;
	}
}

public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex {
		get;
		set;
	}
}