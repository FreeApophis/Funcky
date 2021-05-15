using System;
using System.IO;
using System.Text;
using Funcky.Monads;
using Xunit;

namespace Funcky.Test.Monads
{
    public sealed class IoTest
    {
        [Fact(Skip = "interactive test")]
        public void Test()
        {
            Io<int> query = from unit1 in Io.Return(() => Console.WriteLine("File path:"))
                            from filePath in Io.Return(Console.ReadLine)
                            from unit2 in Io.Return(() => Console.WriteLine("File encoding:"))
                            from encodingName in Io.Return(Console.ReadLine)
                            let encoding = Encoding.GetEncoding(encodingName)
                            from fileContent in Io.Return(() => File.ReadAllText(filePath, encoding))
                            from unit3 in Io.Return(() => Console.WriteLine("File content:"))
                            from unit4 in Io.Return(() => Console.WriteLine(fileContent))
                            select fileContent.Length;

            int result = query(); // Execute query.
        }
    }
}
