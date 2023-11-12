namespace CustomClasses
{
    public class Pair<T, K>
    {
        public T FirstValue;
        public K SecondValue;

        public Pair(T firstValue, K secondValue)
        {
            FirstValue = firstValue;
            SecondValue = secondValue;
        }
    }
}