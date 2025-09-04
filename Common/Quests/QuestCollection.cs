using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace deeprockitems.Common.Quests
{
    public class QuestCollection : IEnumerable<QuestData>
    {
        private QuestData[] _internalArray;
        private int _capacity = 0;
        private int _count = 0;
        public int Count => _count;
        public QuestCollection()
        {
            _internalArray = new QuestData[_capacity];
        }
        public QuestCollection Where(Func<QuestData, bool> predicate)
        {
            QuestCollection toReturn = new();
            foreach (QuestData data in this)
            {
                if (data is null) continue;
                if (predicate.Invoke(data))
                {
                    toReturn.Add(data);
                }
            }
            return toReturn;
        }
        /// <summary>
        /// Returns a random element from this collection using Terraria's randomization algorithm.
        /// </summary>
        /// <returns></returns>
        public QuestData TakeRandom()
        {
            int index = Main.rand.Next(0, Where(q => q.Predicate).Count);
            return _internalArray[index];
        }
        public QuestCollection Add(QuestData questToAdd)
        {
            if (_internalArray.Length >= Array.MaxLength)
            {
                throw new InvalidOperationException();
            }
            else
            {
                if (_internalArray.Length == 0)
                {
                    _capacity = 4;
                }
                else if (_count + 1 > _capacity)
                {
                    _capacity *= 2;
                }
                Array.Resize(ref _internalArray, _capacity);
                _internalArray[_count] = questToAdd;
                _count++;
                return this;
            }
        }
        public QuestCollection Add(QuestID questType, int typeRequired, int amountRequired, bool hardmode)
        {
            return Add(new QuestData(questType, typeRequired, amountRequired, hardmode));
        }

        public IEnumerator<QuestData> GetEnumerator() => new QuestEnumerator(_internalArray);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public QuestData this[int i] { get => _internalArray[i]; set => _internalArray[i] = value; }
    }
    public class QuestEnumerator : IEnumerator<QuestData>
    {
        private QuestData[] _quests;
        int _index = -1;
        public QuestEnumerator(QuestData[] quests)
        {
            _quests = quests;
        }
        public QuestData Current
        {
            get
            {
                try
                {
                    return _quests[_index];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            _index++;
            return _index < _quests.Length;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
