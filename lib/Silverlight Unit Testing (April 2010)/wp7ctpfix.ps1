#
# Copyright (c) Microsoft Corporation.  All rights reserved.
#
function Compile-Csharp ([string] $code, $FrameworkVersion="v2.0.50727", 
[Array]$References)
{
    #
    # Get an instance of the CSharp code provider
    #
    $cp = new-object Microsoft.CSharp.CSharpCodeProvider

    #
    # Build up a compiler params object...
    $framework = Join-Path $env:windir "Microsoft.NET\Framework\$FrameWorkVersion"
    $refs = New-Object Collections.ArrayList
    $refs.AddRange( @("${framework}\System.dll"))
    if ($references.Count -ge 1)
    {
        $refs.AddRange($References)
    }

    $cpar = New-Object System.CodeDom.Compiler.CompilerParameters
    $cpar.GenerateInMemory = $true
    $cpar.GenerateExecutable = $false
    $cpar.OutputAssembly = "custom"
    $cpar.ReferencedAssemblies.AddRange($refs)
    $cr = $cp.CompileAssemblyFromSource($cpar, $code)

    if ( $cr.Errors.Count)
    {
        $codeLines = $code.Split("`n");
        foreach ($ce in $cr.Errors)
        {
            write-host "Error: $($codeLines[$($ce.Line - 1)])"
            $ce |out-default
        }
        Throw "INVALID DATA: Errors encountered while compiling code"
    }
}

$code = @'
using System;
using System.Runtime.InteropServices;

namespace RemoveCertScript
{
    public class RemoveCertclass
    {
        public const short INVALID_HANDLE_VALUE = -1;
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_DELETE = 0x00000004;
        public const uint OPEN_EXISTING = 3;
        public const uint CERT_SECTION_TYPE_ANY = 255;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess,
            uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,
            uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("Imagehlp.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ImageEnumerateCertificates(IntPtr hFile, uint wTypeFilter,
            ref uint dwCertCount, IntPtr pIndices, IntPtr pIndexCount);

        [DllImport("Imagehlp.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ImageRemoveCertificate(IntPtr hFile, uint dwCertCount);

        public static void Run(string filename)
        {
          IntPtr hFile = CreateFile(filename, 
              GENERIC_READ | GENERIC_WRITE, 
              FILE_SHARE_READ | FILE_SHARE_DELETE, 
              IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

          if (hFile.ToInt32() == -1)
          {
              /* ask the framework to marshall the win32 error code to an exception */
              Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
          }
          else
          {
              while (true)
              {
                  uint certCount = 0;
                  if (!ImageEnumerateCertificates(hFile, CERT_SECTION_TYPE_ANY, ref certCount, IntPtr.Zero, IntPtr.Zero))
                      break;

                  if (certCount == 0)
                      break;

                  for (uint certIndex = 0; certIndex < certCount; certIndex++)
                  {
                      if (!ImageRemoveCertificate(hFile, certIndex))
                      {
                          CloseHandle(hFile);
                       Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                      }
                  }
              }
              CloseHandle(hFile);
              Console.WriteLine("Cert removal succeeded.");
          }
        }
    }
}
'@

##################################################################
# So now we compile the code and use .NET object access to run it.
##################################################################
compile-CSharp $code
for ($counter = 1; $counter -le $args.Length; $counter++)
{
    $source = $args[$counter - 1]
    $srcFilename = $source.substring($source.lastindexofany("\") +1 ,$source.length - ($source.lastindexofany("\")+1)) 
    $directory = $source.substring(0, ($source.lastindexofany("\")+1))
    $destFilename = "WP7_CTP_Fix_" + $srcFilename
    $destination = $directory + $destFilename
    copy-item $source -destination $destination
    [RemoveCertScript.RemoveCertClass]::Run($destination)
}