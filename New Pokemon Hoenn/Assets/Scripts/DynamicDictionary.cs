using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic representation of a dictionary that is constructed at runtime via the Initialize() method
/// </summary>
/// <typeparam name="T">The type of the dictionary key</typeparam>
/// <typeparam name="Y">The type of the dictionary value</typeparam>

[System.Serializable] public class DynamicDictionary<T, Y> {
    [SerializeField] private List<Entry> entries;
    private Dictionary<T, Y> _dictionary;
    public bool Initialized { get; private set; }

    /// <summary>
    /// Returns a copy of the dictionary, or null if not Initialized
    /// </summary>
    public Dictionary<T, Y> Dictionary {
        get => Initialized ? new Dictionary<T, Y>(_dictionary) : null;
    }

    public void Initialize() {
        _dictionary = new Dictionary<T, Y>();
        foreach(Entry e in entries){
            _dictionary.TryAdd(e.key, e.value);
        }
        Initialized = true;
    }

    [System.Serializable] public struct Entry {
        public T key;
        public Y value;
    }
}
