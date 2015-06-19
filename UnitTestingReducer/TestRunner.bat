@echo off

set runlist=%1
shift
set params=%1
:loop
shift
if [%1]==[] goto afterloop
set params=%params% %1
goto loop
:afterloop
..\..\NUnit\bin\nunit-console-x86.exe /runlist:%runlist% /xml:TestResult.xml %params%