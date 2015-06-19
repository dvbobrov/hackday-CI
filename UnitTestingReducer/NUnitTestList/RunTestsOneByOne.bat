@echo off

NUnitTestList.exe %* > test-list.txt
for /F "tokens=*" %%A in (test-list.txt) do ..\..\NUnit\bin\nunit-console-x86.exe /out:%%A.xml /run:%%A %*