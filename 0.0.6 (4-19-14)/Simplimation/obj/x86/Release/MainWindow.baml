   M S B A M L   `   `   `  ����   Simplimation    Simplimation.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$*  $New Project (Unsaved) - Simplimation�� � Icon$% /Simplimation;component/bar.ico�� � WindowStartupLocation$ CenterScreen=���. $��Center=�$
��Auto=�$
��Auto=�$��Center=� � UseLayoutRounding .  $
��1024��$	��680��.��� -   
������� � IsSharedSizeScope . .+�� -    �Name$ 
AboveBench��$��60��$��Bottom=�$��	0,0,0,276q�$��0�����	#FF3B3B3B � ColumnDefinitions �� $	��102 ��� $	��107 ��� .+��� -    ��Name$ delayt��$��Delay��$��Center=�$��15(�
�������$��	30,0,24,0q�$��48���� -    ��Name$ DelaySet��$��18��$��Bottom=�$��	0,0,24,10q�$��0��	 �� TextAlignment$	 Center=�{�.  
 �� AcceptsReturn
 . $��Right=�$��48���� -    ��Name$ Moveback��$��1��$��48��$
��Left=�$��0,6,0,0q�$
��Fill=�$	��Top=�$��48��$3��-/Simplimation;component/graphics/frame_b1.png���� -   $ forward��$��48��$
��Left=�$��48,6,0,0q�$
��Fill=�$	��Top=�$��48��$��1��$3��-/Simplimation;component/graphics/frame_f1.png���� -   $ con_beg��$��48��$
��Left=�$��1,6,0,0q�$.��(/Simplimation;component/graphics/rev.png��$
��Fill=�$	��Top=�$��48��$��2���� -	   $ Play_Button��$��2��$��48��$
��Left=�$��54,6,0,0q�$
��Fill=�$	��Top=�$��48��$1��+/Simplimation;component/graphics/play_i.png���� -
   $ 
Play_Point��$��48��$
��Left=�$��graphics/playbeg.png��$
��Fill=�$	��Top=�$��48��$��2��$��	107,6,0,0q��� -   $ con_end��$��48��$
��Left=�$��	160,6,0,0q�$��graphics/for.png��$
��Fill=�$	��Top=�$��48��$��2���� -   $ Loop_button��$��2��$��48��$
��Left=�$��	213,6,0,0q�$
��Fill=�$	��Top=�$��48��$��graphics/looper0.png��� -   $ grid1�����	#FF212121$	��480��$	��336��$��400,20,0,276q�$��Right=�$	��Top=� � RowDefinitions �� $��64��$��64���� $	��160���� $��64��$��64�� �� $��64��$��64���� �� �� $��64��$��64��.+��� -   $ Ratio��$��1��$��1��$��Uniform=�$,��&/Simplimation;component/Images/169.png��$��2���� -   $	 Big��$��1��$��1��$��Uniform=�$1��+/Simplimation;component/graphics/nonbut.png��$��2���� -    ��Name$ 	TimeShown��$��1��$��50��$��Center=�$��
97,10,90,0q�$��	00:00.000��$	��Top=�$��2��$	��213��$��36(� �� TextAlignment$ Center=�$
��Bold"�
��������� -    ��Name$ 
statusBar1��$��23��$��	0,435,0,0q�$��Bottom=�$��Bottom=��� -   $ Add_it��$��96��$
��Left=�$��
10,160,0,0q�$	��Top=�$��96��$4��./Simplimation;component/graphics/new_image.png���� -   $	 rem��$��96��$
��Left=�$��111,160,0,0q�$.��(/Simplimation;component/graphics/rem.png��$	��Top=�$��96��o� -    o�Name$ MainMenu��$��20��$��
0,0,-238,0q�$	��Top=�$
��1920��$
��1920��$	��Top=�$
��Left=�.@�m� -    m�Name$ F_Menu��$
��File��.@�m� -   $��New Project��m� -   $
 open��$��Open Project...��m� -   $
 save��$��Save Project���� m� -   $
 exit��$
��Exit��m� -   $ E_Menu��$
��Edit��.@�m� -   $ AddIM��$ ��Add Image(s) to Project...��m� $��Remove Selected Frame(s)��m� -   $ S_Menu��$��Settings��.@�m� $��Aspect Ratio��.@�m� $��16:9 Widescreen�� m� IsCheckable .  m� 	IsChecked . m� $��4:3 Standard�� . m� $��
1:1 Square�� . m� -   $ About��$��About��.@�m� -   $ T_Menu��$��
About Team���� $��Disabled=�$��0,0,0,26q�$	��250��$��Bottom=����	#FF484848��. .��� -    �Name$ 	CineBench�� � 	ItemWidth$	 164�� � 
ItemHeight$	 120��$	��Top=�$
��Left=�$
��Auto��$	��900��$
��Auto���� $��
Storyboard��$
��Left=�$��
10,129,0,0q�$	��Top=��� $��Project��$
��Left=�$��	10,25,0,0q�$	��Top=��� -    $ new_docu��$��96��$
��Left=�$��	10,51,0,0q�$	��Top=�$��96��$��graphics/new_docu.png���� -!   $ 	Open_proj��$��96��$
��Left=�$��
111,51,0,0q�$	��Top=�$��96��$��graphics/open_docu.png���� -"   $ 	Save_proj��$��96��$
��Left=�$��
212,51,0,0q�$	��Top=�$��96��$��graphics/save_docu.png���� -#   $ AVI_exp��$��96��$
��Left=�$��
313,51,0,0q�$	��Top=�$��96��$��graphics/save_avi.png��