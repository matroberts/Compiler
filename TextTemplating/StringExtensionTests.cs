using System;
using System.Linq;
using NUnit.Framework;

namespace TextTemplating
{
    [TestFixture]
    public class StringExtensionTests
    {
        [Test]
        public void TruncateWithElipses_ReturnsTheString_IfItIsLessThanOrEqualTo25Characters()
        {
            string text = new string('a', 25);
            Assert.That(text.TruncateWithElipses(25), Is.EqualTo(new string('a', 25)));
        }

        [Test]
        public void TruncateWithElipses_ReturnsA25CharacterString_FinishingWithThreeElipses_IfTheStringIsLongerThat25Characters()
        {
            string text = new string('a', 26);
            Assert.That(text.TruncateWithElipses(25), Is.EqualTo(new string('a', 22)+"..."));
        }
        
        [Test]
        public void TruncateWithElipses_WillThrow_IfLenghtIsThreeOrLess()
        {
            Assert.That(() => "aaaa".TruncateWithElipses(3), Throws.ArgumentException.With.Message.EqualTo("TruncateWithElipses must be called with length 4 or more"));
        }
    }
}