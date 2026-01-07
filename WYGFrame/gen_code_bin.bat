set WORKSPACE=..

set GEN_CLIENT=dotnet %WORKSPACE%\Luban.ClientServer\Luban.ClientServer.dll
set CONF_ROOT=%WORKSPACE%\DesignerConfigs
set "SOURCE=Assets\Gen\Data"
set "TARGET=Assets\StreamingAssets\Data"


%GEN_CLIENT% --template_search_path CustomTemplates -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Datas ^
 --output_code_dir Assets/Gen/Code ^
 --output_data_dir Assets/Gen/Data ^
 --gen_types code_cs_bin,data_bin ^
--cs:use_unity_vector ^
 -s all 

if exist "%TARGET%" (
    rd /s /q "%TARGET%"
)

rem 复制文件夹
xcopy /e /i /y "%SOURCE%" "%TARGET%"

pause