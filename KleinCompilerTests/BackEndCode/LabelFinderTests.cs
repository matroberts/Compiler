using System;
using System.Linq;
using KleinCompiler.BackEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.BackEndCode
{
    [TestFixture]
    public class LabelFinderTests
    {
        [Test]
        public void LabelFinder_ShouldFindTheFirstLineNumber_AfterTheLabel()
        {
            string pretemplate = @"0: LDC 6, 9(0) ; set registers 6 to value 9
1: LDA 7, [label0](0)  ; jump to label label0

2: LDC 1, 1(0)     ; load constant into r1
3: OUT 1, 0, 0           ; write out contens of r1
* label:label0

4: LDC 1, 2(0)     ; load constant into r1
5: OUT 1, 0, 0           ; write out contens of r1";

            var dictionary = LabelFinder.FindLabels(pretemplate);

            Assert.That(dictionary.Count, Is.EqualTo(1));
            Assert.That(dictionary["label0"], Is.EqualTo("4"));
        }

        [Test]
        public void LabelFinder_ShouldWorkWithBigLineNumbers()
        {
            string pretemplate = @"0: LDC 6, 9(0) ; set registers 6 to value 9
101: LDA 7, [label0](0)  ; jump to label label0

102: LDC 1, 1(0)     ; load constant into r1
103: OUT 1, 0, 0           ; write out contens of r1
* label:label0

104: LDC 1, 2(0)     ; load constant into r1
105: OUT 1, 0, 0           ; write out contens of r1";

            var dictionary = LabelFinder.FindLabels(pretemplate);

            Assert.That(dictionary.Count, Is.EqualTo(1));
            Assert.That(dictionary["label0"], Is.EqualTo("104"));
        }

        [Test]
        public void LabelFinder_ShouldWorkWithIntermediateComments()
        {
            string pretemplate = @"0: LDC 6, 9(0) ; set registers 6 to value 9
101: LDA 7, [label0](0)  ; jump to label label0

102: LDC 1, 1(0)     ; load constant into r1
103: OUT 1, 0, 0           ; write out contens of r1
* label:label0
* Somthing complicated starting
* It really is
104: LDC 1, 2(0)     ; load constant into r1
105: OUT 1, 0, 0           ; write out contens of r1";

            var dictionary = LabelFinder.FindLabels(pretemplate);

            Assert.That(dictionary.Count, Is.EqualTo(1));
            Assert.That(dictionary["label0"], Is.EqualTo("104"));
        }

        [Test]
        public void LabelFinder_OnlyFindsTheFirstLabel_IfThereAreTwoLabelsToTheSameLine_HopefullyThisIsntAProblem()
        {
            string pretemplate = @"0: LDC 6, 9(0) ; set registers 6 to value 9
101: LDA 7, [label0](0)  ; jump to label label0

102: LDC 1, 1(0)     ; load constant into r1
103: OUT 1, 0, 0           ; write out contens of r1
* label:label0
* label:label1
104: LDC 1, 2(0)     ; load constant into r1
105: OUT 1, 0, 0           ; write out contens of r1";

            var dictionary = LabelFinder.FindLabels(pretemplate);

            Assert.That(dictionary.Count, Is.EqualTo(1));
            Assert.That(dictionary["label0"], Is.EqualTo("104"));
        }

        [Test]
        public void LabelFinder_FindsMultipleLabels()
        {
            string pretemplate = @"0: LDC 6, 9(0) ; set registers 6 to value 9
101: LDA 7, [label0](0)  ; jump to label label0

102: LDC 1, 1(0)     ; load constant into r1
* label:label2
103: OUT 1, 0, 0           ; write out contens of r1
* label:label0
104: LDC 1, 2(0)     ; load constant into r1
* label:label1
105: OUT 1, 0, 0           ; write out contens of r1";

            var dictionary = LabelFinder.FindLabels(pretemplate);

            Assert.That(dictionary.Count, Is.EqualTo(3));
            Assert.That(dictionary["label0"], Is.EqualTo("104"));
            Assert.That(dictionary["label1"], Is.EqualTo("105"));
            Assert.That(dictionary["label2"], Is.EqualTo("103"));
        }
    }
}