﻿using System.IO;
using System.Text;
using System.Linq;
using CSharpUtils.Extensions;
using NUnit.Framework;
using CSharpUtils.Misc.Acme1;

namespace CSharpUtilsTests
{
    [TestFixture]
    public class Acme1FileTest
    {
        [Test]
        public void LoadTest()
        {
            var acme1File = new Acme1File();
            var testEncoding = Encoding.UTF8;
            acme1File.Load(new MemoryStream(testEncoding.GetBytes(@"
## POINTER 0 [t:1;r:1;ru:a;user:a;time:1246275105]
Text
With Line

## POINTER 2 [t:1;r:1;ru:a;user:a;time:1246275060]
	 Spaces at the beggining

## POINTER 3 [t:1;r:1;ru:b;user:a;time:1254188688]
Another single-line text.

## POINTER 43 [t:1;r:1;ru:b;user:b;time:1246275060]
Multi line text
with several new lines at the end.



## POINTER 571 [t:1;r:1;ru:b;user:b;time:1246275060]
End of the file.

")), testEncoding);
            Assert.AreEqual(
                "Text\r\nWith Line",
                acme1File[0].Text
            );
            Assert.AreEqual(
                "	 Spaces at the beggining",
                acme1File[2].Text
            );
            Assert.AreEqual(
                "Another single-line text.",
                acme1File[3].Text
            );
            Assert.AreEqual(
                "Multi line text\r\nwith several new lines at the end.",
                acme1File[43].Text
            );
            Assert.AreEqual(
                "End of the file.",
                acme1File[571].Text
            );
            Assert.AreEqual(
                "0,2,3,43,571",
                acme1File.Select(Entry => Entry.Id).ToStringArray()
            );
        }
    }
}