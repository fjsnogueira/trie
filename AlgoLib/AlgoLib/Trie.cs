using System.Collections.Generic;

namespace AlgoLib
{
    /// <summary>
    /// Implementation of trie data structure.
    /// </summary>
    /// <typeparam name="TValue">The type of values in the trie.</typeparam>
    public class Trie<TValue> : TrieBase<TValue>
    {
        private TrieNode root;

        /// <summary>
        /// Initializes a new instance of the <see cref="Trie{TValue}"/>.
        /// </summary>
        /// <param name="comparer">Comparer.</param>
        public Trie(IEqualityComparer<char> comparer)
            : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trie{TValue}"/>.
        /// </summary>
        public Trie()
            : this(EqualityComparer<char>.Default)
        {
        }

        protected override ITrieNode<TValue> Root
        {
            get
            {
                if (root == null)
                {
                    root = new TrieNode(char.MinValue, Comparer);
                }

                return root;
            }
        }

        /// <summary>
        /// <see cref="Trie{TValue}"/>'s node.
        /// </summary>
        private sealed class TrieNode : ITrieNode<TValue>
        {
            private readonly Dictionary<char, TrieNode> children;

            private readonly IEqualityComparer<char> comparer;

            private readonly char keyChar;

            internal TrieNode(char keyChar, IEqualityComparer<char> comparer)
            {
                this.keyChar = keyChar;
                this.comparer = comparer;
                children = new Dictionary<char, TrieNode>(comparer);
            }

            public bool IsTerminal { get; set; }

            public string Key
            {
                get
                {
                    ////var result = new StringBuilder().Append(keyChar);

                    ////TrieNode node = this;

                    ////while ((node = node.Parent).Parent != null)
                    ////{
                    ////    result.Insert(0, node.keyChar);
                    ////}

                    ////return result.ToString();
                    
                    var stack = new Stack<char>();
                    stack.Push(keyChar);

                    TrieNode node = this;

                    while ((node = node.Parent).Parent != null)
                    {
                        stack.Push(node.keyChar);
                    }

                    return new string(stack.ToArray());
                }
            }

            public TValue Value { get; set; }

            private TrieNode Parent { get; set; }

            public ITrieNode<TValue> Add(char key)
            {
                TrieNode childNode;

                if (!children.TryGetValue(key, out childNode))
                {
                    childNode = new TrieNode(key, comparer)
                        {
                            Parent = this
                        };

                    children.Add(key, childNode);
                }

                return childNode;
            }

            public void Clear()
            {
                children.Clear();
            }

            public IEnumerable<ITrieNode<TValue>> GetAllNodes()
            {
                foreach (var child in children)
                {
                    if (child.Value.IsTerminal)
                    {
                        yield return child.Value;
                    }

                    foreach (var item in child.Value.GetAllNodes())
                    {
                        if (item.IsTerminal)
                        {
                            yield return item;
                        }
                    }
                }
            }

            public IEnumerable<TrieEntry<TValue>> GetByPrefix()
            {
                if (IsTerminal)
                {
                    yield return new TrieEntry<TValue>(Key, Value);
                }

                foreach (var item in children)
                {
                    foreach (var element in item.Value.GetByPrefix())
                    {
                        yield return element;
                    }
                }
            }

            public void Remove()
            {
                IsTerminal = false;

                if (children.Count == 0 && Parent != null)
                {
                    Parent.children.Remove(keyChar);

                    if (!Parent.IsTerminal)
                    {
                        Parent.Remove();
                    }
                }
            }

            public bool TryGetValue(char key, out ITrieNode<TValue> node)
            {
                TrieNode tmp;
                var result = children.TryGetValue(key, out tmp);

                node = tmp;

                return result;
            }
        }
    }
}