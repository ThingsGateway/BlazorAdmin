cd ..
sc create BlazorAdmin binPath= %~dp0BlazorAdmin.Server.exe start= auto 
sc description BlazorAdmin "BlazorAdmin"
Net Start BlazorAdmin
pause
