using System.Collections.Generic;

namespace AlgoLib
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITrieNode<TValue>
    {
        bool IsTerminal { get; set; }

        string Key { get; }

        TValue Value { get; set; }
        
        ITrieNode<TValue> Add(char key);

        void Clear();

        IEnumerable<ITrieNode<TValue>> GetAllNodes();

        IEnumerable<TrieEntry<TValue>> GetByPrefix();

        void Remove();

        bool TryGetValue(char key, out ITrieNode<TValue> node);
    }
}
