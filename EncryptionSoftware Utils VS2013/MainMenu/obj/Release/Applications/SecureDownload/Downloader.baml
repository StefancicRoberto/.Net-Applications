   M S B A M L   `   `   `  ����   ControlCenter:    4ControlCenter.Applications.SecureDownload.Downloader   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    -     �Name$  DownloaderWindow�� � Title$ 
Downloader��$	��350��$	��600�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � Icon$: 4/ControlCenter;component/Resources/btnDownloader.png��.���  � ColumnDefinitions �� .+��� $��Simple Download��$
��Left=�$��	10,10,0,0q�$	��Top=�$	��134��$	��574�����	#FF28A9C5
���   �$
��Bold"�$��2q�.��� $
��Left=�$	��112��$	��Top=�$	��541��$��	10,0,-2,0q�.+��� -    ��Name$ txtURL��$
��Left=�$��23��$��	66,10,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��465��
���   �$��2q��� $��	File URL:��$
��Left=�$��6,7,0,0q�$	��Top=�� -    �Name$ progressDownload��$
��Left=�$��17.21��$��
175,38,0,0q�$	��Top=�$	��209�����	#FFE8E8E8���	#FF40E410
���   ��� -   	 ��Name$	 btnDownload��$��Download��$
��Left=�$��175,84.05,0,0q�$	��Top=�$��75��
���   �
���   �
�������$
��Bold"�$��2q��� -   $	 	btnCancel��$��Cancel��$
��Left=�$��309,84.05,0,0q�$	��Top=�$��75��
���   �
���   �
�������$
��Bold"�$��2q��� -   
 ��Name$
 lblProgress��$�� ��$
��Left=�$��240,55.21,0,0q�$	��Top=�$��27.21�����	#FF2AD619�� $��Secure Download��$
��Left=�$��
10,159,0,0q�$	��Top=�$	��574��$	��152�����	#FF28A9C5
���   �$
��Bold"�$��2q�.��� $
��Left=�$	��133��$��	10,0,-2,0q�$	��Top=�$	��541�� �� $
��269* ��� $
��247* ��� $	��25* �.+��� -   $ txtURLSecure��$
��Left=�$��23��$��	66,10,0,0q�$
 Wrap=�$	��Top=�$	��465��
���   �$��2q�$��3���� $��	File URL:��$
��Left=�$��6,7,0,0q�$	��Top=�� -   $ progressDownloadSecure��$
��Left=�$��17.21��$��
175,38,0,0q�$	��Top=�$	��209�����	#FFE8E8E8���	#FFF94646
���   �$��2���� -	   $	 btnDownloadSecure��$��Download��$
��Left=�$��175,96.15,0,0q�$	��Top=�$��75��
���   �
���   �
�������$
��Bold"�$��2q��� -
   $	 btnCancelSecure��$��Cancel��$
��Left=�$��40,96.15,0,0q�$	��Top=�$��75��
���   �
���   �
�������$
��Bold"�$��2q�$��1���� -   $ txtProgressSecure��$
��Left=�$��20��$��66,65.1,0,0q�$
 Wrap=�$	��Top=�{�.  
����������	#FFF94646$
��Bold"�$��11(�$��3��$	��465���� $��Status:��$
��Left=�$��10,65.1,0,0q�$	��Top=�$
��Bold"�$��11(