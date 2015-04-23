namespace ObjectRepository.Entities
{
    public class Rating
    {
        public Rating(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }                                        

        public void SetRating(int value)
        {
            if (value > 5)
            {
                value = 5;
            }

            if (value < 1)
            {
                value = 1;
            }

            Value = value;            
        }
    }
}
