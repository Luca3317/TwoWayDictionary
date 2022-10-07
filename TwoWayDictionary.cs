using System;
using System.Collections.Generic;
using System.Text;

namespace TwoWayDictionary
{
    /// <summary>
    /// <para>
    /// A bi-directional dictionary.
    /// Every value in the TwoWayDictionary serves both as key and as value (bijective).
    /// <paramref name="T1"/> and <paramref name="T2"/> are required to be of different types; otherwise the constructor will throw an exception.
    /// </para>
    ///
    /// <para>
    /// Warning: Not threadsafe!
    /// Maybe implement later using Double Buffer?
    /// </para>
    /// 
    /// <para>
    /// Class invariant:
    /// At any point in time:
    /// <list type="table">
    ///    <item> For every key x mapped to value y in forward, there is a key y mapped to value x in backward </item>
    ///    <item> For every key x mapped to value y in backward, there is a key y mapped to value x in backward </item>
    /// </list>
    /// </para>
    /// </summary>
    /// <typeparam name="T1">Type used as key in forward / value in backward. Must be a different type from T2</typeparam>
    /// <typeparam name="T2">Type used as value in forward / key in backward. Must be a different type from T1</typeparam>

    public class TwoWayDictionary<T1, T2>
    {
        readonly Dictionary<T1, T2> forward = new Dictionary<T1, T2>();
        readonly Dictionary<T2, T1> backward = new Dictionary<T2, T1>();

        public Exposer<T1, T2> Forward { get; private set; }
        public Exposer<T2, T1> Backward { get; private set; }

        public TwoWayDictionary()
        {
            if (typeof(T1) == typeof(T2)) throw new TypeInitializationException("Generics of same type", new Exception("TwoWayDictionary requires its two generic type parameters to be of different types"));

            Forward = new Exposer<T1, T2>(forward);
            Backward = new Exposer<T2, T1>(backward);
        }

        public int Count => forward.Count;

        public bool Contains(T1 t1) => forward.ContainsKey(t1);
        public bool Contains(T2 t2) => backward.ContainsKey(t2);

        public T2 this[T1 index]
        {
            get { return forward[index]; }
            set
            {
                if (Contains(value)) throw new ArgumentException("The given value is already in the dictionary; TwoWayDictionary allows only 1:1 connections, therefore each value may only be added once, whether as key or as value");
                if (Contains(index))
                    Remove(index);
                Add(index, value);
            }
        }

        public T1 this[T2 index]
        {
            get { return backward[index]; }
            set
            {
                if (Contains(value)) throw new ArgumentException("The given value is already in the dictionary; TwoWayDictionary allows only 1:1 connections, therefore each value may only be added once, whether as key or as value");
                if (Contains(index))
                    Remove(index);
                Add(value, index);
            }
        }

        public void Add(T1 t1, T2 t2)
        {
            if (t1 == null || t2 == null) throw new ArgumentNullException();
            if (forward.ContainsKey(t1)) throw new ArgumentException("Already contains key " + t1.ToString());
            if (backward.ContainsKey(t2)) throw new ArgumentException("Already contains key " + t2.ToString());

            forward.Add(t1, t2);
            backward.Add(t2, t1);
        }

        public void Remove(T1 t1)
        {
            if (t1 == null) throw new ArgumentNullException();

            T2 t2 = forward[t1];
            forward.Remove(t1);
            backward.Remove(t2);
        }

        public void Remove(T2 t2)
        {
            if (t2 == null) throw new ArgumentNullException();

            T1 t1 = backward[t2];
            backward.Remove(t2);
            forward.Remove(t1);
        }

        public void Overwrite(T1 key, T2 newValue, T2 oldValue)
        {
            if (!forward[key].Equals(oldValue)) throw new ArgumentException("The given key and oldValue do not correspond");
            if (backward.ContainsKey(newValue)) throw new ArgumentException("The given newValue is already in the dictionary");

            Remove(key);
            Add(key, newValue);
        }

        public void Overwrite(T2 key, T1 newValue, T1 oldValue)
        {
            if (!backward[key].Equals(oldValue)) throw new ArgumentException("The given key and oldValue do not correspond");
            if (forward.ContainsKey(newValue)) throw new ArgumentException("The given newValue is already in the dictionary");

            Remove(key);
            Add(newValue, key);
        }

        // Allows for exposing more restricted access; essentially normal read-only dictionary
        public class Exposer<T3, T4>
        {
            private readonly Dictionary<T3, T4> dict;

            public Exposer(Dictionary<T3, T4> dict)
            {
                this.dict = dict;
            }

            public T4 this[T3 index]
            {
                get { return dict[index]; }
            }

            public int Count => dict.Count;

            public IReadOnlyCollection<T3> Keys => dict.Keys;
            public IReadOnlyCollection<T4> Values => dict.Values;

            public bool ContainsKey(T3 key)
            {
                return dict.ContainsKey(key);
            }

            public bool ContainsValue(T4 value)
            {
                return dict.ContainsValue(value);
            }
        }
    }

}
