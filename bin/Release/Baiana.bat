@SET BDS=C:\Program Files (x86)\Embarcadero\RAD Studio\10.0
@SET BDSINCLUDE=C:\Program Files (x86)\Embarcadero\RAD Studio\10.0\include
@SET BDSCOMMONDIR=C:\Users\Public\Documents\RAD Studio\10.0
@SET FrameworkDir=C:\Windows\Microsoft.NET\Framework\v3.5
@SET FrameworkVersion=v3.5
@SET FrameworkSDKDir=
@SET PATH=%FrameworkDir%;%FrameworkSDKDir%;C:\Program Files (x86)\Embarcadero\RAD Studio\10.0\bin;C:\Program Files (x86)\Embarcadero\RAD Studio\10.0\bin64;C:\Users\Public\Documents\InterBase\redist\InterBaseXE3\win32_togo;C:\Users\Public\Documents\InterBase\redist\InterBaseXE3\win64_togo;%PATH%
@SET LANGDIR=EN
@SET CG_BOOST_ROOT=C:\Program Files (x86)\Embarcadero\RAD Studio\10.0\include\boost_1_39
@SET CG_64_BOOST_ROOT=C:\Program Files (x86)\Embarcadero\RAD Studio\10.0\include\boost_1_50
MSBuild "[CAMINHO][PROJETO].dproj" /flp:logfile="[ERRORSLOG]";errorsonly
