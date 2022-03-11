# CryptDetect
Project to detect potential cryptographic algorithms that have been used in given projects/source code. 



# Installation Guide
You will need to install the following to operate these frontend files:
.NET SDK's:
    .NET Core 3.1:  https://dotnet.microsoft.com/en-us/download/dotnet/3.1
    .NET 5.0:       https://dotnet.microsoft.com/en-us/download/dotnet/5.0
    .NET 6.0:       https://dotnet.microsoft.com/en-us/download/dotnet/6.0


IDE:
    Visual Studio: https://visualstudio.microsoft.com/downloads
    Visual Studio Code: https://code.visualstudio.com/download


For VISUAL STUDIO refer to the following instructions:
    You will need IISExpress turned on for this to compile. To turn this feature on you will need
    to simply navigate in WINDOWS to your control panel --> programs --> Programs and Features -->
    Turn Windows features on or off. Navigate to the check box and make sure it is marked for the
    following:
        Internet Information Services
    
    After this is completed make sure to configure the VISUAL STUDIO IDE via the Visual Studio Installer.
    Click on Modify for Visual Studio Community 2022 (or your current edition), and install the ASP.NET
    and web development, Azure development, and Python development packages. 
    Under the ASP.NET installation package, make sure that the following check box is marked:
        Development time IIS support
    
    Now you may load CryptDetect's folder as a project into Visual Studio and build the current index.cshtml
    or any other page found under the Views directory. Make sure the build/debug settings are switched to
    IISExpress for runtime. You may need to reopen the folder with Visual Studio's if you opened it prior
    to IIS Installation and Modification.
    Once the build is completed via IISExpress, it will force the project open as a localhost in your
    browser.


For VISUAL STUDIO CODE refer to the following instructions:
    Launch VISUAL STUDIO CODE and under the extensions tab, select to install the following:
        .NET Install Tool for Extension Authors
        .NET Interactive Notebooks
        C#
        REST Client
        MSBuild project tools

    After these extensions have been installed, it is safe to load the folder of CryptDetect as a
    project into Visual Studio Code, and then simply click on Run --> Start Without Debugging.
    This will take a moment to load but it will force the project open as a localhost in your
    browser.