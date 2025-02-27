using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace com.knapp.CodingContest.core
{
    public class JavaPropertiesFile
    {
        /// <summary>
        /// full path to the file containing the properties
        /// </summary>
        private readonly string filename;

        /// <summary>
        /// store for the read properties
        /// </summary>
        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();

        public JavaPropertiesFile(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                throw new ConfigurationException("Properties file not found:" + filename);
            }

            this.filename = filename;
        }

        /// <summary>
        /// Get the value of the property named 'key'
        /// </summary>
        /// <param name="key">the key of the property to retrieve</param>
        /// <returns>the value of the property</returns>
        /// <exception cref="KeyNotFoundException">when no property for 'key' is stored</exception>
        public string Get(string key)
        {
            return properties[key];
        }

        /// <summary>
        /// Get the value of the property named 'key' as integer
        /// </summary>
        /// <param name="key">the key of the property to retrieve</param>
        /// <returns>the value of the property</returns>
        /// <exception cref="KeyNotFoundException">when no property for 'key' is stored</exception>
        /// <exception cref="FormatException">when the value could not be converted to an int</exception>
        public int GetInt(string key)
        {
            return int.Parse(properties[key]);
        }

        /// <summary>
        /// Load the properties file and parse the contents to the distionary
        /// </summary>
        /// <returns>this - syntactic suger to allow operator joining</returns>
        public JavaPropertiesFile Load()
        {
            foreach (string line in File.ReadAllLines(filename))
            {
                string trimmedLine = line.Trim();

                if (!string.IsNullOrWhiteSpace(trimmedLine)
                    && !IsComment(trimmedLine))
                {
                    if (ParseLine(trimmedLine, out string key, out string value))
                    {
                        if (!properties.ContainsKey(key))
                        {
                            properties.Add(key, value.Trim());
                        }
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Split the line into key and value, both are Trim'ed
        /// </summary>
        /// <param name="trimmedLine">line to split</param>
        /// <param name="key">trimmed key part</param>
        /// <param name="value">trimmed value part</param>
        /// <returns>currently always true</returns>
        private bool ParseLine(string trimmedLine, out string key, out string value)
        {
            int seperatorPos = trimmedLine.IndexOf('=');

            key = trimmedLine.Substring(0, seperatorPos).Trim();
            value = trimmedLine.Substring(seperatorPos + 1).Trim();

            return true;
        }

        /// <summary>
        /// Is the line a comment line
        /// </summary>
        /// <param name="trimmedLine">the line to check, must be trimmed that is the first char must be the first char in the line</param>
        /// <returns>true when line is a comment line, false in all other cases</returns>
        private static bool IsComment(string trimmedLine)
        {
            return !string.IsNullOrEmpty(trimmedLine)
                    &&
                    (
                        trimmedLine[0] == '#'
                        || trimmedLine[0] == '!'
                    )
                    ;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var element in properties)
            {
                sb.AppendLine($"{element.Key,20} = {element.Value}");
            }

            return sb.ToString();
        }
    }
}
