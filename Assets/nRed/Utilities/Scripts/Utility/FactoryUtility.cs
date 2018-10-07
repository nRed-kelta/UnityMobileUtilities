// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace nRed.FactoryUtil
{
	//===================================================================
	/**
	 * @brief	Factory interface
	 */
	//===================================================================
	public interface IFactory<T>
	{
		ICollection<T> Alloc(int length);
		void Release(ref ICollection<T> target);
	}
	//===================================================================
	/**
	 * @brief	Factory for jagged array interface
	 */
	//===================================================================
	public interface IJaggedFactory<T>
	{
		T[,] Alloc(int length, int length2);
		void Release(ref T[,] target);
	}

	//===================================================================
	/**
	 * @brief	Factory of Array/List abstract class
	 */
	//===================================================================
	public abstract class ACollectionFactory<T> : IFactory<T>
	{
		internal static class Config
		{
			public static int STACKDICT_CAP = 32;
			public static int STACK_CAP = 32;
		}

		protected Dictionary<int, Stack<ICollection<T>>> stackDict = new Dictionary<int, Stack<ICollection<T>>>( Config.STACKDICT_CAP );

		public bool isClearOnAlloc		= true;

		protected virtual int CacheIndex(int length)
		{
			return length;
		}

		protected virtual ICollection<T> CreateCollection(int length)
		{
			return null;
		}

		public ICollection<T> Alloc(int length)
		{
			ICollection<T> a = null;
			Stack<ICollection<T>> stack;
			
			if( stackDict.TryGetValue( CacheIndex(length), out stack ) && stack.Count>0 )
			{
				a = stack.Pop();
				if( isClearOnAlloc )
					a.Clear();
			}
			else
				a = CreateCollection(length);
			
			return a;
		}

		public void Release(ref ICollection<T> target)
		{
			if(target==null)
				return;
			
			if( isClearOnAlloc==false )
				target.Clear();
			
			Stack<ICollection<T>> stack;
			if( stackDict.TryGetValue( CacheIndex(target.Count), out stack )==false )
				stack = new Stack<ICollection<T>>(Config.STACK_CAP);
			
			stack.Push(target);
			target = null;
		}

	}

	//===================================================================
	/**
	* @brief	Factory with cache for fixed size array
	*/
	//===================================================================
	public class CArray<Type> : ACollectionFactory<Type>
	{
		protected override ICollection<Type> CreateCollection(int length)
		{
			return new Type[length];
		}
	}

	//===================================================================
	/**
	* @brief	Factory with cache for dynamic size list
	*/
	//===================================================================
	public class CList<Type> : ACollectionFactory<Type>
	{
		protected override int CacheIndex(int length)
		{
			return 0;
		}

		protected override ICollection<Type> CreateCollection(int length)
		{
			return new List<Type>(length);
		}
	}

	//===================================================================
	/**
	* @brief	Factory of Array/List abstract class
	*/
	//===================================================================
	public abstract class AJaggedArrayFactory<Type> : IJaggedFactory<Type>
	{
		internal static class Config
		{
			public static int STACKDICT_CAP = 32;
			public static int STACK_CAP = 32;
		}

		protected Dictionary<ulong, Stack<Type[,]>> stackDict = new Dictionary<ulong, Stack<Type[,]>>( Config.STACKDICT_CAP );

		public bool isClearOnAlloc		= true;

		protected virtual ulong CacheIndex(int length, int length2)
		{
			return (ulong)((uint)length<<32 | (uint)length2);
		}

		protected virtual Type[,] CreateCollection(int length, int length2)
		{
			return null;
		}

		public Type[,] Alloc(int length, int length2)
		{
			Type[,] a = null;
			Stack<Type[,]> stack;
			
			if( stackDict.TryGetValue( CacheIndex(length, length2), out stack ) && stack.Count>0 )
			{
				a = stack.Pop();
				if( isClearOnAlloc )
					System.Array.Clear(a, 0, a.Length);
			}
			else
				a = CreateCollection(length, length2);
			
			return a;
		}

		public void Release(ref Type[,] target)
		{
			if(target==null)
				return;
			
			if( isClearOnAlloc==false )
				System.Array.Clear(target, 0, target.Length);
			
			Stack<Type[,]> stack;
			if( stackDict.TryGetValue( CacheIndex(target.GetLength(0), target.GetLength(1)), out stack )==false )
				stack = new Stack<Type[,]>(Config.STACK_CAP);
			
			stack.Push(target);
			target = null;
		}
	}

	//===================================================================
	/**
	* @brief	Factory with cache for fixed size jagged array
	*/
	//===================================================================
	public class CJArray<Type> : AJaggedArrayFactory<Type>
	{
		protected override Type[,] CreateCollection(int length, int length2)
		{
			return new Type[length, length2];
		}
	}
	
	//===================================================================
	/**
	* @brief	Factory with cache for dynamic size list
	*/
	//===================================================================
	public class CStack<Type> : ACollectionFactory<Type>
	{
		protected override int CacheIndex(int length)
		{
			return 0;
		}

		protected override ICollection<Type> CreateCollection(int length)
		{
			return new Stack<Type>(length) as ICollection<Type>;
		}
	}
}