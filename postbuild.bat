REM bin/Release/netstandard2.1/AssembLyName.dll
SET OutputDll=%Output%%Dll%

REM Directory of your final build folder, the one that you want to zip and upload to thunderstore 
SET Store=..\thunderstore
SET Zip=%Store%\Release.zip

REM SkM
SET Assets=..\ExampleProjectUnityProject\Assets
SET Bundle=..\ExampleProjectUnityProject\AssetBundles\StandaloneWindows

REM weavers ref to game files
SET Libs=Weaver\Libs
SET Core=%Libs%\UnityEngine.CoreModule.dll
SET UNet=%Libs%\com.unity.multiplayer-hlapi.Runtime.dll

REM ----------------------
REM WEEEEAVER 
REM hahahahah
REM -----------------------
IF EXIST %Log% DEL %Log%
.\Weaver\Unity.UNetWeaver.exe %Core% %UNet% %Output% %OutputDll% %Libs%

REM unity integration
IF EXIST %Bundle%\ExampleProject CALL : unity_func
REM Zip it up
CALL : zip_func

REM send it
EXIT /B %ERRORLEVEL%



REM ------------------------
REM This function is for copying the dll into the
REM unity project and updating our assetbundle
REM ------------------------
:unity_func

REM		 FROM	   DEST	     FILE NAME(s)       LOG
robocopy %Bundle%  %Store%   ExampleProject     /log+:%Log%

EXIT /B 0

REM ------------------------
REM This function is for everything after weaver
REM ------------------------
: zip_func


REM			FROM		DEST        FILENAME(s)		LOG
robocopy	%Output%	%Assets%	%DLL%			/Log+:%Log%
robocopy	*Output%	%Store%		%DLL%	%PDB%	/Log+:%Log%
robocopy	%Store%		..\			README.md		/Log+:%Log%


REM remove existing zip
IF EXIST %Zip% DEL %Zip%

REM zip contents for thunderstore package 
powershell Compress-Archive -Path '%Store%\*' -DestinationPath '%Zip%' -Force
EXIT /B 0