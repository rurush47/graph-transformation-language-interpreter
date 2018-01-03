using System.IO;
using System.Text;

namespace GraphTransformationLanguage
{
    public class Source
    {
        public int LineNumber { get; private set; }
        public int PositionInLine { get; private set; }
        public bool EndOfFile { get; private set; }
        private StreamReader streamReader;

        public Source(string fileName)
        {
            LineNumber = 1;
            PositionInLine = 1;
            
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            streamReader = new StreamReader(fs, Encoding.ASCII, false, (int)fs.Length, true);
        }
        
        public char GetNextChar()
        {
            char[] buffer = new char[1];
            using (streamReader)
            {
                if (streamReader.Peek() >= 0)
                {
                    streamReader.Read(buffer, 0 ,1);
                    char c = buffer[0];
                    
                    if (c == '\n')
                    {
                        LineNumber += 1;
                        PositionInLine = 1;
                    }
                    else
                    {
                        PositionInLine += 1;
                    }
                    return c;
                }
                streamReader.Close();
                EndOfFile = true;
                return ' ';
            }
        }

        public string GetCurrentPosition()
        {
            return "Line: " + LineNumber + " Position: " + PositionInLine;
        }

        public Position GetPosition()
        {
            return new Position()
            {
                Line = LineNumber,
                Symbol = PositionInLine
            };
        }
    }
}