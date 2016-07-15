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
        #region SymbolType Attribute

        public enum TestEnum
        {
            [SymbolType(SymbolType.NonTerminal)]
            Program,
            [SymbolType(SymbolType.Token)]
            Identifier,
            [SymbolType(SymbolType.Semantic)]
            MakeDefinition,
            WillThrowExceptionBecauseNoSymbolType
        }

        [Test]
        public void ToSymbolType_ShouldReturnTheKeywordIfItExists_OtherThrow()
        {
            Assert.That(TestEnum.Program.ToSymbolType(), Is.EqualTo(SymbolType.NonTerminal));
            Assert.That(TestEnum.Identifier.ToSymbolType(), Is.EqualTo(SymbolType.Token));
            Assert.That(TestEnum.MakeDefinition.ToSymbolType(), Is.EqualTo(SymbolType.Semantic));
            Assert.That(() => TestEnum.WillThrowExceptionBecauseNoSymbolType.ToSymbolType(), Throws.ArgumentException.With.Message.EqualTo("Add SymbolTypeAttribute to TestEnum.WillThrowExceptionBecauseNoSymbolType if you want to call ToSymbolType()."));
        }

        #endregion

        #region IsAlpha

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

        #region IsNumeric

        [TestCase('0')]
        [TestCase('1')]
        [TestCase('8')]
        [TestCase('9')]
        public void IsNumeric_ShouldReturnTrue_For0to9characters(char character)
        {
            Assert.That(character.IsNumeric(), Is.True);
        }

        [TestCase('/')]
        [TestCase(':')]
        public void IsNumeric_ShouldReturnFalse_ForNonNumerals(char character)
        {
            Assert.That(character.IsNumeric(), Is.False);
        }


        #endregion

        #region TruncateWithElipses

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
            Assert.That(text.TruncateWithElipses(25), Is.EqualTo(new string('a', 22) + "..."));
        }

        [Test]
        public void TruncateWithElipses_WillThrow_IfLenghtIsThreeOrLess()
        {
            Assert.That(() => "aaaa".TruncateWithElipses(3), Throws.ArgumentException.With.Message.EqualTo("TruncateWithElipses must be called with length 4 or more"));
        }

        #endregion

    }
}