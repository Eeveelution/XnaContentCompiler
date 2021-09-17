using System;
using System.IO;
using XnaContentCompiler.Compilers;

string inPath = args[0];
string extension = Path.GetExtension(inPath);

switch (extension) {
    case ".png": {
        Console.WriteLine("Detected Image File\nCompiling as Texture2D...");

        string outPath = Path.GetFileNameWithoutExtension(inPath) + ".xnb";

        Texture2dCompiler texture2dCompiler = new(inPath);
        texture2dCompiler.Compile(outPath);

        Console.WriteLine($"Wrote to {outPath}; have fun!");

        break;
    }
}

Console.ReadLine();