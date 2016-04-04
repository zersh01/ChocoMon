;�������� ���� ������ ���������� 
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles


; myprog.nsi
;
; ��� ��������� ������������� myprog � ������� \Program Files\mp
;------------------------------------------------------------------------

; ��� ������������
Name "ChocoMon"
; ������� Name ������������� ��� ������������. ��� - ��� ������ ��� 
; ��������, �������� 'MyApp' ��� 'CrapSoft MyApp'.
;------------------------------------------------------------------------
; ���� ��� ������. ��� ��� ����� ����� ��� �����������.
OutFile "ChocoMon_1.0.0.1_Setup.exe"

;------------------------------------------------------------------------
; ������� InstallDir ���������� ���������� ��� ��������� ���������. ����
; ������������ � ���������� $INSTDIR. ���� ����� ������������� ���� 
; ���������. ���������� $PROGRAMFILES ���������� 
; ���� � �������� Program files
InstallDir "$PROGRAMFILES\ChocoMon"
;------------------------------------------------------------------------
; �������� ����� ������� ��� ����������, � ������� ����� ���������������
; ��������� (��� ��������� ��������� ������ ���� ����� ��������� 
; �������������). mp - �������, ���� ��������������� ���������
InstallDirRegKey HKLM SOFTWARE\ChocoMon "Install_Dir"
;------------------------------------------------------------------------
; ���� ������������ ������ ��������� �������� ��� ���������. Myprog - 
; ��� �������, ������������ �������� Name.
Section "ChocoMon (required)"
;------------------------------------------------------------------------
  SectionIn RO
  ; ��� ������� ���������� ��� ��������� (��. InstType) �������� �������.
;------------------------------------------------------------------------
  ; ���������� ���� ��� ���������� ���������.
  SetOutPath $INSTDIR
;------------------------------------------------------------------------
  ; ��������� �������������� ������, ������� ����� ���������������
  ; �������������. ������� File ���������� ���� � ����� ��� ��������
  File /r "D:\YandexDisk\backup\Chocolatey\MyProject\ChocoMon\ChocoMon\ChocoMon\bin\Release\"   
  ; �������� /r ��������, ��� ������� DATA ����� ������������ ������ �  
  ; ������������ � ��� �������



;------------------------------------------------------------------------
  ; �������� ���� ����������� � ������. mp - �������, 
  ; ���� ��������������� ���������
  WriteRegStr HKLM SOFTWARE\ChocoMon "Install_Dir" "$INSTDIR"
;------------------------------------------------------------------------
; �������� ����� ������������� ��� Windows. Myprog - ��� �������, 
; ������������ �������� Name. ����� "������ ��������" �������������. ���
; ���������� ������������ � ������ ������������� ��������, ����� �� 
; ��������� ������ ���������� -> ��������� � �������� ��������
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ChocoMon" "DisplayName" " ChocoMon (������ ��������)"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ChocoMon" "UninstallString" '"$INSTDIR\uninstall.exe"'
;------------------------------------------------------------------------
  WriteUninstaller "uninstall.exe"
  ; ����������� ������������� (� ������������� - ����) ��� �������������
  ; �������� File �����.
;------------------------------------------------------------------------
SectionEnd
; ��� ������� ��������� ������� �������� ������
;------------------------------------------------------------------------
; ������ ����� (����� ���� ��������� �������������). ������� ������
Section "Start Menu Shortcuts"

  ; ������� CreateDirectory ������� ������� �� ���������� ����.
  ; ���������� $SMPROGRAMS ���������� ���� � ������ ���� ���� -> 
  ; ���������, �.�. �� ������� ������� myprog � ���� ���� -> ���������
  CreateDirectory "$SMPROGRAMS\ChocoMon"

  ; � ���� �������� ������� ������ � ������� ������� CreateShortCut:
  ; ����� Uninstall - �������������
  CreateShortCut "$SMPROGRAMS\ChocoMon\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0

  ; ����� myprog - ���������
  CreateShortCut "$SMPROGRAMS\ChocoMon\ChocoMon.lnk" "$INSTDIR\ChocoMon.exe" "" "$INSTDIR\ChocoMon.exe" 0
  ; ���������� $DESKTOP ���������� ���� � �������� �����, �.�. ��
  ; ������� ����� �� ������� �����. 
  CreateShortCut "$DESKTOP\ChocoMon.lnk" "$INSTDIR\ChocoMon.exe" "" "$INSTDIR\ChocoMon.exe" 0


SectionEnd
;------------------------------------------------------------------------
; �������������. ��� ���������� ��������� ��� �������� ���������
UninstallText "��������� myprog ����� ������� � ������ ����������. ������� Uninstall, ����� ����������." "������� ��������� ��:"
;------------------------------------------------------------------------
; ������ �������������
Section "Uninstall"
  
  ; ������� ����� �� �������
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ChocoMon"

  DeleteRegKey HKLM SOFTWARE\ChocoMon

  ; ������� ����� � �������������
  Delete $INSTDIR\makensisw.exe
  Delete $INSTDIR\uninstall.exe

  ; ������� ������: ��������� ������� � ���� ����\��������� (myprog).
  ; *.* - ��� ������, ��� �� ���������� �������� ����� ������� ��� �����
  Delete "$SMPROGRAMS\ChocoMon\*.*"
  RMDir "$SMPROGRAMS\ChocoMon"       ; ������� ������� myprog �� ���� ���������
  Delete "$DESKTOP\ChocoMon.lnk"     ; ������� ����� � �������� �����

  ; ������� �������������� �����
  RMDir /r "$PROGRAMFILES\ChocoMon"
  ; /r - � ���� ���������� ������� ���������, ���� ���� �� �� ������.
  
SectionEnd
; ���������� - �������� ���� �����.
