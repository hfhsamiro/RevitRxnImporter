   M S B A M L   `   `   `  ����  RevitReactionImporterApp1    +RevitReactionImporter.BeamAnnotationToClear   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   $
��Auto��5   
   $	��360��6     � 	Resources  5         MyButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType � ActualWidth � ActualHeight �� CornerRadius  MyButtonStyle2��%A  &	        &	 ]    �� 5   
   " ��6&   .�5      ��  � �� 5      �� 5	      " ��6&   .R�5
      � #��z��6   .+�5      �� $��Center=�6/   $��Center=�5      �� 5      $��r1��6(   $	��0.5��62   
���   �6H   �� 5      $��border��6%   #��z 63   #��z 6Y   $ 2��6�   $��1q�6�   5      5      5      5      �� 5   
   " ��6'   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � #��z��6   .+�5      �� $��Center=�6/   $��Center=�5      5      5      5      5       5!      .��5"      � 
������� � RowDefinitions 5#   
   �� 5$      $
��Auto �6   �� 5%      $
��Auto �6   	 � ColumnDefinitions	 5'   
   �� 5(      $��1* �6   �� 5)      $��1* �6   �� 5*      $��1* �6   �� 5+      $
��0.5* �6   .+�5-   
   �� $��0��6   
 �� Orientation$
 
Horizontal=�5.      $��0,10,0,5q�5/      $��Center=�50      $��Center=�51      .+�52      �� -   5        $��ClearReactions��52      $��60��53      $��60��54      #��[  55      .��58      �� $>��8/RevitReactionImporterApp;component/Resources/import.png��6   $��50��59      $��60��5:      5;      5<      �� 5=   
   -   5        $��ClearReactionsText��5=   3   $��0��6   $��1��6%   $
��Auto��5>      $
��Auto��5?      $��	10,0,10,5q�5@      $��Clear Beam Reactions��5A       �� TextAlignment$ Center=�5B      $��9(�5C      $
��Wrap=�5D      �� 5E   
   $��0��6   $��1��5F      $
 
Horizontal=�5G      $��0,10,0,5q�5H      $��Center=�5I      $��Center=�5J      .+�5L      �� -   5        $��
ClearStuds��5L      $��60��5M      $��60��5N      #��[  5O      .��5Q      �� $<��6/RevitReactionImporterApp;component/Resources/stud.png��6   $��50��5R      $��60��5S      5T      5U      �� 5V   
   -   5        $��ClearStudCountsText��5V   3   $��1��6   $��1��6"   $
��Auto��5W      $
��Auto��5X      $��	10,0,10,5q�5Y      $��Clear Beam Stud Counts��5Z      $ Center=�5[      $��9(�5\      $
��Wrap=�5]      �� 5^   
   $��0��6   $��2��5_      $
 
Horizontal=�5`      $��0,10,0,5q�5a      $��Center=�5b      $��Center=�5c      .+�5d      �� -   5        $��ClearCamber��5d      $��60��5e      $��60��5f      #��[  5g      .��5j      �� $B��</RevitReactionImporterApp;component/Resources/deflection.png��6   $��50��5k      $��60��5l      5m      5n      �� 5o   
   -   5        $��ClearCamberText��5o   3   $��1��6   $��2��6"   $
��Auto��5p      $
��Auto��5q      $��	10,0,10,5q�5r      $��Clear Beam Camber Values��5s      $ Center=�5t      $��9(�5u      $
��Wrap=�5v      �� 5x   
   $��3��5y      $
 
Horizontal=�5z      $��2��6.   $��Center=�5{      $��Right=�5|      $��0,0,5,0q�5}      .+�5~      �� -   5         ��Name$# LoadBeamAnnotationsToClearBtn��5~      $
��Auto��5      $
��Auto��5�      
������ #��[ 5�      
������  � Cursor$
 Hand��5�      .��5�      �� $:��4/RevitReactionImporterApp;component/Resources/go.png��6   $��40��5�      $��40��5�      5�      5�      5�      5�      