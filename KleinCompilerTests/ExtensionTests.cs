using System;
using System.Collections.Generic;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class ExtensionTests
    {
        #region IsAlpa

        [TestCase('a')]
        [TestCase('b')]
        [TestCase('y')]
        [TestCase('z')]
        [TestCase('A')]
        [TestCase('B')]
        [TestCase('Y')]
        [TestCase('Z')]
        public void IsAlpha_ShouldReturnTrue_ForAtoZcharacters_CaseInsensitive(char character)
        {
            Assert.That(character.IsAlpha(), Is.True);
        }

        [TestCase('@')] // before A
        [TestCase('[')] // after Z
        [TestCase('`')] // before a
        [TestCase('{')] // after z
        public void IsAlpha_ShouldReturnFalse_ForOtherCharacters(char character)
        {
            Assert.That(character.IsAlpha(), Is.False);
        }

        #endregion

        #region AddIfNotNull

        [Test]
        public void IfItemIsNull_NothingIsAddedToList()
        {
            var tokens = new List<Token>();

            tokens.AddIfNotNull(null);

            Assert.That(tokens.Count, Is.EqualTo(0));
        }

        [Test]
        public void IfItemIsNotNull_TokenIsAddedToList()
        {
            var tokens = new List<Token>();

            tokens.AddIfNotNull(new IdentifierToken("a"));

            Assert.That(tokens.Count, Is.EqualTo(1));
        }

        #endregion
    }
}