using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectRepository.Entities;

namespace Tests
{
    [TestClass]
    public class RatingTest
    {
        [TestMethod]
        [Description("Test SetRating method")]
        public void Set5()
        {
            var rating = new Rating(9);

            rating.SetRating(9);

            Assert.AreEqual(5, rating.Value);
        }

        [TestMethod]
        [Description("Test SetRating method")]
        public void Set0()
        {
            var rating = new Rating(-1);

            rating.SetRating(-1);

            Assert.AreEqual(1, rating.Value);
        }
    }
}
