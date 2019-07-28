﻿using System;
using System.IO;
using System.Text;
using SimdJsonSharp;

namespace ConsoleApp124
{
    class Program
    {
        static unsafe void Main(string[] args)
        {

            string helloWorldJson = @"{ ""answer"": 42, ""name"": ""Egor"" }";
            ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(helloWorldJson);

            //bytes = File.ReadAllBytes(@"C:\prj\simdjson\jsonexamples\numbers.json");

            // SimdJson is UTF8 only

            fixed (byte* ptr = bytes)
            {
                // SimdJsonN -- N stands for Native, it means we are using Bindings for simdjson native lib
                // SimdJson -- fully managed .NET Core 3.0 port
                using (ParsedJson doc = SimdJson.ParseJson(ptr, bytes.Length))
                {
                    Console.WriteLine($"Is json valid:{doc.IsValid}\n");

                    // open iterator:
                    using (var iterator = new ParsedJsonIterator(doc))
                    {
                        while (iterator.MoveForward())
                        {
                            if (iterator.IsInteger)
                                Console.WriteLine("integer: " + iterator.GetInteger());
                            if (iterator.IsString)
                                Console.WriteLine("string: " + iterator.GetUtf16String());
                            if (iterator.IsDouble)
                                Console.WriteLine("double: " + iterator.GetDouble());
                        }
                    }
                }
            }
        }
    }
}
