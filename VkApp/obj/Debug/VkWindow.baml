   M S B A M L   `   `   `  ����@  <VkApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:VkAppVkApp  5   	       VkApp.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:VkApp     5   	   -   5          � Icon$  Icons\vkLogo.png��5   	    � Title$ VK-Рассылка��5	   	   $	��250��6   $	��400��6*   ���	#FF345EB8 � WindowStartupLocation$ CenterScreen=�5
   	   .��5      �  � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��0.25* �6   �� 5      $
��0.4* �6   .+�5   
   �� 
����ح�$��2q�6)   $��5q�6=   ._�5      �� -   5         ��Name$ 	tbMessage��5      $��0��5      $��13(�5      $��0q�5       �� AcceptsReturn .  �� 
AcceptsTab . 5      5      �� 5   
   $��1��6   $��
5, 1, 5, 1q�6"   .+�5      	� -   5         	�Name$ rbtnAdd��5   l   $J��DЗапустить ботов на добавление друзей��6    d� 	IsChecked$
 TrueY�6I   $
��Bold"�6Z   6}   	� 5      -   5        $ rbtnSend��5   d   $[��UПроверить наличие друзей и выполнить рассылку��6   $
��Bold"�6R   6v   5      ]� 5   
   $��2��6   .+�5      � 	 � ColumnDefinitions	 5       �� 5!      $��1* �6'   �� 5"      $��1* �6'   .+�5%      �� 
 �� Orientation$
 
Horizontal=�6   .+�5&      �� -   5         ��Name$ cbGames��5(      $��5, 15, 5, 10q�5&      $	��120��5'       �� SelectedIndex$ 0��5)      5*      5+      �� 5-      -   5         ��Name$ btnStart��50      $	��100��5-      $��0, 15, 5, 15q�5.      $��Start��5/      $��1��52      $��50��53      $��Right=�54      55      56      57      58      