   M S B A M L   `   `   `  ���� K  GEntityGarbageWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:EntityGarbageWPFEntityGarbageWPF  !    EntityGarbageWPF.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:EntityGarbageWPF       � Title$  
MainWindow��$	��350��$	��525��.��� .+�&   System.Windows.Controls.DataGrid  -     Name$ dataGrid��$
��Left=�$��	10,45,0,0q�$	��Top=�$	��158��$	��392��   AutoGenerateColumns .  ���� *Accessories+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Category��   CanUserResize .     Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *
Category+  $
 Name��$	 160  �� *Name+  $ Amount�� �� *Amount+  $
 Min.��$ 56  �� *	MinAmount+   RowStyle �� 	 �� 
TargetType)  #System.Windows.Controls.DataGridRow"	  
 �� Triggers
 ��  �� Binding �� *
isEnough+ �� Value True.a���  �� Property � �� �� Value
 �  ����  �� *
isEnough+	 False.a���  � ��
 ������� -    ��Name$ newAccessoryButton��$	��New��$
��Left=�$��
432,36,0,0q�$	��Top=�$��75���� -   $ updateAccessoryButton��$
��Edit��$
��Left=�$��
432,63,0,0q�$	��Top=�$��75���� -   $ deleteAccessoryButton��$��Delete��$
��Left=�$��
432,90,0,0q�$	��Top=�$��75���� -   $ acceptButton��$��Accept��$
��Left=�$��
10,225,0,0q�$	��Top=�$��75���� -   $ handoverButton��$��	Hand Over��$
��Left=�$��111,225,0,0q�$	��Top=�$��75��