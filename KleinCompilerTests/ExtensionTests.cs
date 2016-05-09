using System;
using System.Linq;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class ExtensionTests
    {
        [TestCase ('a')]
        [TestCase ('b')]
        [TestCase ('y')]
        [TestCase ('z')]
        [TestCase ('A')]
        [TestCase ('B')]
        [TestCase ('Y')]
        [TestCase ('Z')]
        public void IsAlpha_ShouldReturnTrue_ForAtoZcharacters_CaseInsensitive(char character)
        {
            Assert.That(character.IsAlpha(), Is.True);
        }

        [TestCase('@')]  // before A
        [TestCase('[')]  // after Z
        [TestCase('`')]  // before a
        [TestCase('{')]  // after z
        public void IsAlpha_ShouldReturnFalse_ForOtherCharacters(char character)
        {
            Assert.That(character.IsAlpha(), Is.False);
        }
    }
}