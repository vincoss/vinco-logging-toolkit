echo Build start

%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe "Build.proj" /t:go /fileLogger

REM pause