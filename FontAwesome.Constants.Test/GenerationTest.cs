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
        public void FontAwesomeConstants_ShouldAllHaveBeenGenerated()
        {
            var dictionary = BuildLocalDictionary();
            var fields = GetGeneratedData();
            Assert.AreEqual(dictionary.Count, fields.Length);
            foreach (var entry in dictionary)
            {
                var value = GetGeneratedField(entry.Key, fields);
                Assert.IsNotNull(value);
                Assert.AreEqual(entry.Value, value);
            }
        }

        [TestMethod]
        public void FontAwesomeConstants_ShouldHaveDefinitionForPhoto()
        {
            var field = GetGeneratedField("PHOTO");
            Assert.AreEqual(@"\f03e", field);
        }

        [TestMethod]
        public void FontAwesomeConstants_ShouldHaveDefinitionForPictureO()
        {
            var field = GetGeneratedField("PICTURE_O");
            Assert.AreEqual(@"\f03e", field);
        }

        [TestMethod]
        public void FontAwesomeConstants_ShouldHaveDefinitionForImage()
        {
            var field = GetGeneratedField("IMAGE");
            Assert.AreEqual(@"\f03e", field);
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
            var fields = typeof(FontAwesome).GetFields();
            Assert.IsNotNull(fields);
            Assert.IsTrue(fields.Length > 0);
            return fields;
        }

        private static IDictionary<string, string> BuildLocalDictionary()
        {
            var fontDictionary = new Dictionary<string, string>();

            var numbers = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            var definitionRegex = new Regex("(\\.fa([^{}])+?):before(.+?)}", RegexOptions.IgnoreCase);
            var textRegex = new Regex("(?<=content:\")(.+?)(?=\")", RegexOptions.IgnoreCase);
            var headerRegex = new Regex("(?<=\\.fa-)(.+?)(?=:)", RegexOptions.IgnoreCase);

            var path = "../../../FontAwesome.Constants/css/font-awesome.min.css";
            using (StreamReader sr = new StreamReader(path))
            {
                var line = sr.ReadToEnd();
                var definition = definitionRegex.Match(line);
                while (definition.Success)
                {
                    var text = textRegex.Match(definition.Value);
                    if (text.Success)
                    {
                        var header = headerRegex.Match(definition.Value);
                        while (header.Success)
                        {
                            var headerText = header.Value.Replace("-", "_").Replace("fa", "").Replace(".", "");
                            for (var i = 0; i < 10; i++)
                            {
                                headerText = headerText.Replace(i + "", numbers[i] + "");
                            }
                            headerText = headerText.ToUpper();
                            fontDictionary.Add(headerText, text.Value);
                            header = header.NextMatch();
                        }
                    }
                    definition = definition.NextMatch();
                }
            }
            Assert.IsNotNull(fontDictionary);
            Assert.IsTrue(fontDictionary.Count > 0);
            return fontDictionary;
        }
    }
}
