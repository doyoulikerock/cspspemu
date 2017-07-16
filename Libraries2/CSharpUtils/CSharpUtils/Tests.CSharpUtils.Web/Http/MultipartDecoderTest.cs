﻿using System.IO;
using CSharpUtils.Http;
using NUnit.Framework;

namespace CSharpUtilsTests.Http
{
	[TestFixture]
	public class MultipartDecoderTest
	{
		static byte[] rawData = {
			0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x57, 0x65, 0x62, 0x4B, 0x69, 0x74,
			0x46, 0x6F, 0x72, 0x6D, 0x42, 0x6F, 0x75, 0x6E, 0x64, 0x61, 0x72, 0x79,
			0x41, 0x47, 0x78, 0x35, 0x49, 0x78, 0x6E, 0x58, 0x78, 0x5A, 0x30, 0x50,
			0x61, 0x78, 0x6D, 0x32, 0x0D, 0x0A, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E,
			0x74, 0x2D, 0x44, 0x69, 0x73, 0x70, 0x6F, 0x73, 0x69, 0x74, 0x69, 0x6F,
			0x6E, 0x3A, 0x20, 0x66, 0x6F, 0x72, 0x6D, 0x2D, 0x64, 0x61, 0x74, 0x61,
			0x3B, 0x20, 0x6E, 0x61, 0x6D, 0x65, 0x3D, 0x22, 0x54, 0x69, 0x74, 0x6C,
			0x65, 0x22, 0x0D, 0x0A, 0x0D, 0x0A, 0x61, 0x73, 0x64, 0x61, 0x73, 0x64,
			0x61, 0x64, 0x61, 0x73, 0x0D, 0x0A, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D,
			0x57, 0x65, 0x62, 0x4B, 0x69, 0x74, 0x46, 0x6F, 0x72, 0x6D, 0x42, 0x6F,
			0x75, 0x6E, 0x64, 0x61, 0x72, 0x79, 0x41, 0x47, 0x78, 0x35, 0x49, 0x78,
			0x6E, 0x58, 0x78, 0x5A, 0x30, 0x50, 0x61, 0x78, 0x6D, 0x32, 0x0D, 0x0A,
			0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x2D, 0x44, 0x69, 0x73, 0x70,
			0x6F, 0x73, 0x69, 0x74, 0x69, 0x6F, 0x6E, 0x3A, 0x20, 0x66, 0x6F, 0x72,
			0x6D, 0x2D, 0x64, 0x61, 0x74, 0x61, 0x3B, 0x20, 0x6E, 0x61, 0x6D, 0x65,
			0x3D, 0x22, 0x42, 0x6F, 0x64, 0x79, 0x22, 0x0D, 0x0A, 0x0D, 0x0A, 0x64,
			0x61, 0x73, 0x64, 0x61, 0x73, 0x64, 0x0D, 0x0A, 0x2D, 0x2D, 0x2D, 0x2D,
			0x2D, 0x2D, 0x57, 0x65, 0x62, 0x4B, 0x69, 0x74, 0x46, 0x6F, 0x72, 0x6D,
			0x42, 0x6F, 0x75, 0x6E, 0x64, 0x61, 0x72, 0x79, 0x41, 0x47, 0x78, 0x35,
			0x49, 0x78, 0x6E, 0x58, 0x78, 0x5A, 0x30, 0x50, 0x61, 0x78, 0x6D, 0x32,
			0x0D, 0x0A, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x2D, 0x44, 0x69,
			0x73, 0x70, 0x6F, 0x73, 0x69, 0x74, 0x69, 0x6F, 0x6E, 0x3A, 0x20, 0x66,
			0x6F, 0x72, 0x6D, 0x2D, 0x64, 0x61, 0x74, 0x61, 0x3B, 0x20, 0x6E, 0x61,
			0x6D, 0x65, 0x3D, 0x22, 0x46, 0x69, 0x6C, 0x65, 0x22, 0x3B, 0x20, 0x66,
			0x69, 0x6C, 0x65, 0x6E, 0x61, 0x6D, 0x65, 0x3D, 0x22, 0x22, 0x0D, 0x0A,
			0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x2D, 0x54, 0x79, 0x70, 0x65,
			0x3A, 0x20, 0x61, 0x70, 0x70, 0x6C, 0x69, 0x63, 0x61, 0x74, 0x69, 0x6F,
			0x6E, 0x2F, 0x6F, 0x63, 0x74, 0x65, 0x74, 0x2D, 0x73, 0x74, 0x72, 0x65,
			0x61, 0x6D, 0x0D, 0x0A, 0x0D, 0x0A, 0x0D, 0x0A, 0x2D, 0x2D, 0x2D, 0x2D,
			0x2D, 0x2D, 0x57, 0x65, 0x62, 0x4B, 0x69, 0x74, 0x46, 0x6F, 0x72, 0x6D,
			0x42, 0x6F, 0x75, 0x6E, 0x64, 0x61, 0x72, 0x79, 0x41, 0x47, 0x78, 0x35,
			0x49, 0x78, 0x6E, 0x58, 0x78, 0x5A, 0x30, 0x50, 0x61, 0x78, 0x6D, 0x32,
			0x2D, 0x2D, 0x0D, 0x0A
		};

