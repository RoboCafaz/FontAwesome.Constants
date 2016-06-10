using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FontAwesome.Constants.Test
{
    [TestClass]
    public class GenerationTest
    {
        private Dictionary<string, string> _fontDictionary;

        [TestInitialize]
        public void Initialize()
        {
            _fontDictionary = new Dictionary<string, string>();

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
                            _fontDictionary.Add(headerText, text.Value);
                            header = header.NextMatch();
                        }
                    }
                    definition = definition.NextMatch();
                }
            }
            Assert.IsNotNull(_fontDictionary);
            Assert.IsTrue(_fontDictionary.Count > 0);
        }

        [TestMethod]
        public void EnsureConsistentGeneration()
        {
        }
    }
}
