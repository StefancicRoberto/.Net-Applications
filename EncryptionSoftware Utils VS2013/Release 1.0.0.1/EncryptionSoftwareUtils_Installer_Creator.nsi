Name "Encryption Software Utils"

!define INSTALLER_NAME "Encryption Software Utils"

!include "MUI2.nsh"

!define MUI_ICON "Utils/AppIco.ico"

!define MUI_WELCOMEPAGE_TITLE "Encryption Software Utils Install Version 1.0.0.1."
!define MUI_WELCOMEPAGE_TEXT "Free use license software. Software based on Windows integrated features. Please acknowledge credits to Stefancic Roberto Stefan."
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES


VIProductVersion "1.0.0.1"
VIAddVersionKey ProductName "Encryption Software"
VIAddVersionKey ProductVersion "1.0.0.1"
VIAddVersionKey CompanyName "Stefancic Roberto Stefan"
VIAddVersionKey FileVersion "1.0.0.1"
VIAddVersionKey FileDescription "Free Use License"
VIAddVersionKey LegalCopyright "Stefancic Roberto Stefan"

OutFile "${INSTALLER_NAME}.exe"


;===================================Install Section=======================================

InstallDir "$PROGRAMFILES\EncryptionSoftwareUtils"

Section "install"
SetOutPath "$INSTDIR"
File /r "Utils"
File "Files\ControlCenter.exe"
File "Files\EncryptionSoftware.exe"
File "Files\Microsoft.Win32.TaskScheduler.dll"
File "Files\Microsoft.Win32.TaskScheduler.xml"

writeUninstaller "Uninstall.exe"

CreateShortCut "$DESKTOP\Encryption Software Utils Control Center.lnk" "$INSTDIR\ControlCenter.exe" ""

;write uninstall information to the registry
WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${INSTALLER_NAME}" "DisplayName" "${INSTALLER_NAME} (remove only)"
WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${INSTALLER_NAME}" "UninstallString" "$INSTDIR\Uninstall.exe"
WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${INSTALLER_NAME}" "DisplayIcon" "$INSTDIR\Utils\AppIco.ico"

SectionEnd

Function .onInstSuccess
  MessageBox MB_OK "You have successfully installed ${INSTALLER_NAME}."
FunctionEnd

;===================================Uninstall Section=======================================

function un.OnInit
MessageBox MB_OKCANCEL "Are you sure you want to remove ${INSTALLER_NAME}?" IDOK next
	Abort
next:
functionEnd

Section "uninstall"
RMDir /r "$INSTDIR\Utils"
delete "$INSTDIR\ControlCenter.exe"
delete "$INSTDIR\EncryptionSoftware.exe"
delete "$INSTDIR\Microsoft.Win32.TaskScheduler.dll"
delete "$INSTDIR\Microsoft.Win32.TaskScheduler.xml"
delete "$INSTDIR\uninstall.exe"
rmDir "$INSTDIR"

Delete "$DESKTOP\Encryption Software Utils Control Center.lnk"

DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\${INSTALLER_NAME}"
DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${INSTALLER_NAME}"

SectionEnd

Function un.onUninstSuccess
  MessageBox MB_OK "You have successfully uninstalled ${INSTALLER_NAME}."
FunctionEnd

;===================================Verify for .net 3.5=======================================

!define MIN_FRA_MAJOR "3"
!define MIN_FRA_MINOR "5"
!define MIN_FRA_BUILD "*"

Function .onInit
	Call AbortIfBadFramework
FunctionEnd