		byte[] rawData2 = {
			0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x57, 0x65, 0x62, 0x4B, 0x69, 0x74,
			0x46, 0x6F, 0x72, 0x6D, 0x42, 0x6F, 0x75, 0x6E, 0x64, 0x61, 0x72, 0x79,
			0x77, 0x75, 0x76, 0x47, 0x47, 0x63, 0x41, 0x74, 0x44, 0x68, 0x37, 0x35,
			0x68, 0x66, 0x47, 0x46, 0x0D, 0x0A, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E,
			0x74, 0x2D, 0x44, 0x69, 0x73, 0x70, 0x6F, 0x73, 0x69, 0x74, 0x69, 0x6F,
			0x6E, 0x3A, 0x20, 0x66, 0x6F, 0x72, 0x6D, 0x2D, 0x64, 0x61, 0x74, 0x61,
			0x3B, 0x20, 0x6E, 0x61, 0x6D, 0x65, 0x3D, 0x22, 0x54, 0x69, 0x74, 0x6C,
			0x65, 0x22, 0x0D, 0x0A, 0x0D, 0x0A, 0x61, 0x73, 0x64, 0x73, 0x61, 0x64,
			0x73, 0x0D, 0x0A, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x57, 0x65, 0x62,
			0x4B, 0x69, 0x74, 0x46, 0x6F, 0x72, 0x6D, 0x42, 0x6F, 0x75, 0x6E, 0x64,
			0x61, 0x72, 0x79, 0x77, 0x75, 0x76, 0x47, 0x47, 0x63, 0x41, 0x74, 0x44,
			0x68, 0x37, 0x35, 0x68, 0x66, 0x47, 0x46, 0x0D, 0x0A, 0x43, 0x6F, 0x6E,
			0x74, 0x65, 0x6E, 0x74, 0x2D, 0x44, 0x69, 0x73, 0x70, 0x6F, 0x73, 0x69,
			0x74, 0x69, 0x6F, 0x6E, 0x3A, 0x20, 0x66, 0x6F, 0x72, 0x6D, 0x2D, 0x64,
			0x61, 0x74, 0x61, 0x3B, 0x20, 0x6E, 0x61, 0x6D, 0x65, 0x3D, 0x22, 0x42,
			0x6F, 0x64, 0x79, 0x22, 0x0D, 0x0A, 0x0D, 0x0A, 0x61, 0x73, 0x64, 0x61,
			0x73, 0x64, 0x61, 0x73, 0x0D, 0x0A, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D,
			0x57, 0x65, 0x62, 0x4B, 0x69, 0x74, 0x46, 0x6F, 0x72, 0x6D, 0x42, 0x6F,
			0x75, 0x6E, 0x64, 0x61, 0x72, 0x79, 0x77, 0x75, 0x76, 0x47, 0x47, 0x63,
			0x41, 0x74, 0x44, 0x68, 0x37, 0x35, 0x68, 0x66, 0x47, 0x46, 0x0D, 0x0A,
			0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x2D, 0x44, 0x69, 0x73, 0x70,
			0x6F, 0x73, 0x69, 0x74, 0x69, 0x6F, 0x6E, 0x3A, 0x20, 0x66, 0x6F, 0x72,
			0x6D, 0x2D, 0x64, 0x61, 0x74, 0x61, 0x3B, 0x20, 0x6E, 0x61, 0x6D, 0x65,
			0x3D, 0x22, 0x46, 0x69, 0x6C, 0x65, 0x22, 0x3B, 0x20, 0x66, 0x69, 0x6C,
			0x65, 0x6E, 0x61, 0x6D, 0x65, 0x3D, 0x22, 0x22, 0x0D, 0x0A, 0x43, 0x6F,
			0x6E, 0x74, 0x65, 0x6E, 0x74, 0x2D, 0x54, 0x79, 0x70, 0x65, 0x3A, 0x20,
			0x61, 0x70, 0x70, 0x6C, 0x69, 0x63, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x2F,
			0x6F, 0x63, 0x74, 0x65, 0x74, 0x2D, 0x73, 0x74, 0x72, 0x65, 0x61, 0x6D,
			0x0D, 0x0A, 0x0D, 0x0A, 0x0D, 0x0A, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D,
			0x57, 0x65, 0x62, 0x4B, 0x69, 0x74, 0x46, 0x6F, 0x72, 0x6D, 0x42, 0x6F,
			0x75, 0x6E, 0x64, 0x61, 0x72, 0x79, 0x77, 0x75, 0x76, 0x47, 0x47, 0x63,
			0x41, 0x74, 0x44, 0x68, 0x37, 0x35, 0x68, 0x66, 0x47, 0x46, 0x2D, 0x2D,
			0x0D, 0x0A
		};


