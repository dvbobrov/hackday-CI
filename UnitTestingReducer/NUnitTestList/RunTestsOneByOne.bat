@echo off

NUnitTestList.exe %* > test-list.txt
for /F "tokens=*" %%A in (test-list.txt) do "C:\Program Files (x86)\NCover\NCover.Console.exe" //xml %%A.ncov ..\..\NUnit\bin\nunit-console-x86.exe /xml:%%A.xml /run:%%A %*