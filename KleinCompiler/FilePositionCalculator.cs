using System.Collections.Generic;
using System.Linq;

namespace KleinCompiler
{
    // adapted from http://stackoverflow.com/a/35840759
    public class FilePositionCalculator
    {
        private List<int> _newlinePositions = new List<int>();

        public FilePositionCalculator(string input)
        {
            _newlinePositions.Add(-1);
            for (int i = 0; i < input.Length; i++)
            {
                if(input[i] == '\n')
                    _newlinePositions.Add(i);
            }
        }

        public int Lines => _newlinePositions.Count;

        /// <summary>
        /// Returns the line number and character line position in the file
        /// Uses one based indexing, i.e. first character in the file is at postion (1,1)  
        /// </summary>
        /// <param name="position">zero based character position in the string</param>
        /// <returns></returns>
        public FilePosition FilePosition(int position)
        {
            // https://msdn.microsoft.com/en-us/library/w4e7fxsh(v=vs.110).aspx
            // The zero-based index of item in the sorted List<T>, if item is found; 
            // otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item 
            // or, if there is no larger element, the bitwise complement of Count.
            var result = _newlinePositions.BinarySearch(position);
            if (result < 0)
                result = ~result;

            int lineStartPos = _newlinePositions[result - 1];
             
            return new FilePosition(result, position-lineStartPos);
        }

        public override string ToString()
        {
            return string.Join(", ", _newlinePositions);
        }
    }

    public struct FilePosition
    {
        public FilePosition(int lineNumber, int linePosition)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        public int LineNumber { get; }
        public int LinePosition { get; }
        public override string ToString()
        {
            return $"({LineNumber}, {LinePosition})";
        }
    }
}