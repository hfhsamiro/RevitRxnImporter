   M S B A M L   `   `   `  ����  RevitReactionImporterAppC    =RevitReactionImporter.AnnotationTypeSelectionForVisualization   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$,  &Choose Annotation Type to Visualize...��5   	   $	��150��68   $	��360��6E   .��5      � 
������� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��50 �6    � ColumnDefinitions 5
   
   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�5      .+�5      �� -   5        $��VisualizeRAMReactions��5      $��60��5      $��60��5      $��15 10 10 10q�5      $
��Left=�5      $	��Top=�5      .��5      �� $>��8/RevitReactionImporterApp;component/Resources/import.png��6   $��50��5      $��60��5      5      5      �� 5   
   -   5        $��VisualizeReactionsText��5   3   $��0��6   $��1��6%   $
��Auto��5      $
��Auto��5       $��
10,0,10,10q�5!      $��Visualize Beam Reactions��5"       �� TextAlignment$ Center=�5#      $��9(�5$      $
��Wrap=�5%      �� 5&   
   $��1��6   $��0��6&   $ 
Horizontal=�5'      .+�5(      �� -   5        $��VisualizeRAMSizes��5(      $��60��5)      $��60��5*      $��10 10 10 10q�5+      $
��Left=�5,      $	��Top=�5-      .��5/      �� $<��6/RevitReactionImporterApp;component/Resources/beam.png��6   $��50��50      $��60��51      52      53      �� 54   
   -   5        $��VisualizeSizesText��54   3   $��1��6   $��1��6%   $
��Auto��55      $
��Auto��56      $��
10,0,10,10q�57      $��Visualize Beam Sizes��58      $ Center=�59      $��9(�5:      $
��Wrap=�5;      �� 5<   
   $��0��6   $��2��5=      $ 
Horizontal=�5>      .+�5?      �� -   5        $��VisualizeRAMStuds��5?      $��60��5@      $��60��5A      $��10 10 10 10q�5B      $
��Left=�5C      $	��Top=�5D      .��5F      �� $<��6/RevitReactionImporterApp;component/Resources/stud.png��6   $��50��5G      $��60��5H      5I      5J      �� 5K   
   -   5        $��VisualizeStudCountsText��5K   3   $��1��6   $��2��6"   $
��Auto��5L      $
��Auto��5M      $��
10,0,10,10q�5N      $ ��Visualize Beam Stud Counts��5O      $ Center=�5P      $��9(�5Q      $
��Wrap=�5R      �� 5S   
   $��0��6   $��3��5T      $ 
Horizontal=�5U      .+�5V      �� -   5        $��VisualizeRAMCamber��5V      $��60��5W      $��60��5X      $��10 10 15 10q�5Y      $
��Left=�5Z      $	��Top=�5[      .��5]      �� $B��</RevitReactionImporterApp;component/Resources/deflection.png��6   $��50��5^      $��60��5_      5`      5a      �� 5b   
   -	   5        $��VisualizeCamberText��5b   3   $��1��6   $��3��6"   $
��Auto��5c      $
��Auto��5d      $��
10,0,15,10q�5e      $"��Visualize Beam Camber Values��5f      $ Center=�5g      $��9(�5h      $
��Wrap=�5i      5j      5k      