@ECHO OFF
REM Batch file to add CustomData element to JSON schemas.
REM It does so by adding a CustomData to all properties sections
REM and then adding its definition to the definitions sections.
REM Finally, the 'empty definition' is handled as a special case (to avoid a trailing comma).
REM
SET PATH=%PATH%;c:\Program Files\grepWin
REM Check if directory of json files is specified.
IF "%~1"=="" ECHO Parameter for directory is missing. && GOTO End
ECHO Changing directory to: %1
CD %1
CD
ECHO Check that no CustomData is present in JSON...
FINDSTR "CustomData" *.json
IF %ERRORLEVEL% EQU 0 ( ECHO. && ECHO CustomData already present! && ECHO. ) ELSE ECHO OK!
ECHO About to add the CustomData property to JSON schemas. (Ctrl-C to abort)...
PAUSE
REM First change properties sections to add CustomData element to non-empty message bodies
grepWin.exe /closedialog /k:no /size:-1 /searchpath:"." /filemask:"*.json" /searchfor:"^(\s*)?\"properties\":\s\{" /replacewith:"$1\"properties\": {\r\n$1  \"customData\": {\r\n$1    \"$ref\": \"#/definitions/CustomData\"\r\n$1  }," /executereplace
REM Then add a properties section with CustomData to all empty message bodies
grepWin.exe /closedialog /k:no /size:-1 /searchpath:"." /filemask:"*.json" /searchfor:"^(\s*)?(\"additionalProperties\": false)$\r\n\}" /replacewith:"$1$2,\r\n$1\"properties\": {\r\n$1  \"customData\": {\r\n$1    \"$ref\": \"#/definitions/CustomData\"\r\n$1  }\r\n$1}\r\n}" /executereplace
ECHO About to add the CustomData definition to JSON schemas. (Ctrl-C to abort)...
PAUSE
REM Then change non-empty definitions sections to add definition for CustomData
grepWin.exe /closedialog /k:no /size:-1 /searchpath:"." /filemask:"*.json" /searchfor:"^(\s*)?\"definitions\":\s\{\s*$" /replacewith:"$1\"definitions\": {\r\n$1  \"CustomData\": {\r\n$1    \"description\": \"This class does not get 'AdditionalProperties = false' in the schema generation, so it can be extended with arbitrary JSON properties to allow adding custom data.\",\r\n$1    \"javaType\": \"CustomData\",\r\n$1    \"type\": \"object\",\r\n$1    \"properties\": {\r\n$1      \"vendorId\": {\r\n$1        \"type\": \"string\",\r\n$1        \"maxLength\": 255\r\n$1      }\r\n$1    },\r\n$1    \"required\": [\r\n$1      \"vendorId\"\r\n$1    ]\r\n$1  }," /executereplace
REM And change empty definition sections to add definition for CustomData
grepWin.exe /closedialog /k:no /size:-1 /searchpath:"." /filemask:"*.json" /searchfor:"^(\s*)?\"definitions\":\s\{\s*\}" /replacewith:"$1\"definitions\": {\r\n$1  \"CustomData\": {\r\n$1    \"description\": \"This class does not get 'AdditionalProperties = false' in the schema generation, so it can be extended with arbitrary JSON properties to allow adding custom data.\",\r\n$1    \"javaType\": \"CustomData\",\r\n$1    \"type\": \"object\",\r\n$1    \"properties\": {\r\n$1      \"vendorId\": {\r\n$1        \"type\": \"string\",\r\n$1        \"maxLength\": 255\r\n$1      }\r\n$1    },\r\n$1    \"required\": [\r\n$1      \"vendorId\"\r\n$1    ]\r\n$1  }\r\n  }" /executereplace
ECHO Done.
:End
