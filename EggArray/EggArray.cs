/// <summary>
/// EggArray
/// Created by chenjd
/// http://www.cnblogs.com/murongxiaopifu/
/// </summary>
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Linq;
namespace EggToolkit
{
	public class EggArray<T> where T : class
	{
		public delegate void IterationHandler(T item);
		public delegate bool IterationBoolHandler(T item);
		public delegate T IterationVauleHandler(T item);
		
		private int capacity;
		private int count;
		private T[] items;
		private int growthFactor = 2;

		public EggArray() : this(8)
		{
		}
		
		public EggArray(int capacity)
		{
			this.capacity = capacity;
			this.items = new T[capacity];
		}
		/// <summary>
		/// Attr
		/// </summary>
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		public int Capacity
		{
			get
			{
				return this.capacity;
			}
		}
		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty
		{
			get
			{
				return this.count == 0;
			}
		}
		/// <summary>
		/// Gets a value indicating whether this instance is full.
		/// </summary>
		/// <value><c>true</c> if this instance is full; otherwise, <c>false</c>.</value>
		public bool IsFull
		{
			get
			{
				return this.count == this.capacity;
			}
		}
		// Methods
		/// <summary>
		/// Resize this instance capacity.
		/// </summary>
		private void Resize()
		{
			int capacity = this.capacity * growthFactor;
			if (this.count > capacity)
			{
				this.count = capacity;
			}
			T[] destinationArray = new T[capacity];
			Array.Copy(this.items, destinationArray, this.count);
			this.items = destinationArray;
			this.capacity = capacity;
		}
		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Add(T item)
		{
			if (this.count >= this.capacity)
			{
				this.Resize();
			}
			this.items[this.count++] = item;
		}
		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="collection">Collection.</param>
		public void AddRange(IEnumerable<T> collection)
		{
			if (collection != null)
			{
				foreach (T current in collection)
				{
					this.Add(current);
				}
			}
		}
		/// <summary>
		/// Insert the specified index and item.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="item">Item.</param>
		public void Insert(int index, T item)
		{
			if (this.count >= this.capacity)
			{
				this.Resize();
			}
			this.count++;
			for (int i = this.count - 1; i > index; i--)
			{
				this.items[i] = this.items[i - 1];
			}
			this.items[index] = item;
		}
		/// <summary>
		/// Contains the specified arg.
		/// </summary>
		/// <param name="arg">Argument.</param>
		public bool Contains(T arg)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.items[i].Equals(arg))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear()
		{
			if (this.count > 0)
			{
				for (int i = 0; i < this.count; i++)
				{
					this.items[i] = null;
				}
				this.count = 0;
			}
		}
		/// <summary>
		/// Tos the array.
		/// </summary>
		/// <param name="array">Array.</param>
		public void ToArray(T[] array)
		{
			if (array != null)
			{
				for (int i = 0; i < this.count; i++)
				{
					array[i] = this.items[i];
				}
			}
		}
		/// <summary>
		/// Sort the specified comparer.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		public void Sort(IComparer<T> comparer)
		{
			Array.Sort<T>(this.items, 0, this.count, comparer);
		}
		/// <summary>
		/// Foreach the specified handler.
		/// </summary>
		/// <param name="handler">Handler.</param>
		public void Foreach(EggArray<T>.IterationHandler handler)
		{
			for (int i = 0; i < this.count; i++)
			{
				handler(this.items[i]);
			}
		}
		/// <summary>
		/// Remove the specified arg.
		/// </summary>
		/// <param name="arg">Argument.</param>
		public bool Remove(T arg)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.items[i].Equals(arg))
				{
					this.items[i] = null;
					this.Compact();
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Removes at index.
		/// </summary>
		/// <param name="index">Index.</param>
		public void RemoveAt(int index)
		{
			if (index < this.count)
			{
				this.items[index] = null;
				this.Compact();
			}
		}

		/// <summary>
		/// Indexs the of.
		/// </summary>
		/// <returns>The of.</returns>
		/// <param name="arg">Argument.</param>
		public int IndexOf(T arg)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.items[i].Equals(arg))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// First this instance.
		/// </summary>
		public T First()
		{
			if (this.count > 0)
			{
				return this.items[0];
			}
			return null;
		}

		public T[] First(int n)
		{
			if(n < 0)
			{
				return null;
			}
			if (this.count > n)
			{
				T[] targetArray = this.Slice(0, n);
				return targetArray;
			}
			else
			{
				return this.items;
			}
		}

		public T Last()
		{
			if (this.count > 0)
			{
				return this.items[this.count - 1];
			}
			return null;
		}

		public T[] Last(int n)
		{
			if(n < 0)
			{
				return null;
			}
			if(this.count > n)
			{
				int startIndex = this.count - 1 - n;
				T[] target = this.Slice(startIndex, this.count - 1);
				return target;
			}
			else
			{
				return this.items;
			}
		}
		/// <summary>
		/// Slice the specified start and end.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public T[] Slice(int start, int end)
		{
			if (end < 0)
			{
				end = this.items.Length + end;
			}
			int len = end - start;
			
			T[] res = new T[len];
			for (int i = 0; i < len; i++)
			{
				res[i] = this.items[i + start];
			}
			return res;
		}
		/// <summary>
		/// Get the specified i.
		/// </summary>
		/// <param name="i">The index.</param>
		public T Get(int i)
		{
			return this.items[i];
		}
		/// <summary>
		/// Set the specified index and item.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="item">Item.</param>
		public void Set(int index, T item)
		{
			this.items[index] = item;
		}
		/// <summary>
		/// Adds the first.
		/// </summary>
		/// <param name="item">Item.</param>
		public void AddFirst(T item)
		{
			this.Insert(0, item);
		}
		/// <summary>
		/// Removes the last.
		/// </summary>
		/// <returns>The last.</returns>
		public T RemoveLast()
		{
			T result = null;
			if (this.count > 0)
			{
				result = this.items[this.count - 1];
				this.items[this.count - 1] = null;
				this.count--;
			}
			return result;
		}
		/// <summary>
		/// Compact this instance.
		/// </summary>
		private void Compact()
		{
			int num = 0;
			for (int i = 0; i < this.count; i++)
			{
				if (this.items[i] == null)
				{
					num++;
				}
				else
				{
					if (num > 0)
					{
						this.items[i - num] = this.items[i];
						this.items[i] = null;
					}
				}
			}
			this.count -= num;
		}
		/// <summary>
		/// Containses the strict.
		/// </summary>
		/// <returns><c>true</c>, if strict was containsed, <c>false</c> otherwise.</returns>
		/// <param name="arg">Argument.</param>
		public bool ContainsStrict(T arg)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.items[i] == arg)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Indexs the of strict.
		/// </summary>
		/// <returns>The of strict.</returns>
		/// <param name="arg">Argument.</param>
		public int IndexOfStrict(T arg)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.items[i] == arg)
				{
					return i;
				}
			}
			return -1;
		}
		/// <summary>
		/// Tries the get.
		/// </summary>
		/// <returns>The get.</returns>
		/// <typeparam name="F">The 1st type parameter.</typeparam>
		public F TryGet<F>() where F : class, T
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.items[i] is F)
				{
					return (F)((object)this.items[i]);
				}
			}
			return null;
		}
		/// <summary>
		/// Lasts the index of.
		/// </summary>
		/// <returns>The index of.</returns>
		/// <param name="arg">Argument.</param>
		public int LastIndexOf(T arg)
		{
			for (int i = this.count - 1; i >= 0; i--)
			{
				if (this.items[i].Equals(arg))
				{
					return i;
				}
			}
			return -1;
		}

		//Liked UnderScore.js Array Functions
	
		/// <summary>
		/// Map the specified handler.
		/// 通过变换函数把items中的每个值映射到一个新的数组中
		/// </summary>
		/// <param name="handler">Handler.</param>
		public EggArray<T> Map(EggArray<T>.IterationVauleHandler handler)
		{
			EggArray<T> targetArray = new EggArray<T>(this.capacity);
			for(int i = 0; i < this.count; i++)
			{
				T t = handler(this.items[i]);
				targetArray.Add(t);
			}
			return targetArray;
		}
		/// <summary>
		/// Find the specified handler.
		/// </summary>
		/// <param name="handler">Handler.</param>
		public T Find(EggArray<T>.IterationBoolHandler handler)
		{
			for(int i = 0; i < this.count; i++)
			{
				if(handler(this.items[i]))
				{
					return this.items[i];
				}
			}
			return null;
		}
		/// <summary>
		/// Filter the specified handler.
		/// 遍历items中的每个值，返回包含所有通过真值检测的元素值。
		/// </summary>
		/// <param name="handler">Handler.</param>
		public EggArray<T> Filter(EggArray<T>.IterationBoolHandler handler)
		{
			EggArray<T> targetArray = new EggArray<T>();
			for(int i = 0; i < this.count; i++)
			{
				if(handler(this.items[i]))
				{
					T t = this.items[i];
					targetArray.Add(t);
				}
			}
			return targetArray;
		}
		/// <summary>
		/// Every the specified handler.
		/// 如果items中的所有元素都通过真值检测就返回true
		/// </summary>
		/// <param name="handler">Handler.</param>
		public bool Every(EggArray<T>.IterationBoolHandler handler)
		{
			for(int i = 0; i < this.count; i++)
			{
				if(!handler(this.items[i]))
				{
					return false;
				}
			}
			return true;
		}
		/// <summary>
		/// Some the specified handler.
		/// 如果items中有任何一个元素通过真值检测就返回true。
		/// </summary>
		/// <param name="handler">Handler.</param>
		public bool Some(EggArray<T>.IterationBoolHandler handler)
		{
			for(int i = 0; i < this.count; i++)
			{
				if(handler(this.items[i]))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Partition the specified handler.
		/// 拆分一个数组为两个数组
		/// </summary>
		/// <param name="handler">Handler.</param>
		public EggArray<T>[] Partition(IterationBoolHandler handler)
		{
			EggArray<T> targetArray = new EggArray<T>();
			EggArray<T> targetArray2 = new EggArray<T>();
			for(int i = 0; i < this.count; i++)
			{
				if(handler(this.items[i]))
					targetArray.Add(this.items[i]);
				else
					targetArray2.Add(this.items[i]);
			}
			EggArray<T>[] target = new EggArray<T>[2];
			target[0] = targetArray;
			target[1] = targetArray2;
			return target;
		}
		/// <summary>
		/// Difference the specified others.
		/// </summary>
		/// <param name="others">Others.</param>
		public EggArray<T> Difference(EggArray<T> others)
		{
			EggArray<T> targetArray = new EggArray<T>();
			targetArray = 
			this.Filter(delegate(T item) {
				bool b = !others.Contains(item);
				return b;
			});
			return targetArray;
		}
		/// <summary>
		/// Uniq this instance.
		/// 返回去重后的副本
		/// </summary>
		public EggArray<T> Uniq()
		{
			EggArray<T> targetArray = new EggArray<T>();
			for(int i = 0; i < this.count; i++)
			{
				if(targetArray.Contains(this.items[i]))
					continue;
				targetArray.Add(this.items[i]);
			}
			return targetArray;
		}
		/// <summary>
		/// Invoke the specified methodName.
		/// 每个元素上执行methodName方法
		/// </summary>
		/// <param name="methodName">Method name.</param>
		public void Invoke(string methodName)
		{
			Type t = typeof(T);
			var method = t.GetMethod(methodName);
			if(method == null)
				throw new Exception("没有找到指定的方法哦~,可能不叫" + methodName);
			for(int i = 0; i < this.count; i++)
			{
				method.Invoke(this.items[i], null);
			}
		}
		/// <summary>
		/// Pluck the specified fieldName.
		/// 萃取对象数组中某属性值，返回一个数组
		/// 由于字段类型不确定，所以需要装箱
		/// </summary>
		/// <param name="fieldName">Field name.</param>
		public object[] Pluck(string fieldName)
		{
			Type t = typeof(T);
			object[] targetArray = new object[this.count];
			var field = t.GetField(fieldName);
			if(field == null)
				throw new Exception("没有找到指定的field哦~,可能不叫" + fieldName);
			for(int i = 0; i < this.count; i++)
			{
				object value = field.GetValue(this.items[i]);
				targetArray[i] = value;
			}
			return targetArray;
		}
		/// <summary>
		/// Shuffle this instance.
		/// 返回一个随机乱序的副本
		/// </summary>
		public T[] Shuffle()
		{
			T[] shuffled = new T[this.count];
			Random random = new Random();
			for (int index = 0, rand; index < this.count; index++) {
				rand = random.Next(index);
				if (rand != index) 
					shuffled[index] = shuffled[rand];
				shuffled[rand] = this.items[index];
			}
			return shuffled;
		}
		/// <summary>
		/// Sample the specified n.
		/// </summary>
		/// <param name="n">N.</param>
//		public T[] Sample(int n = 0)
//		{
//			Random random = new Random();
//			if(n== 0)
//				return this.items[random.Next(this.count)];
//			return this.Shuffle();
//		}

	}
}
