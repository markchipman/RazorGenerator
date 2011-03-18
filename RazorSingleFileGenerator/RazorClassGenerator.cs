/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using VSLangProj80;

namespace Microsoft.Web.RazorSingleFileGenerator {
    /// <summary>
    /// This is the generator class. 
    /// When setting the 'Custom Tool' property of a C#, VB, or J# project item to "XmlClassGenerator", 
    /// the GenerateCode function will get called and will return the contents of the generated file 
    /// to the project system
    /// </summary>
    [ComVisible(true)]
    [Guid("52B316AA-1997-4c81-9969-83604C09EEB4")]
    [CodeGeneratorRegistration(typeof(RazorClassGenerator), "C# Razor Generator (.cshtml)", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true)]
    //[CodeGeneratorRegistration(typeof(RazorClassGenerator), "VB Razor Generator (.vbhtml)", vsContextGuids.vsContextGuidVBProject, GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(RazorClassGenerator))]
    public class RazorClassGenerator : BaseCodeGeneratorWithSite {
#pragma warning disable 0414
        //The name of this generator (use for 'Custom Tool' property of project item)
        internal static string name = "RazorClassGenerator";
#pragma warning restore 0414

        /// <summary>
        /// Function that builds the contents of the generated file based on the contents of the input file
        /// </summary>
        /// <param name="inputFileContent">Content of the input file</param>
        /// <returns>Generated file as a byte array</returns>
        protected override byte[] GenerateCode(string inputFileContent) {
            var codeGenerator = new RazorCodeGenerator() { ErrorHandler = GeneratorError };
            if (this.CodeGeneratorProgress != null) {
                codeGenerator.CompletionHandler = CodeGeneratorProgress.Progress;
            }

            // Get the root folder of the project
            var appRoot = Path.GetDirectoryName(GetProject().FullName);

            // Determine the project-relative path
            string projectRelativePath = InputFilePath.Substring(appRoot.Length);

            var razorHost = new CompiledWebRazorHost(FileNameSpace, projectRelativePath, InputFilePath);
            
            return codeGenerator.GenerateCode(inputFileContent, razorHost, GetCodeProvider());
        }
    }
}