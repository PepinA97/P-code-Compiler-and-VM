   M S B A M L   `   `   `  ����J  Fp-code compiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WpfAppWpfApp  5   	       WpfApp.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:WpfApp     5   	   -   5          �Name$  MainWindow1��5   	    � Title$ P-Code Compiler��5   	   $	��450��6!   $	��525��6.   
�������$	��525��6M   $	��450��6\   .��5	      � $��0,0,2,0q�6    � RowDefinitions 5
   
   �� 5       � ColumnDefinitions 5   
   �� 5      $
��399* �6   �� 5      $
��391* �6   .+�5   
   �� -   5         ��Name$ InputCodeBox��5       �� TextWrapping$
 Wrap=�6(   $��
10,80,0,10q�6<    �� AcceptsReturn .  �� 
AcceptsTab . 6w   �� 5   
   -   5        $ InputMachineBox��5      $
 Wrap=�6+   $��	10,54,0,0q�6?    .  . $��21��6�   $	��Top=�6�   �� 5   
   -   5         ��Name$ 	OutputBox��5      $
��Wrap=�6'   $��1��6;   $��10q�6K   ���	#FFEEEEEE6W   �� 5   
   -   5        	 ��Name$	 CompileButton��5      $��Compile��6(   $
��Left=�6:   $	��Top=�6U   $	��120��6m   $��	10,10,0,0q�6y   $��35��6�   6�   �� 5   
   -   5        $	 	RunButton��5      $	��Run��6$   $
��Left=�62   $��
135,10,0,0q�6M   $	��Top=�6a   $	��120��6y   $��35��6�   6�   5      5      