		[Test]
		public void TestMultipartDecoder()
		{
			MultipartDecoder MultipartDecoder = new MultipartDecoder(new MemoryStream(rawData), "------WebKitFormBoundaryAGx5IxnXxZ0Paxm2");
			var Parts = MultipartDecoder.Parse();
			Assert.AreEqual(Parts[0].ContentType, "text/plain");
			Assert.AreEqual(Parts[1].ContentType, "text/plain");
			Assert.AreEqual(Parts[2].ContentType, "application/octet-stream");

			Assert.AreEqual(Parts[0].Name, "Title");
			Assert.AreEqual(Parts[1].Name, "Body");
			Assert.AreEqual(Parts[2].Name, "File");

			Assert.AreEqual(Parts[0].Content, "asdasdadas");
			Assert.AreEqual(Parts[1].Content, "dasdasd");
			Assert.AreEqual(Parts[2].Content, null);

			Assert.AreEqual(Parts[2].Stream.Length, 0);
		}

		[Test]
		public void TestMultipartDecoder2()
		{
			MultipartDecoder MultipartDecoder = new MultipartDecoder(new MemoryStream(rawData2), "------WebKitFormBoundarywuvGGcAtDh75hfGF");
			var Parts = MultipartDecoder.Parse();
			Assert.AreEqual(Parts[0].ContentType, "text/plain");
			Assert.AreEqual(Parts[1].ContentType, "text/plain");
			Assert.AreEqual(Parts[2].ContentType, "application/octet-stream");

			Assert.AreEqual(Parts[0].Name, "Title");
			Assert.AreEqual(Parts[1].Name, "Body");
			Assert.AreEqual(Parts[2].Name, "File");

			Assert.AreEqual(Parts[0].Content, "asdsads");
			Assert.AreEqual(Parts[1].Content, "asdasdas");
			Assert.AreEqual(Parts[2].Content, null);

			Assert.AreEqual(Parts[2].Stream.Length, 0);
		}
	}
}
