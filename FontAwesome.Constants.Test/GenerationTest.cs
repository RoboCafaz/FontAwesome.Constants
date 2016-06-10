using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FontAwesome.Constants.Test
{
    [TestClass]
    public class GenerationTest
    {
        [TestMethod]
        public void FontAwesomeConstants_ShouldHaveGenerated()
        {
            GetGeneratedData();
        }

        [TestMethod]
        public void FontAwesomeConstants_ShouldHaveDefinitionForPhoto()
        {
            var field = GetGeneratedField("PHOTO");
            Assert.AreEqual("\uf03e", field);
        }

        [TestMethod]
        public void FontAwesomeConstants_ShouldHaveDefinitionForPictureO()
        {
            var field = GetGeneratedField("PICTURE_O");
            Assert.AreEqual("\uf03e", field);
        }

        [TestMethod]
        public void FontAwesomeConstants_ShouldHaveDefinitionForImage()
        {
            var field = GetGeneratedField("IMAGE");
            Assert.AreEqual("\uf03e", field);
        }

        private static string GetGeneratedField(string field, FieldInfo[] fields = null)
        {
            fields = fields ?? GetGeneratedData();
            var match = fields.SingleOrDefault(x => x.Name == field);
            Assert.IsNotNull(match);
            var value = match.GetRawConstantValue() as string;
            Assert.IsNotNull(value);
            return value;
        }

        private static FieldInfo[] GetGeneratedData()
        {
            var fields = typeof(FA).GetFields();
            Assert.IsNotNull(fields);
            Assert.IsTrue(fields.Length > 0);
            return fields;
        }
    }
}
