using System.Collections.Generic;

namespace Enemy
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
}