Function AbortIfBadFramework
 
  ;Save the variables in case something else is using them
  Push $0
  Push $1
  Push $2
  Push $3
  Push $4
  Push $R1
  Push $R2
  Push $R3
  Push $R4
  Push $R5
  Push $R6
  Push $R7
  Push $R8
 
  StrCpy $R5 "0"
  StrCpy $R6 "0"
  StrCpy $R7 "0"
  StrCpy $R8 "0.0.0"
  StrCpy $0 0
 
  loop:
 
  ;Get each sub key under "SOFTWARE\Microsoft\NET Framework Setup\NDP"
  EnumRegKey $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP" $0
  StrCmp $1 "" done ;jump to end if no more registry keys
  IntOp $0 $0 + 1
  StrCpy $2 $1 1 ;Cut off the first character
  StrCpy $3 $1 "" 1 ;Remainder of string
 
  ;Loop if first character is not a 'v'
  StrCmpS $2 "v" start_parse loop
 
  ;Parse the string
  start_parse:
  StrCpy $R1 ""
  StrCpy $R2 ""
  StrCpy $R3 ""
  StrCpy $R4 $3
 
  StrCpy $4 1
 
  parse:
  StrCmp $3 "" parse_done ;If string is empty, we are finished
  StrCpy $2 $3 1 ;Cut off the first character
  StrCpy $3 $3 "" 1 ;Remainder of string
  StrCmp $2 "." is_dot not_dot ;Move to next part if it's a dot
 
  is_dot:
  IntOp $4 $4 + 1 ; Move to the next section
  goto parse ;Carry on parsing
 
  not_dot:
  IntCmp $4 1 major_ver
  IntCmp $4 2 minor_ver
  IntCmp $4 3 build_ver
  IntCmp $4 4 parse_done
 
  major_ver:
  StrCpy $R1 $R1$2
  goto parse ;Carry on parsing
 
  minor_ver:
  StrCpy $R2 $R2$2
  goto parse ;Carry on parsing
 
  build_ver:
  StrCpy $R3 $R3$2
  goto parse ;Carry on parsing
 
  parse_done:
 
  IntCmp $R1 $R5 this_major_same loop this_major_more
  this_major_more:
  StrCpy $R5 $R1
  StrCpy $R6 $R2
  StrCpy $R7 $R3
  StrCpy $R8 $R4
 
  goto loop
 
  this_major_same:
  IntCmp $R2 $R6 this_minor_same loop this_minor_more
  this_minor_more:
  StrCpy $R6 $R2
  StrCpy $R7 $R3
  StrCpy $R8 $R4
  goto loop
 
  this_minor_same:
  IntCmp R3 $R7 loop loop this_build_more
  this_build_more:
  StrCpy $R7 $R3
  StrCpy $R8 $R4
  goto loop
 
  done:
 
  ;Have we got the framework we need?
  IntCmp $R5 ${MIN_FRA_MAJOR} max_major_same fail end
  max_major_same:
  IntCmp $R6 ${MIN_FRA_MINOR} max_minor_same fail end
  max_minor_same:
  IntCmp $R7 ${MIN_FRA_BUILD} end fail end
 
  fail:
  StrCmp $R8 "0.0.0" no_framework
  goto wrong_framework
 
  no_framework:
  MessageBox MB_YESNO|MB_ICONSTOP "Installation failed.$\n$\n\
         This software requires Windows Framework version \
         ${MIN_FRA_MAJOR}.${MIN_FRA_MINOR}.${MIN_FRA_BUILD} or higher.$\n$\n\
         No version of Windows Framework is installed.$\n$\n\
         Would you like to go to the Microsoft download page?" IDYES agree IDNO disagree
	 agree:
  	 execshell open "http://download.microsoft.com/download/2/0/E/20E90413-712F-438C-988E-FDAA79A8AC3D/dotnetfx35.exe"
  	 abort
	 disagree:
	 abort
 
 
  wrong_framework:
  MessageBox MB_YESNO|MB_ICONSTOP "Installation failed!$\n$\n\
         This software requires Windows Framework version \
         ${MIN_FRA_MAJOR}.${MIN_FRA_MINOR}.${MIN_FRA_BUILD} or higher.$\n$\n\
         The highest version on this computer is $R8.$\n$\n\
         Would you like to go to the Microsoft download page?" IDYES yes IDNO no
	 yes:
  	 execshell open "http://download.microsoft.com/download/2/0/E/20E90413-712F-438C-988E-FDAA79A8AC3D/dotnetfx35.exe"
  	 abort
	 no:
	 abort
 
  end:
 
  ;Pop the variables we pushed earlier
  Pop $R8
  Pop $R7
  Pop $R6
  Pop $R5
  Pop $R4
  Pop $R3
  Pop $R2
  Pop $R1
  Pop $4
  Pop $3
  Pop $2
  Pop $1
  Pop $0
 
FunctionEnd