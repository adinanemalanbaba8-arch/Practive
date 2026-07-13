using System.Collections;

namespace task03;

public class CustomCollection<T> : IEnumerable<T>
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);

    public void Remove(T item) => _items.Remove(item);

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Итератор для обратного обхода коллекции
    public IEnumerable<T> GetReverseEnumerator()
    {
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            yield return _items[i];
        }
    }

    // Генерация числовой последовательности
    public static IEnumerable<int> GenerateSequence(int start, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return start + i;
            
        }
    }

    // Фильтрация и сортировка с помощью LINQ
    public IEnumerable<T> FilterAndSort(Func<T, bool> predicate, Func<T, IComparable> keySelector)
        => _items.Where(predicate).OrderBy(keySelector);
        // test CI
}
