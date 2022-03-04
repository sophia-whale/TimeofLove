using System;
using NUnit.Framework;
using TabbedTemplate.Converters;
using TabbedTemplate.Utils;

namespace TabbedTemplate.UnitTest.Converters {
    public class BoolConverterTest {
        [Test]
        public void TestImageSourceToBoolConverter() {
            var imageSourceToBoolConverter = new ImageSourceToBoolConverter();
            var resultNull = imageSourceToBoolConverter.Convert(null, null, null, null);
            Assert.IsFalse((bool?) resultNull);

            byte[] bytes = new byte[1];
            var result = imageSourceToBoolConverter.Convert(bytes, null, null, null);
            Assert.IsTrue((bool?) result);
        }

        [Test]
        public void TestImageSourceToBoolConvertBack()
        {
            var imageSourceToBoolConverter = new ImageSourceToBoolConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                imageSourceToBoolConverter.ConvertBack(null, null, null,
                    null));
        }

        [Test]
        public void TestNegativeBoolConverter() {
            var negativeBoolConverter = new NegativeBoolConverter();
            //not bool
            var resultNull = negativeBoolConverter.Convert(0, null, null, null);
            Assert.IsNull(resultNull);

            //bool true
            var resultTrue =
                negativeBoolConverter.Convert(true, null, null, null);
            Assert.IsFalse((bool?) resultTrue);

            //bool false
            var resultFalse =
                negativeBoolConverter.Convert(false, null, null, null);
            Assert.IsTrue((bool?) resultFalse);
        }

        [Test]
        public void TestNegativeBoolConvertBack()
        {
            var negativeBoolConverter = new NegativeBoolConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                negativeBoolConverter.ConvertBack(null, null, null,
                    null));
        }

        [Test]
        public void TestNegativeImageSourceToBoolConverter() {
            var negativeImageSourceToBoolConverter = new NegativeImageSourceToBoolConverter();
            //null
            var resultNull =
                negativeImageSourceToBoolConverter.Convert(null, null, null,
                    null);
            Assert.IsTrue((bool?) resultNull);

            //notNull
            byte[] bytes = new byte[1];
            var resultNotNull =
                negativeImageSourceToBoolConverter.Convert(bytes, null, null,
                    null);
            Assert.IsFalse((bool?) resultNotNull);
        }

        [Test]
        public void TestNegativeImageSourceToBoolConvertBack()
        {
            var negativeImageSourceToBoolConverter = new NegativeImageSourceToBoolConverter();
            Assert.Catch<DoNotCallThisException>(() =>
                negativeImageSourceToBoolConverter.ConvertBack(null, null, null,
                    null));
        }
    }
}