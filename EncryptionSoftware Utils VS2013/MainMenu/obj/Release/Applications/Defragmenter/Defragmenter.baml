   M S B A M L   `   `   `  ����   ControlCenter:    4ControlCenter.Applications.Defragmenter.Defragmenter   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    -     �Name$  DefragmenterWindow�� � Title$ Defragmenter��$	��300��$	��600�� � 
ResizeMode$ NoResize=� � Icon$< 6/ControlCenter;component/Resources/btnDefragmenter.png�� � WindowStartupLocation$ CenterScreen=�.���  � ColumnDefinitions �� $
��100* ��� $	��71* ��� $
��423* �.+��� $��Choose the disk: ��$
��Left=�$��21.333,22.05,0,0q�$	��Top=�$��100.667��$��Italic$�$
��Bold"�$��2���� -    ��Name$ lbDrives��$
��Left=�$��18��$��21.333,50.05,0,0q�$	��Top=�$	��100��$
��Bold"����	#FF28A9C5$��2q�$��2���� -    ��Name$ btnStart��$��Start��$
��Left=�$��143.032,22.05,0,0q�$	��Top=�$��75��
�������$
��Bold"�
���   �$��2q�$��2���� -   $ btnAbort��$��Abort��$
��Left=�$��285.604,22.05,0,0q�$	��Top=�$��75��
�������
���   �
���   �$
��Bold"�$��2q�$��2���� -   $ lbOutput��$
��Left=�$��166.667��$��43.179,50.05,0,0q�$	��Top=�$	��425�����	#FF28A9C5$��2q�$��1��$��2���� $��Output��$
��Left=�$��43.179,221.717,0,0q�$	��Top=�$��52.667��$
��Bold"�$��Italic$�$��1��$��2���� -   $ 
btnAnalyze��$��Analyze��$
��Left=�$��0,22.05,0,0q�$	��Top=�$��75��
�������
���   �$
��Bold"�$��2q�$��2��