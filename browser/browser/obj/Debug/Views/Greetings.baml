   M S B A M L   `   `   `  ����B  >browser, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:browserbrowser  5   	       browser.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:browser     5   	   -   5          �Name$  main_window��5   3    � Title$ Welcome��6	   $	��500��6   $
��1024��6&   .��5
      �  � UseLayoutRounding .  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $	��35* �6#   $��35��6/   $��16��6=   �� 5      $	��35* �6#   $��35��6/   $��16��6=   �� 5      $
��600* �6#   �� 5      $	��35* �6#   $��35��6/   $��16��6=   �� 5      $	��35* �6#   $��35��6/   $��16��6=   �� 5      $	��35* �6#   $��35��6/   $��16��6=   �� 5      $	��35* �6#   $��35��6/   $��16��6=   .+�5      �� -   5         ��Name$ button_back��5   %   $��0��6   $��5q�6:   
������ 
������  � ToolTip$ Go back one page��6�    J� Opacity$ 1Q�6�   .��5      �� .+�5      �� $X��RC:\Users\Ramachandran\Documents\Visual Studio 2015\Projects\browser\icons\back.png��6   5      5      �� 5       -   5        $ button_forward��5    %   $��1��6   $��5q�6=   
������ 
������ $ Go forward one page��6�   $ 1Q�6�   .��5!      �� .+�5"      �� $[��UC:\Users\Ramachandran\Documents\Visual Studio 2015\Projects\browser\icons\forward.png��6   $��24��6{   $��24��6�   5#      5$      �� 5&      -   5         ��Name$ cb��5&   '   $��2��6   $��	5,7.5,5,5q�63   $	��Top=�6F   	 �� 
IsEditable	 . 
 �� MaxDropDownHeight$	
 150��6�   6�   �� 5(      -   5        $ 	button_go��5(   %   $��3��6   $��5q�68   
������ 
������ $ Navigate to URL��6�   $ 1Q�6�   .��5)      �� .+�5*      �� $V��PC:\Users\Ramachandran\Documents\Visual Studio 2015\Projects\browser\icons\go.png��6   $��24��6v   $��24��6�   5+      5,      �� 5-      -   5        $ button_stop��5-   %   $��4��6   $��5q�6:   
������ 
������ $# Stop loading the current page��6�   $ 1Q�6�   .��5.      �� .+�5/      �� $X��RC:\Users\Ramachandran\Documents\Visual Studio 2015\Projects\browser\icons\stop.png��6   $��24��6x   $��24��6�   50      51      �� 52      -   5        $ button_home��52   %   $��5��6   $��5q�6:   
������ 
������ $ Go to homepage��6�   $ 1Q�6�   .��53      �� .+�54      �� $X��RC:\Users\Ramachandran\Documents\Visual Studio 2015\Projects\browser\icons\home.png��6   $��24��6x   $��24��6�   55      56      �� 57      -   5        $ button_refresh��57   %   $��6��6   $��5q�6=   
������ 
������ $< 6Click button or press Ctrl + R to refresh current page��6�   $ 1Q�6�   .��58      �� .+�59      �� $[��UC:\Users\Ramachandran\Documents\Visual Studio 2015\Projects\browser\icons\refresh.png��6   $��24��6{   $��24��6�   5:      5;      5<      ]� 5=   
   $��1��6   $��5q�6!   .+�5>      `� $
��Left=�6   $
��Left=�60   c� 5?      .@�5@      �� -	   5        $ button_favorites��5@      $��5,0,0,0q�63   $��	Favorites��6D   $ Opens the favorites menu��6X   6{   �� 5A      -
   5        $ button_close_other_tabs��5A      $ Close other tabs��6`   $ 1Q�6{   $��
Close tabs��6�   {�.  6�   5B      c� 5C      .@�5D      �� -   5        $ button_clear_history��5D      $��Clear history��67   $ Clears the history��6r   {�.  6�   �� 5E      -   5        $ button_clear_favorites��5E      $��Clear favorites��69   $ Clears the favorites��6x   {�.  $	��Top=�6�   5F      5G      �� 5H   
   -   5         ��Name$ tab_control��5H   "   $��2��6   $��Stretch=�67   $��10q�6U   $	��Top=�6a   
������ 
������ ��5I      �� 5J       �� 
TargetType" ��6"   .R�5K      �  5L      �� 5M      $
��Auto �6,   �� 5N      .+�5P      �� $
��Auto=�6'   $��Hidden=�6M   .��5Q      ��  ��Name$ HeaderPanel��6'    K� ZIndex$ 1��6<   -  'System.Windows.Input.KeyboardNavigation   TabIndex$ 1��6N   $��0��6n   $��0��6~   $��2,2,2,0q�6�    K� IsItemsHost . 6�   5R      �� 5S       ��Name$ PART_SelectedContentHost��6+    J� SnapsToDevicePixels# z 6M   #��z��6�   $��SelectedContent��6�   $��1��6�   5T      5U       � 	Resources 5W       �� 
TargetType �� Property �� Value ��Name �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName��%�  '��       �� 5X      " ��6   .�5Y      ��  � �� 5Z      �� 5[      " ��6.   .R�5\   "   �� $ header_border��6b   $ 5��6)   $��1q�6:   
���   �._�5]   &   �� $��Header��67   5^   '   5_   #    5`   "   P� 5a   &    �@��
IsSelected . . �5b   *   �� $ header_border��61    � ��
 ��ح�6b   5c   '   P� 5d   &    �@��
IsSelected .  . �5e   *   �� $ header_border��61    � ��
 �����6b   5f   '   5h      5j      5k      5l      5m      5n      5o      